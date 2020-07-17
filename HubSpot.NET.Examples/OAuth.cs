using System.Threading;
using System.Threading.Tasks;

namespace HubSpot.NET.Examples
{
    using HubSpot.NET.Core;

    public class OAuth
    {
        public static async Task Example(HubSpotApi hubspot, string redirectCode = "", string redirectUri = "", CancellationToken cancellationToken = default)
        {
            var token = await hubspot.OAuth.AuthorizeAsync(redirectCode, redirectUri, cancellationToken);
        }
    }
}