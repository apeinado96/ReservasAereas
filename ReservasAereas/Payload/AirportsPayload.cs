using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ReservasAereas.Core.Entities
{
    public class AirportsPayload
    {
        public int id { get; set; }
        public string name { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime? updatedAt { get; set; }        
    }
}
