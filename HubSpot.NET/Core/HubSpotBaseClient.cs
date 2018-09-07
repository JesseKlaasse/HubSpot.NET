using System;
using System.Collections.Generic;
using Flurl;
using HubSpot.NET.Core.Interfaces;
using HubSpot.NET.Core.Requests;
using Newtonsoft.Json;
using RestSharp;

namespace HubSpot.NET.Core
{
    public class HubSpotBaseClient : IHubSpotClient
    {
        private readonly RequestSerializer _serializer = new RequestSerializer(new RequestDataConverter());
        private readonly RestClient _client;

        private string _baseUrl => "https://api.hubapi.com";
        private readonly string _apiKey;

        public HubSpotBaseClient(string apiKey)
        { 
            _apiKey = apiKey;
            _client = new RestClient(_baseUrl);
        }

        public T Execute<T>(string absoluteUriPath, T entity = default, Method method = Method.GET, bool convertToPropertiesSchema = true)
        {
            string json = null;
            if (entity != default)
                json = _serializer.SerializeEntity(entity, convertToPropertiesSchema);
            return SendRequest(absoluteUriPath, method, json, responseData => _serializer.DeserializeEntity<T>(responseData, convertToPropertiesSchema));
        }

        public void Execute(string absoluteUriPath, object entity = null, Method method = Method.GET, bool convertToPropertiesSchema = true)
        {
            var json = _serializer.SerializeEntity(entity, convertToPropertiesSchema);
            
            SendRequest(absoluteUriPath, method, json);
        }

        public void ExecuteBatch(string absoluteUriPath, List<object> entities, Method method = Method.GET, bool convertToPropertiesSchema = true)
        {
            var json = _serializer.SerializeEntity(entities, convertToPropertiesSchema);

            SendRequest(absoluteUriPath, method, json);
        }

        public T ExecuteMultipart<T>(string absoluteUriPath, byte[] data, string filename, Dictionary<string,string> parameters, Method method = Method.POST)
        {
            var fullUrl = $"{_baseUrl}{absoluteUriPath}".SetQueryParam("hapikey", _apiKey);

            var request = new RestRequest(fullUrl, method);

            request.AddFile(filename, data, filename);

            foreach (var kvp in parameters)
            {
                request.AddParameter(kvp.Key, kvp.Value);
            }

            var response = _client.Execute(request);
            if (!response.IsSuccessful())
                throw new HubSpotException("Error from HubSpot", response.Content); // lettuce get some good exception info back

            var responseData = JsonConvert.DeserializeObject<T>(response.Content);         

            return responseData;
        }

        public T ExecuteList<T>(string absoluteUriPath, object entity = null, Method method = Method.GET, bool convertToPropertiesSchema = true)
        {
            var json = _serializer.SerializeEntity(entity);

            return SendRequest(
                absoluteUriPath,
                method,
                json,
                responseData => _serializer.DeserializeListEntity<T>(responseData, convertToPropertiesSchema));            
        }

        private T SendRequest<T>(string path, Method method, string json, Func<string, T> deserializeFunc) 
        {
            string responseData = SendRequest(path, method, json);

            if (string.IsNullOrWhiteSpace(responseData))
                return default;            

            return deserializeFunc(responseData);
        }

        private string SendRequest(string path, Method method, string json)
        {
            string url = $"{path}".SetQueryParam("hapikey", _apiKey);

            RestRequest request = new RestRequest(url, method);

            if (!string.IsNullOrWhiteSpace(json))
            {
                request.AddParameter("application/json", json, ParameterType.RequestBody);
            }

            IRestResponse response = _client.Execute(request);

            string responseData = response.Content;

            if (!response.IsSuccessful())            
                throw new HubSpotException("Error from HubSpot", responseData);            

            return responseData;
        }
        
    }
}