using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Services
{
    public static class Gravatar
    {
        public static string GetHash(string emailAddress)
        {
            using (var md5 = MD5.Create())
            {
                var inputBytes = Encoding.Default.GetBytes(emailAddress.Trim().ToLowerInvariant());
                var hashBytes = md5.ComputeHash(inputBytes);

                var builder = new StringBuilder();
                foreach (var b in hashBytes)
                {
                    builder.Append(b.ToString("X2"));
                }
                return builder.ToString().ToLowerInvariant();
            }
        }
    }

    public class GavatarProfile
    {
        private const string defaultGravatarImageUrl = "https://www.gravatar.com/avatar";

        public static GavatarProfile Default(string name) => new(name, defaultGravatarImageUrl);

        public static GavatarProfile Empty() => new(string.Empty, defaultGravatarImageUrl);

        public GavatarProfile(string displayName, string thumbnailUrl)
        {
            DisplayName = displayName;
            ThumbnailUrl = thumbnailUrl;

            if(string.IsNullOrEmpty(thumbnailUrl))
                ThumbnailUrl = defaultGravatarImageUrl;
        }

        public string DisplayName { get; }
        public string ThumbnailUrl { get; }
    }

    public sealed class GravatarApiClient
    {
        private readonly HttpClient httpClient;

        public GravatarApiClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<GavatarProfile> GetProfile(string email, string defaultName)
        {
            try
            {
                var emailHash = Gravatar.GetHash(email);
                var response = await httpClient.GetFromJsonAsync<GetProfileResponseBody>($"{emailHash}.json");
                var entry = response.entry.FirstOrDefault();

                if (entry == null) return GavatarProfile.Default(defaultName);

                return new GavatarProfile(entry.displayName, entry.thumbnailUrl);
            }
            catch (Exception ex)
            {
                if (ex is HttpRequestException || ex is TaskCanceledException)
                    return GavatarProfile.Default(defaultName);

                throw;
            }

        }


        private class GetProfileResponseBody
        {
            public GetProfileEntry[] entry { get; set; }
        }


        private class GetProfileEntry
        {
            public string displayName { get; set; }

            public string thumbnailUrl { get; set; }
        }
    }
}