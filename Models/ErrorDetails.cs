using System.Text.Json;
using Newtonsoft.Json;

namespace NewWebApi.Models
{
	public class ErrorDetails
	{
		public int StatusCode { get; set; }
		public string? Message { get; set; }
        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}