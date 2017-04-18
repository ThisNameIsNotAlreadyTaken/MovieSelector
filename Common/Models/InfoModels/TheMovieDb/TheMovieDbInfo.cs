using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Common.Models.InfoModels.TheMovieDb
{
    public class TheMovieDbInfoModel : ISpecificInfoModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("title")]
        public string Name { get; set; }

        [JsonProperty("original_title")]
        public string OriginalName { get; set; }

        [JsonProperty("release_date")]
        public string ReleaseDate { get; set; }

        [JsonProperty("overview")]
        public string Description { get; set; }

        [JsonProperty("poster_path")]
        public string PosterUrl { get; set; }

        public byte[] Poster { get; set; }

        [JsonProperty("vote_average")]
        public string Rating { get; set; }

        public List<TheMovieDbParticipantInfo> Participants { get; set; }

        public CommonInfoModel ToCommonInfoModel()
        {
            return new CommonInfoModel
            {
                Id = Id,
                Name = Name,
                OriginalName = OriginalName,
                ReleaseDate = ReleaseDate,
                Description = Description,
                Poster = Poster,
                Participants = Participants?.Select(x => new ParticipantModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Character = x.Character,
                    Role = x.Role
                }).ToList() ?? new List<ParticipantModel>(),
                Ratings = new List<RatingModel>
                {
                    new RatingModel
                    {
                        Source = "TheMovieDb",
                        Value = Rating
                    }
                }
            };
        }
    }
}