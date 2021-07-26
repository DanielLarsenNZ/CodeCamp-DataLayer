using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public abstract class Model
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("_eTag")]
        public string ETag { get; set; }
    }
}
