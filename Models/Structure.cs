using System;

namespace wilhe1m.StructureWatch.Models
{
    public class Structure
    {
        public enum States
        {
            UNKNOWN,
            ANCHORING,
            UNANCHORING,
            LOWPOWER,
            ABANDONDED,
            NOCORE,
            CORE,
            GONE,
            REINFORCED
        }

        public DateTime FirstSeen { get; set; } = DateTime.UtcNow;
        public string Name { get; set; } = "Unknown Structure";
        public States State { get; set; } = States.UNKNOWN;
        public DateTime StateChanged { get; set; } = DateTime.UtcNow;
        public int Id { get; set; }
        public long StructureId { get; set; }
        public string CorportationTag { get; set; }
        public string AllianceTag { get; set; }
    }
}