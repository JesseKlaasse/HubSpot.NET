using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HubSpot.NET.Core;

namespace HubSpot.NET.Examples
{
    public class EmailSubscriptions
    {
        public static async Task Example(HubSpotApi api, CancellationToken cancellationToken = default)
        {
            try
            {
                await Tests(api, cancellationToken);
                Console.WriteLine("Email Subscriptions tests passed.");
            }
            catch(Exception ex)
            {
                Console.WriteLine("Email Subscriptions tests failed!");
                Console.WriteLine(ex.ToString());
            }
        }

        private static async Task Tests(HubSpotApi api, CancellationToken cancellationToken = default)
        {
           /**
             * Get the available subscription types
             */
            var all = await api.EmailSubscriptions.GetSubscriptionTypesAsync(cancellationToken);

            /**
             * Get the subscription statuses for the given email address
             * A missing type implies that they have not opted out
             */
            //var john = api.EmailSubscriptions.GetSubscriptionStatusForContact("john@squaredup.com");

            /**
             * Unsubscribe a user from ALL emails
             * WARNING: You cannot undo this
             */
           // api.EmailSubscriptions.UnsubscribeAll("john@squaredup.com");


            /**
             * Unsubscribe a user from a given email type
             * WARNING: You cannot undo this
             */
            var type = all.Types.First();
           // api.EmailSubscriptions.UnsubscribeFrom("dan@squaredup.com", type.Id);

            await api.EmailSubscriptions.SubscribeToAsync("dev@vtrpro.com", type.Id, cancellationToken);
        }
    }
}
