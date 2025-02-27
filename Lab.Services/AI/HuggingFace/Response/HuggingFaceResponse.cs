using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Lab.Services.AI.HuggingFace.Response
{
    public class HuggingFaceResponse
    {
        [JsonPropertyName("generated_text")]
        public string GeneratedText { get; set; }
    }
}
