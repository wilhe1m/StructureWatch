using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace wilhe1m.StructureWatch.Models
{
    public class Notification
    {
        public static readonly int NO_INT = 0;
        public static readonly string NO_STRING = "";
        public static readonly DateTime NO_DATE = new DateTime(2020, 01, 01);

        private Dictionary<string, string> _parsed;


        [JsonProperty("notification_id")]
        public long NotificationId { get; set; } //= NO_INT;
        public long Id { get; set; } //= NO_INT;
        [JsonProperty("sender_id")]
        public long SenderId { get; set; } //= NO_INT;
        [JsonProperty("sender_type")]
        public string SenderType { get; set; } //= NO_STRING;
        public string Text { get; set; } //= NO_STRING;

        public DateTime Timestamp { get; set; }// = NO_DATE;
        public string Type { get; set; } //= NO_STRING;

        public bool Hidden { get; set; }

        public Dictionary<string, string> ParsedData
        {
            get
            {
                if (_parsed == null)
                {
                    _parsed = new Dictionary<string, string>();
                    var items = Text.Split("\n");
                    for (var i = 0; i < items.Length; i++)
                    {
                        var idx = items[i].IndexOf(": ");
                        if (idx > 0) _parsed.Add(items[i].Substring(0, idx), items[i].Substring(idx + 2));
                    }
                }

                return _parsed;
            }
        }
    }
}