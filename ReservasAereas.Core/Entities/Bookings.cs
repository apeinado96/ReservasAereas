using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ReservasAereas.Core.Entities
{
    public class Bookings
    {
        [JsonIgnore]
        public int id { get; set; }
        public int airportOrigintId { get; set; }
        public int airportDestinationtId { get; set; }
        public DateTime entryTime { get; set; }
        public DateTime departureTime { get; set; }
        public int airlineId { get; set; }
        public string flightNumber { get; set; }
        public double priceTypePassenger { get; set; }
        [JsonIgnore]
        public DateTime? createdAt { get; set; }
        [JsonIgnore]
        public DateTime? updatedAt { get; set; }
        [JsonIgnore]
        public string airportOrigin { get; set; }
        [JsonIgnore]
        public string airportDestination { get; set; }
        [JsonIgnore]
        public string airline { get; set; }
    }
}
