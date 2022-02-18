using Newtonsoft.Json;

namespace ReservasAereas.Utilities
{
    public class Responses
    {
        public static object ParseResponse(int StatusCode, string message, object data)
        {
            var response = new
            {
                ContentType = "application/json",
                StatusCode,
                message,
                data,
            };
            var json = JsonConvert.SerializeObject(response);
            var jsonResponse = new { response = json };
            return jsonResponse;
        }
    }
}
