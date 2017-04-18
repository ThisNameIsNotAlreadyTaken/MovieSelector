using System.Collections.Generic;
using System.Linq;

namespace Common.Models
{
    public class CommonInfoModel
    {
        public string Id { get; set; }

        public string ImdbId { get; set; }

        public string Name { get; set; }

        public string OriginalName { get; set; }

        public string PosterUrl { get; set; }

        public byte[] Poster { get; set; }

        public string ReleaseDate { get; set; }

        public string Description { get; set; }

        public IEnumerable<RatingModel> Ratings { get; set; }

        public IEnumerable<ParticipantModel> Participants { get; set; }

        public string Directors
        {
            get
            {
                var directors = Participants?.Where(x => x.Role == ParticipantRole.Director).Select(x => x.Name);

                return directors != null ? string.Join(", ", directors) : null;
            }
        }

        public string Actors
        {
            get
            {
                var directors = Participants?.Where(x => x.Role == ParticipantRole.Actor).Select(x => x.Name);

                return directors != null ? string.Join(", ", directors) : null;
            }
        }

        public string Producers
        {
            get
            {
                var directors = Participants?.Where(x => x.Role == ParticipantRole.Producer).Select(x => x.Name);

                return directors != null ? string.Join(", ", directors) : null;
            }
        }

        public string Writers
        {
            get
            {
                var directors = Participants?.Where(x => x.Role == ParticipantRole.Writer).Select(x => x.Name);

                return directors != null ? string.Join(", ", directors) : null;
            }
        }

        public string RelatedFileName { get; set; }
    }
}