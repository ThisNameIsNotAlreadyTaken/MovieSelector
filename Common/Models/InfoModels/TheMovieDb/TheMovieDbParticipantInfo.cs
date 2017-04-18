using Newtonsoft.Json;

namespace Common.Models.InfoModels.TheMovieDb
{
    public class TheMovieDbParticipantInfo
    {
        [JsonProperty("cast_id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("character")]
        public string Character { get; set; }

        [JsonProperty("job")]
        public string Job { get; set; }

        public ParticipantRole Role
        {
            get
            {
                switch (Job)
                {
                    case null:
                        return ParticipantRole.Actor;
                    case "Director":
                        return ParticipantRole.Director;
                    case "Screenplay":
                        return ParticipantRole.Writer;
                    case "Producer":
                        return ParticipantRole.Producer;
                }

                return ParticipantRole.Unknown;
            }
        }
    }
}