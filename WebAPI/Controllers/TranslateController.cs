using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using WebAPI.Model;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TranslateController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public TranslateController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpPost]
        public async Task<ActionResult<TranslationResponse>> TranslateText([FromForm]TranslationRequest request)
        {
            string url = $"https://translate.google.com/translate_a/single?client=gtx&sl={request.InputLanguage}&tl={request.OutputLanguage}&dt=t&q={request.OriginalText}";

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var responseText = await response.Content.ReadAsStringAsync();
                JArray jsonResponse = JArray.Parse(responseText);

                JArray translationParts = (JArray)jsonResponse[0];
                List<string> translatedParts = new List<string>();

                foreach (JArray part in translationParts)
                {
                    string translatedText = (string)part[0];
                    translatedParts.Add(translatedText);
                }

                var translationResponse = new TranslationResponse { Translations = translatedParts };
                return translationResponse;
            }
            else
            {
                return StatusCode((int)response.StatusCode, "Translation request failed");
            }
        }
    }
}
