namespace Common.Models
{
    public enum ParticipantRole
    {
        Actor,
        Director,
        Writer,
        Producer,
        Unknown
    }

    public class ParticipantModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Character { get; set; }

        public ParticipantRole Role { get; set; }
    }
}