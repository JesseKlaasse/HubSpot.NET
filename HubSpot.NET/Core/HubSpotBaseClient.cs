using System.Threading;
using System.Threading.Tasks;

namespace HubSpot.NET.Core
{
    using HubSpot.NET.Api.OAuth.Dto;
    using HubSpot.NET.Core.Interfaces;
    using HubSpot.NET.Core.Serializers;
    using Newtonsoft.Json;
    using RestSharp;
    using System.Collections.Generic;

    public class HubSpotBaseClient : IHubSpotClient
    {
        private readonly RestClient _client;

        private string _baseUrl => "https://api.hubapi.com";
        private readonly HubSpotAuthenticationMode _mode;
        
        public string BasePath { get => _baseUrl; }

        // Used for HAPIKEY method
        private readonly string _apiKeyName = "hapikey";
        private readonly string _apiKey;

        // Used for OAUTH
        public string AppId { get; private set; }
        private HubSpotToken _token;

        /// <summary>
        /// Creates a HubSpot client with the specified authentication scheme (Default: HAPIKEY). 
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="mode"></param>
        public HubSpotBaseClient(string apiKey, HubSpotAuthenticationMode mode = HubSpotAuthenticationMode.HAPIKEY, string appId = "", HubSpotToken token = null)
        { 
            _apiKey = apiKey;
            _client = new RestClient(_baseUrl);
            _mode = mode;
            _token = token;
            AppId = appId;
        }

        public Task<T> ExecuteAsync<T>(string path, Method method = Method.GET, CancellationToken cancellationToken = default) where T : new() 
            => SendReceiveRequestAsync<T>(path, method, cancellationToken);

        public Task<T> ExecuteAsync<T, K>(string absoluteUriPath, K entity, Method method = Method.GET, CancellationToken cancellationToken = default) where T : new() 
            => SendReceiveRequestAsync<T, K>(absoluteUriPath, method, entity, cancellationToken);

        public Task ExecuteOnlyAsync(string absoluteUriPath, Method method = Method.GET, CancellationToken cancellationToken = default) 
            => SendOnlyRequestAsync(absoluteUriPath, method, cancellationToken);

        public Task ExecuteOnlyAsync<T>(string absoluteUriPath, T entity, Method method = Method.GET, CancellationToken cancellationToken = default) 
            => SendOnlyRequestAsync(absoluteUriPath, method, entity, cancellationToken);

        public Task ExecuteBatchAsync(string absoluteUriPath, IEnumerable<object> entities, Method method = Method.GET, CancellationToken cancellationToken = default) 
            => SendOnlyRequestAsync(absoluteUriPath, method, entities, cancellationToken);

        public async Task<T> ExecuteMultipartAsync<T>(string absoluteUriPath, byte[] data, string filename, Dictionary<string,string> parameters, Method method = Method.POST, CancellationToken cancellationToken = default)
        {
            var fullUrl = $"{_baseUrl}{absoluteUriPath}";
            var request = ConfigureRequestAuthentication(fullUrl, method);

            request.AddFile(filename, data, filename);
        
            foreach (var kvp in parameters)
            {
                request.AddParameter(kvp.Key, kvp.Value);
            }

            var response = await _client.ExecuteTaskAsync(request, cancellationToken);
            if (!response.IsSuccessful)
                throw new HubSpotException("Error from HubSpot", response.Content); // lettuce get some good exception info back

            return JsonConvert.DeserializeObject<T>(response.Content);         
        }

        /// <summary>
        /// Updates the OAuth token used by the client.
        /// </summary>
        /// <param name="token"></param>
        public Task UpdateTokenAsync(HubSpotToken token, CancellationToken cancellationToken = default)
        {
            _token = token;
            return Task.CompletedTask;
        }

        #region 'private methods'
        /// <summary>
        /// Sends requests to the given endpoint and returns an entity object of T.
        /// </summary>
        /// <typeparam name="T">The expected return entity type.</typeparam>
        /// <param name="path">The path to the endpoint.</param>
        /// <param name="method">The REST method used.</param>
        /// <returns>An entity of type T returned from the request.</returns>
        private async Task<T> SendReceiveRequestAsync<T>(string path, Method method, CancellationToken cancellationToken = default) where T : new()
        {
            RestRequest request = ConfigureRequestAuthentication(path, method);
            IRestResponse<T> response = await _client.ExecuteTaskAsync<T>(request, cancellationToken);

            if (response.IsSuccessful == false)
                throw new HubSpotException("Error from HubSpot", new HubSpotError(response.StatusCode, response.StatusDescription), response.Content);

            return JsonConvert.DeserializeObject<T>(response.Content);
        }

