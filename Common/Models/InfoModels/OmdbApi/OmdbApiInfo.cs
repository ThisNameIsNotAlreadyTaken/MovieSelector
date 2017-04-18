using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Common.Models.InfoModels.OmdbApi
{
    public class OmdbApiInfo : ISpecificInfoModel
    {
        [JsonProperty("imdbID")]
        public string Id { get; set; }

        [JsonProperty("Title")]
        public string Name { get; set; }

        public string OriginalName => Name;

        [JsonProperty("Released")]
        public string ReleaseDate { get; set; }

        [JsonProperty("Plot")]
        public string Description { get; set; }

        [JsonProperty("Poster")]
        public string PosterUrl { get; set; }

        public byte[] PosterImage { get; set; }

        [JsonProperty("Genre")]
        public string Genre { get; set; }

        [JsonProperty("Director")]
        public string Directors { get; set; }

        [JsonProperty("Actors")]
        public string Actors { get; set; }

        [JsonProperty("Writer")]
        public string Writers { get; set; }

        [JsonProperty("Ratings")]
        public List<RatingModel> Ratings { get; set; }

        [JsonProperty("Metascore")]
        public string MetascoreRating { get; set; }

        [JsonProperty("imdbRating")]
        public string ImdbRating { get; set; }

        public List<ParticipantModel> Participants
        {
            get
            {
                var directors = Directors.Split(',').Select(x => new ParticipantModel
                {
                    Name = x,
                    Role = ParticipantRole.Director
                });

                var actors = Actors.Split(',').Select(x => new ParticipantModel
                {
                    Name = x,
                    Role = ParticipantRole.Actor
                });

                var writers = Writers.Split(',').Select(x => new ParticipantModel
                {
                    Name = x,
                    Role = ParticipantRole.Writer
                });

                return directors.Union(actors).Union(writers).ToList();
            }
        }

        public List<RatingModel> TopRatings
        {
            get
            {
                RatingModel sideRating = null;

                if (Ratings.Any())
                {
                    sideRating = Ratings.FirstOrDefault(x => x.Source == "Rotten Tomatoes") ?? Ratings.First();
                }

                return new List<RatingModel>
                {
                    new RatingModel
                    {
                        Source = "Imdb",
                        Value = ImdbRating
                    },
                    sideRating
                };
            }
        } 

        public CommonInfoModel ToCommonInfoModel()
        {
            return new CommonInfoModel
            {
                Id = Id,
                ImdbId = Id,
                Name = Name,
                OriginalName = OriginalName,
                ReleaseDate = ReleaseDate,
                Description = Description,
                Poster = PosterImage,
                Participants = Participants,
                Ratings = TopRatings
            };
        }
    }
}