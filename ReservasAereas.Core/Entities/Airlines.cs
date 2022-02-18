using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ReservasAereas.Core.Entities
{
    public class Airlines
    {
        [JsonIgnore]
        public int id { get; set; }
        public string name { get; set; }
        [JsonIgnore]
        public DateTime? createdAt { get; set; }
        [JsonIgnore]
        public DateTime? updatedAt { get; set; }        
    }
}
