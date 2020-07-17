using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Flurl;
using HubSpot.NET.Api.EmailEvents.Dto;
using HubSpot.NET.Core;
using HubSpot.NET.Core.Interfaces;
using RestSharp;

namespace HubSpot.NET.Api.EmailEvents
{
    public class HubSpotEmailEventsApi : IHubSpotEmailEventsApi
    {
        private readonly IHubSpotClient _client;

        public HubSpotEmailEventsApi(IHubSpotClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Gets campaign data by ID from hubspot
        /// </summary>
        /// <param name="campaignId">The campaign ID to query.</param>
        /// <param name="appId">The app ID to query.</param>
        /// <typeparam name="T">Implementation of EmailCampaignDataHubSpotModel</typeparam>
        /// <returns>The xcampaign data entity</returns>
        public Task<T> GetCampaignDataByIdAsync<T>(long campaignId, long appId, CancellationToken cancellationToken = default) where T : EmailCampaignDataHubSpotModel, new()
        {
            var path = $"{(new T()).RouteBasePath}/{campaignId}"
                .SetQueryParam("appId", appId);
            var data = _client.ExecuteAsync<T>(path, Method.GET, cancellationToken);
            return data;
        }

        /// <summary>
        /// Gets a list of email campaigns.
        /// </summary>
        /// <typeparam name="T">Implementation of EmailCampaignHubSpotModel</typeparam>
        /// <param name="opts">Options (limit, offset) relating to request</param>
        /// <returns>List of email campaigns</returns>
        public Task<EmailCampaignListHubSpotModel<T>> ListCampaignsAsync<T>(EmailCampaignListRequestOptions opts = null, CancellationToken cancellationToken = default) where T : EmailCampaignHubSpotModel, new()
        {
            if (opts == null)
            {
                opts = new EmailCampaignListRequestOptions { Limit = 250 };
            }

            var path = $"{new EmailCampaignListHubSpotModel<T>().RouteBasePath}/by-id"
                .SetQueryParam("limit", opts.Limit);

            if (!string.IsNullOrEmpty(opts.Offset))
            {
                path = path.SetQueryParam("offset", opts.Offset);
            }

            var data = _client.ExecuteAsync<EmailCampaignListHubSpotModel<T>>(path, cancellationToken: cancellationToken);

            return data;
        }

        /// <summary>
        /// Gets a list of email campaigns.
        /// </summary>
        /// <typeparam name="T">Implementation of EmailCampaignHubSpotModel</typeparam>
        /// <param name="opts">Options (limit, offset) relating to request</param>
        /// <returns>List of email campaigns</returns>
        public Task<EmailCampaignListHubSpotModel<T>> RecentlyUpdatedCampaignsAsync<T>(EmailCampaignListRequestOptions opts = null, CancellationToken cancellationToken = default) where T : EmailCampaignHubSpotModel, new()
        {
            if (opts == null)
            {
                opts = new EmailCampaignListRequestOptions { Limit = 250 };
            }

            var path = $"{new EmailCampaignListHubSpotModel<T>().RouteBasePath}"
                .SetQueryParam("limit", opts.Limit);

            if (!string.IsNullOrEmpty(opts.Offset))
            {
                path = path.SetQueryParam("offset", opts.Offset);
            }

            var data = _client.ExecuteAsync<EmailCampaignListHubSpotModel<T>>(path, cancellationToken: cancellationToken);

            return data;
        }

    }
}
