using System.Collections.Generic;
using System.Linq;
using Common.Instrastructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Common.Models
{
    public class KinopoiskInfo
    {
        [JsonProperty("filmID")]
        public string Id { get; set; }

        [JsonProperty("webURL")]
        public string Url { get; set; }

        [JsonProperty("nameRU")]
        public string Name { get; set; }

        [JsonProperty("nameEN")]
        public string OriginalName { get; set; }

        [JsonProperty("posterURL")]
        public string PosterUrl { get; set; }

        [JsonProperty("year")]
        public string Year { get; set; }

        [JsonProperty("ratingData")]
        public RatingInfo Rating { get; set; }

        [JsonProperty("creators")]
        public List<List<Participant>> Participants { get; set; }

        public byte[] Poster { get; set; }

        public class RatingInfo
        {
            [JsonProperty("ratingIMDb")]
            public string ImDb { get; set; }

            [JsonProperty("rating")]
            public string Kinopoisk { get; set; }
        }

        public class Participant
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("nameRU")]
            public string Name { get; set; }

            [JsonProperty("nameEN")]
            public string OriginalName { get; set; }

            [JsonProperty("posterURL")]
            public string PosterUrl { get; set; }

            [JsonProperty("professionKey")]
            [JsonConverter(typeof(StringEnumConverter))]
            public Profession Profession { get; set; }
        }

        public string RelatedFileName { get; set; }

        [JsonIgnore]
        public string Directors
        {
            get
            {
                var directors = Participants?.FirstOrDefault(x => x.FirstOrDefault(y => y.Profession == Profession.Director) != null)?
                        .Select(x => x.Name);

                return directors != null ? string.Join(", ", directors) : null;
            }
        }

        [JsonIgnore]
        public string Actors
        {
            get
            {
                var actors = Participants?.FirstOrDefault(x => x.FirstOrDefault(y => y.Profession == Profession.Actor) != null)?
                        .Select(x => x.Name);

                return actors != null ?string.Join(", ", actors) : null;
            }
        }
    }
}
