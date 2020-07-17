using System.Threading;
using System.Threading.Tasks;

namespace HubSpot.NET.Api.EmailSubscriptions
{
    using HubSpot.NET.Api.EmailSubscriptions.Dto;
    using HubSpot.NET.Core.Abstracts;
    using HubSpot.NET.Core.Dictionaries;
    using HubSpot.NET.Core.Interfaces;
    using RestSharp;
    using System.Collections.Generic;
    using System.Linq;

    public class HubSpotEmailSubscriptionsApi : ApiRoutable, IHubSpotEmailSubscriptionsApi
    {
        private readonly IHubSpotClient _client;
        public override string MidRoute => "/email/public/v1/subscriptions";

        public HubSpotEmailSubscriptionsApi(IHubSpotClient client)
        {
            _client = client;
            AddRoute<SubscriptionTimelineHubSpotModel>("timeline");
        }

        public Task<SubscriptionTypeListHubSpotModel> GetSubscriptionTypesAsync(CancellationToken cancellationToken = default) 
            => _client.ExecuteAsync<SubscriptionTypeListHubSpotModel>(GetRoute(), cancellationToken: cancellationToken);

        public async Task<SubscriptionTypeHubSpotModel> GetSubscriptionAsync(long id, CancellationToken cancellationToken = default) 
            => (await GetSubscriptionTypesAsync(cancellationToken)).Types.FirstOrDefault(x => x.Id == id);
        
        public Task<SubscriptionStatusHubSpotModel> GetSubscriptionStatusForContactAsync(string email, CancellationToken cancellationToken = default) 
            => _client.ExecuteAsync<SubscriptionStatusHubSpotModel>(GetRoute(email), cancellationToken: cancellationToken);

        public Task<SubscriptionTimelineHubSpotModel> GetChangesTimelineAsync(CancellationToken cancellationToken = default)
            => _client.ExecuteAsync<SubscriptionTimelineHubSpotModel>(GetRoute<SubscriptionTimelineHubSpotModel>(), cancellationToken: cancellationToken);     

        public Task UnsubscribeAllAsync(string email, CancellationToken cancellationToken = default) 
            => SendSubscriptionRequestAsync(GetRoute(email), new { unsubscribeFromAll = true }, cancellationToken);

        public Task UnsubscribeFromAsync(string email, long id, CancellationToken cancellationToken = default)
        {
            SubscriptionStatusHubSpotModel model = new SubscriptionStatusHubSpotModel();
            model.SubscriptionStatuses.Add(new SubscriptionStatusDetailHubSpotModel(id, false, OptState.OPT_OUT));            

           return SendSubscriptionRequestAsync(GetRoute(email), model);
        }

        public async Task SubscribeAllAsync(string email, CancellationToken cancellationToken = default)
        {
            List<SubscriptionStatusDetailHubSpotModel> subs = new List<SubscriptionStatusDetailHubSpotModel>();
            SubscriptionSubscribeHubSpotModel subRequest = new SubscriptionSubscribeHubSpotModel();

            (await GetSubscriptionTypesAsync(cancellationToken)).Types.ForEach(sub =>
            {
                subs.Add(new SubscriptionStatusDetailHubSpotModel(sub.Id, true, OptState.OPT_IN));
            });

            subRequest.SubscriptionStatuses.AddRange(subs);

            await SendSubscriptionRequestAsync(GetRoute(email), subRequest, cancellationToken);
        }

        public async Task SubscribeAllAsync(string email, GDPRLegalBasis legalBasis, string explanation, OptState optState = OptState.OPT_IN, CancellationToken cancellationToken = default)
        {
            List<SubscriptionStatusDetailHubSpotModel> subs = new List<SubscriptionStatusDetailHubSpotModel>();
            SubscriptionSubscribeHubSpotModel subRequest = new SubscriptionSubscribeHubSpotModel();

            (await GetSubscriptionTypesAsync(cancellationToken)).Types.ForEach(sub =>
            {
                subs.Add(new SubscriptionStatusDetailHubSpotModel(sub.Id, true, optState, legalBasis, explanation));
            });

            subRequest.SubscriptionStatuses.AddRange(subs);

            await SendSubscriptionRequestAsync(GetRoute(email), subRequest, cancellationToken);
        }

        public async Task SubscribeToAsync(string email, long id, CancellationToken cancellationToken = default)
        {
            SubscriptionTypeHubSpotModel singleSub = await GetSubscriptionAsync(id, cancellationToken);
            if (singleSub == null)
                throw new KeyNotFoundException("The SubscriptionType ID provided does not exist in the SubscriptionType list");

            SubscriptionSubscribeHubSpotModel subRequest = new SubscriptionSubscribeHubSpotModel();
            subRequest.SubscriptionStatuses.Add(new SubscriptionStatusDetailHubSpotModel(singleSub.Id, true, OptState.OPT_IN));
            
            await SendSubscriptionRequestAsync(GetRoute(email), subRequest, cancellationToken);
        }

        public async Task SubscribeToAsync(string email, long id, GDPRLegalBasis legalBasis, string explanation, OptState optState = OptState.OPT_IN, CancellationToken cancellationToken = default)
        {
            SubscriptionTypeHubSpotModel singleSub = await GetSubscriptionAsync(id, cancellationToken);
            SubscriptionSubscribeHubSpotModel subRequest = new SubscriptionSubscribeHubSpotModel();

            if (singleSub == null)
                throw new KeyNotFoundException("The SubscriptionType ID provided does not exist in the SubscriptionType list");

            subRequest.SubscriptionStatuses.Add(new SubscriptionStatusDetailHubSpotModel(singleSub.Id, true, optState, legalBasis, explanation));

            await SendSubscriptionRequestAsync(GetRoute(email), subRequest, cancellationToken);
        }        

        private Task SendSubscriptionRequestAsync(string path, object payload, CancellationToken cancellationToken = default)
            => _client.ExecuteOnlyAsync(path, payload, Method.PUT, cancellationToken);

        public async Task UnsubscribeFromAsync(string email, CancellationToken cancellationToken = default, params long[] ids)
        {
            foreach (var id in ids)
            {
                await UnsubscribeFromAsync(email, id, cancellationToken);
            }
        }

        public async Task SubscribeToAsync(string email, CancellationToken cancellationToken = default, params long[] ids)
        {
            foreach (var id in ids)
            {
                await SubscribeToAsync(email, id, cancellationToken);
            }
        }
    }
}
