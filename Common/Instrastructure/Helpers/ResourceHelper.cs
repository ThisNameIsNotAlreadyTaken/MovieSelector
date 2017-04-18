using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Common.Instrastructure.Helpers
{
    public class TheMovieDbConfiguration
    {
        public string SearchUrlTemplate { get; set; }

        public string ImageUrlTemplate { get; set; }

        public string CreditsUrlTemplate { get; set; }

        public string ApiKey { get; set; }
    }

    public class OmdbApiConfiguration
    {
        public string SearchUrlTemplate { get; set; }
    }

    public class RequestConfiguration
    {
        public int AttemptsCount { get; set; }

        public int ReconnectTime { get; set; }
    }

    public class Resource
    {
        public List<string> ExtensionArray { get; set; }

        public List<string> Directories { get; set; }

        public string LocalFile { get; set; }

        public TheMovieDbConfiguration TheMovieDbConfiguration { get; set; }

        public OmdbApiConfiguration OmdbApiConfiguration { get; set; }

        public RequestConfiguration RequestConfiguration { get; set; }
    }

    public static class ResourceHelper
    {
        private static readonly string ResourceFilePath = Path.Combine(Environment.CurrentDirectory, "resources.json");

        public static Resource Resources { get; } = new Resource();

        static ResourceHelper()
        {
            if (!File.Exists(ResourceFilePath)) return;

            using (var file = File.OpenText(ResourceFilePath))
            {
                using (var jReader = new JsonTextReader(file))
                {
                    var serializer = new JsonSerializer();
                    var resources = serializer.Deserialize<Resource>(jReader);

                    Resources = resources;
                }
            }
        }

        public static void SavePreferences()
        {
            using (var file = File.CreateText(ResourceFilePath))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(file, Resources);
            }
        }
    }
}
