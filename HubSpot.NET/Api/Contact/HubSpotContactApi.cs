﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Flurl;
using HubSpot.NET.Api.Contact.Dto;
using HubSpot.NET.Core;
using HubSpot.NET.Core.Abstracts;
using HubSpot.NET.Core.Interfaces;
using RestSharp;

namespace HubSpot.NET.Api.Contact
{
    public class HubSpotContactApi : ApiRoutable, IHubSpotContactApi
    {
        private readonly IHubSpotClient _client;
        public override string MidRoute => "/contacts/v1";

        public HubSpotContactApi(IHubSpotClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Creates a contact entity
        /// </summary>
        /// <typeparam name="T">Implementation of ContactHubSpotModel</typeparam>
        /// <param name="entity">The entity</param>
        /// <returns>The created entity (with ID set)</returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<ContactHubSpotModel> CreateAsync(ContactHubSpotModel entity, CancellationToken cancellationToken = default)
        {
            CreateOrUpdateContactTransportModel transport = new CreateOrUpdateContactTransportModel(entity);
            string path = GetRoute<ContactHubSpotModel>("contact");

            return _client.ExecuteAsync<ContactHubSpotModel, CreateOrUpdateContactTransportModel>(path, transport, Method.POST, cancellationToken);
        }

        /// <summary>
        /// Creates or Updates a contact entity based on the Entity Email
        /// </summary>
        /// <typeparam name="T">Implementation of ContactHubSpotModel</typeparam>
        /// <param name="entity">The entity</param>
        /// <returns>The created entity (with ID set)</returns>
        public Task<ContactHubSpotModel> CreateOrUpdateAsync(ContactHubSpotModel entity, CancellationToken cancellationToken = default)
        {
            CreateOrUpdateContactTransportModel transport = new CreateOrUpdateContactTransportModel(entity);
            string path = GetRoute<ContactHubSpotModel>("contact", "createOrUpdate", "email", entity.Email);

            return _client.ExecuteAsync<ContactHubSpotModel, CreateOrUpdateContactTransportModel>(path, transport, Method.POST, cancellationToken);
        }

        /// <summary>
        /// Creates or updates a contact entity based on the entity's current email.
        /// </summary>
        /// <param name="originalEmail">The email the server knows, assuming the entity email may be different.</param>
        /// <param name="entity">The contact entity to update on the server.</param>
        /// <returns>The updated entity (with ID set)</returns>
        public Task<ContactHubSpotModel> CreateOrUpdateAsync(string originalEmail, ContactHubSpotModel entity, CancellationToken cancellationToken = default)
        {
            CreateOrUpdateContactTransportModel transport = new CreateOrUpdateContactTransportModel(entity);
            string path = GetRoute<ContactHubSpotModel>("contact", "createOrUpdate", "email", originalEmail);

            return _client.ExecuteAsync<ContactHubSpotModel, CreateOrUpdateContactTransportModel>(path, transport, Method.POST, cancellationToken);
        }

        /// <summary>
        /// Gets a single contact by ID from hubspot
        /// </summary>
        /// <param name="contactId">ID of the contact</param>
        /// <typeparam name="T">Implementation of ContactHubSpotModel</typeparam>
        /// <returns>The contact entity</returns>
        public Task<ContactHubSpotModel> GetByIdAsync(long contactId, bool includeHistory = true, CancellationToken cancellationToken = default)
        {
            if(includeHistory)
            {
                return _client.ExecuteAsync<ContactHubSpotModel>(GetRoute<ContactHubSpotModel>("contact","vid", contactId.ToString(),"profile"), cancellationToken: cancellationToken);
            }
            else
            {
                return _client.ExecuteAsync<ContactHubSpotModel>(GetRoute<ContactHubSpotModel>("contact", "vid", contactId.ToString(), "profile?propertyMode=value_only"), cancellationToken: cancellationToken);
            }
        }

        public Task<ContactHubSpotModel> GetByIdAsync(long Id, CancellationToken cancellationToken = default) => GetByIdAsync(Id, true, cancellationToken);

        /// <summary>
        /// Gets a contact by their email address
        /// </summary>
        /// <param name="email">Email address to search for</param>
        /// <typeparam name="T">Implementation of ContactHubSpotModel</typeparam>
        /// <returns>The contact entity</returns>
        public Task<ContactHubSpotModel> GetByEmailAsync(string email, bool includeHistory = true, CancellationToken cancellationToken = default)
        {
            if (includeHistory)
            {
                return _client.ExecuteAsync<ContactHubSpotModel>(GetRoute<ContactHubSpotModel>("contact", "email", email, "profile"), cancellationToken: cancellationToken);
            }
            else
            {
                return _client.ExecuteAsync<ContactHubSpotModel>(GetRoute<ContactHubSpotModel>("contact", "email", email, "profile?propertyMode=value_only"), cancellationToken: cancellationToken);
            }
        }

        /// <summary>
        /// Gets a contact by their user token
        /// </summary>
        /// <param name="userToken">User token to search for from hubspotutk cookie</param>
        /// <typeparam name="T">Implementation of ContactHubSpotModel</typeparam>
        /// <returns>The contact entity</returns>
        public Task<ContactHubSpotModel> GetByUserTokenAsync(string userToken, bool includeHistory = true, CancellationToken cancellationToken = default)
        {
            if(includeHistory)
            {
                return _client.ExecuteAsync<ContactHubSpotModel>(GetRoute<ContactHubSpotModel>("contact", "utk", userToken, "profile"), cancellationToken: cancellationToken);
            }
            else
            {
                return _client.ExecuteAsync<ContactHubSpotModel>(GetRoute<ContactHubSpotModel>("contact", "utk", userToken, "profile?propertyMode=value_only"), cancellationToken: cancellationToken);
            }
        }

        /// <summary>
        /// List all available contacts 
        /// </summary>
        /// <param name="properties">List of properties to fetch for each contact</param>
        /// <param name="opts">Request options - used for pagination etc.</param>
        /// <typeparam name="T">Implementation of ContactHubSpotModel</typeparam>
        /// <returns>A list of contacts</returns>
        public Task<ContactListHubSpotModel<ContactHubSpotModel>> ListAsync(ListRequestOptions opts = null, CancellationToken cancellationToken = default)
        {
            opts = opts ?? new ListRequestOptions();            

            var path = GetRoute<ContactHubSpotModel>("lists", "all", "contacts","all")
                .SetQueryParam("count", opts.Limit);

            if (opts.PropertiesToInclude.Any())            
                path.SetQueryParam("property", opts.PropertiesToInclude);            

            if (opts.Offset.HasValue)            
                path = path.SetQueryParam("vidOffset", opts.Offset);            

            return _client.ExecuteAsync<ContactListHubSpotModel<ContactHubSpotModel>, ListRequestOptions>(path, opts, cancellationToken: cancellationToken);           
        }

        /// <summary>
        /// Updates a given contact
        /// </summary>
        /// <typeparam name="T">Implementation of ContactHubSpotModel</typeparam>
        /// <param name="contact">The contact entity</param>
        public Task<ContactHubSpotModel> UpdateAsync(ContactHubSpotModel contact, CancellationToken cancellationToken = default)
        {
            if (contact.Id < 1)            
                throw new ArgumentException("Contact entity must have an id set!");                       

            return _client.ExecuteAsync<ContactHubSpotModel, ContactHubSpotModel>(GetRoute<ContactHubSpotModel>("contact","vid", contact.Id.ToString(), "profile"), contact, Method.POST, cancellationToken);
        }

        /// <summary>
        /// Deletes a given contact
        /// </summary>
        /// <param name="contactId">The ID of the contact</param>
        public Task DeleteAsync(long contactId, CancellationToken cancellationToken = default) 
            => _client.ExecuteOnlyAsync(GetRoute<ContactHubSpotModel>("contact", "vid", contactId.ToString()), method: Method.DELETE, cancellationToken: cancellationToken);

        /// <summary>
        /// Update or create a set of contacts, this is the preferred method when creating/updating in bulk.
        /// Best performance is with a maximum of 250 contacts.
        /// </summary>
        /// <typeparam name="T">Implementation of ContactHubSpotModel</typeparam>
        /// <param name="contacts">The set of contacts to update/create</param>
        public Task BatchAsync(IEnumerable<ContactHubSpotModel> contacts, CancellationToken cancellationToken = default)
        {
            return _client.ExecuteBatchAsync(GetRoute<ContactHubSpotModel>("contact", "batch"),
                contacts.Select(c => (object) new CreateOrUpdateContactBatchTransportModel(c)), Method.POST, cancellationToken);
        }

        /// <summary>
        /// Get recently updated (or created) contacts
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="opts">Request options</param>
        /// <returns></returns>
        public Task<ContactListHubSpotModel<ContactHubSpotModel>> RecentlyUpdatedAsync(ListRecentRequestOptions opts = null, CancellationToken cancellationToken = default)
        {
            opts = opts ?? new ListRecentRequestOptions();            

            Url path = GetRoute<ContactHubSpotModel>("lists", "recently_updated","contacts","recent")
                .SetQueryParam("count", opts.Limit);

            if (opts.PropertiesToInclude.Any())            
                path.SetQueryParam("property", opts.PropertiesToInclude);            

            if (opts.Offset.HasValue)            
                path = path.SetQueryParam("vidOffset", opts.Offset);            

            if (!string.IsNullOrEmpty(opts.TimeOffset))            
                path = path.SetQueryParam("timeOffset", opts.TimeOffset);            
            
            path = path.SetQueryParam("propertyMode", opts.PropertyMode)
                        .SetQueryParam("formSubmissionMode", opts.FormSubmissionMode)
                        .SetQueryParam("showListMemberships", opts.ShowListMemberships);
            
            return _client.ExecuteAsync<ContactListHubSpotModel<ContactHubSpotModel>, ListRecentRequestOptions>(path, opts, cancellationToken: cancellationToken);            
        }

        public Task<ContactSearchHubSpotModel<ContactHubSpotModel>> SearchAsync(ContactSearchRequestOptions opts = null, CancellationToken cancellationToken = default)
        {
            opts = opts ?? new ContactSearchRequestOptions();

            Url path = GetRoute<ContactHubSpotModel>("search","query")
                .SetQueryParam("q", opts.Query)
                .SetQueryParam("count", opts.Limit);

            if (opts.PropertiesToInclude.Any())            
                path.SetQueryParam("property", opts.PropertiesToInclude);            

            if (opts.Offset.HasValue)            
                path = path.SetQueryParam("offset", opts.Offset);            

            return _client.ExecuteAsync<ContactSearchHubSpotModel<ContactHubSpotModel>, ContactSearchRequestOptions>(path, opts, cancellationToken: cancellationToken);            
        }

        /// <summary>
        /// Get a list of recently created contacts
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="opts">Request options</param>
        /// <returns></returns>
        public Task<ContactListHubSpotModel<ContactHubSpotModel>> RecentlyCreatedAsync(ListRecentRequestOptions opts = null, CancellationToken cancellationToken = default)
        {            
            opts = opts ?? new ListRecentRequestOptions();

            Url path = GetRoute<ContactHubSpotModel>("lists","all","contacts","recent")
                .SetQueryParam("count", opts.Limit);

            if (opts.PropertiesToInclude.Any())            
                path.SetQueryParam("property", opts.PropertiesToInclude);

            if (opts.Offset.HasValue)            
                path = path.SetQueryParam("vidOffset", opts.Offset);

            if (!string.IsNullOrEmpty(opts.TimeOffset))            
                path = path.SetQueryParam("timeOffset", opts.TimeOffset);
            
            path = path.SetQueryParam("propertyMode", opts.PropertyMode)
                        .SetQueryParam("formSubmissionMode", opts.FormSubmissionMode)
                        .SetQueryParam("showListMemberships", opts.ShowListMemberships);   
            
            return _client.ExecuteAsync<ContactListHubSpotModel<ContactHubSpotModel>, ListRecentRequestOptions>(path, opts, cancellationToken: cancellationToken);
        }
    }
}
