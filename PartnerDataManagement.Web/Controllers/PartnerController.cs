using Microsoft.AspNetCore.Mvc;
using Microsoft.ML.Data;
using Microsoft.ML;
using Neo4j.Driver;
using System.Threading.Tasks;
using System.Linq;

namespace PartnerDataManagement.Web.Controllers
{

    // Controllers for Partner Validation System
    [ApiController]
    [Route("api/[controller]")]
    public class PartnerController(AzureOpenAIService azureOpenAIService, BusinessValidator validator, AddressValidator addressValidator, Neo4jService neo4JService) : Controller
    {
        private readonly AzureOpenAIService _openAIService = azureOpenAIService;
        private readonly BusinessValidator _businessValidator = validator;
        private readonly AddressValidator _addressValidator = addressValidator;
        private readonly Neo4jService _neo4jService = neo4JService;

        public IActionResult Validate(string name, string url)
        {
            var scrapedNames = WebScraper.ScrapeBusinessNames(url);
            var matchScore = _businessValidator.MatchRatioForNames(name, scrapedNames.FirstOrDefault() ?? "");
            var weightedScore = _businessValidator.WeightedRatioForNames(name, scrapedNames.FirstOrDefault() ?? "");
            var namePrediction = _businessValidator.PredictNameUsingMicrosoftML(name);
            if (matchScore < 90 || weightedScore < 80)
            {
                return Json(new { Error = "Business name does not match the scraped data." });
            }

            return Json(new { BusinessName = name, PredictionLabel = namePrediction, MatchScore = matchScore });
        }

        public async Task<IActionResult> AnalyzeBusiness(string businessName)
        {
            var prompt = $"Verify if '{businessName}' is a legitimate business. Provide detailed analysis.";
            var response = await _openAIService.GetResponse(prompt);
            return Json(new { BusinessName = businessName, AIAnalysis = response });
        }

        public async Task<IActionResult> GetSubsidiaries(string companyName)
        {
            var subsidiaries = await _neo4jService.GetSubsidiaries(companyName);
            return Json(subsidiaries);
        }

        public async Task<IActionResult> ValidateAddress(string address)
        {
            var geocodingResponse = await _addressValidator.ValidateAddress(address);
            if (geocodingResponse == null || !geocodingResponse.Results.Any())
            {
                return Json(new { Error = "Address validation failed." });
            }
            var formattedAddress = geocodingResponse.Results.FirstOrDefault()?.FormattedAddress;
            return Json(new { Original = address, Validated = formattedAddress });
        }
    }
}
