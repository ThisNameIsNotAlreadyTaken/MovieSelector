using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Common.Instrastructure
{
    public class Resource
    {
        public string QimdbUrl { get; set; }

        public string KinopoiskApiUrl { get; set; }

        public string YandexImagesUrl { get; set; }

        public List<string> ExtensionArray { get; set; }

        public List<string> Directories { get; set; }

        public string LocalFile { get; set; }
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
