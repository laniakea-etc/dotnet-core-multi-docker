using Newtonsoft.Json;

namespace Server.Models
{
    public class Value
    {
        [JsonIgnore]
        public int Id { get; set; }

        public int Number { get; set; }
    }
}