        /// <summary>
        /// Sends requests with a given entity JSON body to the target endpoint and returns a result object.
        /// </summary>
        /// <typeparam name="T">The type for the return object.</typeparam>
        /// <typeparam name="K">The type of the sending object.</typeparam>
        /// <param name="path">The path to the endpoint.</param>
        /// <param name="method">The REST method used.</param>
        /// <param name="entity">The entity being sent in the request.</param>
        /// <returns>An entity of type T returned from the request.</returns>
        private async Task<T> SendReceiveRequestAsync<T,K>(string path, Method method, K entity, CancellationToken cancellationToken = default) where T: new()
        {
            RestRequest request = ConfigureRequestAuthentication(path, method);
           
            if(entity != null)
                request.AddJsonBody(entity);
            
            //var body = request.JsonSerializer.Serialize(entity); // uncomment this for body debugging

            IRestResponse<T> response = await _client.ExecuteTaskAsync<T>(request, cancellationToken);

            if (response.IsSuccessful == false)
                throw new HubSpotException("Error from HubSpot", new HubSpotError(response.StatusCode, response.StatusDescription), response.Content);


            return JsonConvert.DeserializeObject<T>(response.Content);
        }

        /// <summary>
        /// Sends a one way request to the server with no return data.
        /// </summary>
        /// <typeparam name="T">The outbound entity type.</typeparam>
        /// <param name="path">The endpoint target.</param>
        /// <param name="method">The REST method to use.</param>
        /// <param name="entity">The entity being sent to the endpoint.</param>
        private async Task SendOnlyRequestAsync<T>(string path, Method method, T entity, CancellationToken cancellationToken = default)
        {

            RestRequest request = ConfigureRequestAuthentication(path, method);

            if (entity != null)
                request.AddJsonBody(entity);

            //var body = request.JsonSerializer.Serialize(entity); // uncomment this for body debugging

            IRestResponse response = await _client.ExecuteTaskAsync(request, cancellationToken);

            if (!response.IsSuccessful)
                throw new HubSpotException("Error from HubSpot", new HubSpotError(response.StatusCode, response.StatusDescription), response.Content);
        }

        /// <summary>
        /// Sends a one way request to the server with no return data.
        /// </summary>
        /// <param name="path">The endpoint target.</param>
        /// <param name="method">The REST method to use.</param>
        private async Task SendOnlyRequestAsync(string path, Method method, CancellationToken cancellationToken = default)
        {

            RestRequest request = ConfigureRequestAuthentication(path, method);

            IRestResponse response = await _client.ExecuteTaskAsync(request, cancellationToken);

            if (!response.IsSuccessful)
                throw new HubSpotException("Error from HubSpot", new HubSpotError(response.StatusCode, response.StatusDescription), response.Content);
        }
        /// <summary>
        /// Configures a RestRequest based on the authentication scheme detected and configures the endpoint path relative to the base path.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        private RestRequest ConfigureRequestAuthentication(string path, Method method)
        {
            string fullPath = $"{BasePath.TrimEnd('/')}/{path.Trim('/')}";
            RestRequest request = new RestRequest(fullPath, method, DataFormat.Json);
            switch(_mode)
            {
                case HubSpotAuthenticationMode.OAUTH:
                    request.AddHeader("Authorization", GetAuthHeader(_token));
                    break;
                default:
                    request.AddQueryParameter(_apiKeyName, _apiKey);
                    break;
            }

            request.JsonSerializer = new NewtonsoftRestSharpSerializer();  
            return request;
        }

        private string GetAuthHeader(HubSpotToken token) 
            => $"Bearer {token.AccessToken}";
        #endregion
    }
     
    public enum HubSpotAuthenticationMode
    {
        HAPIKEY, OAUTH
    }
}