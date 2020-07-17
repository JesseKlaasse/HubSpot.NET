using HubSpot.NET.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HubSpot.NET.Examples
{
    public class Timeline
    {
        public static async Task Example(HubSpotApi api, CancellationToken cancellationToken = default)
        {
            try
            {
                await Tests(api, cancellationToken);
                Console.Write("Timeline tests passed!");
            }
            catch(Exception ex)
            {
                Console.WriteLine("Timeline tests failed!", ex.ToString());
            }
        }

        private static async Task Tests(HubSpotApi api, CancellationToken cancellationToken)
        {
            var eventTypes = await api.Timelines.GetAllEventTypesAsync(cancellationToken);

        }
    }
}
