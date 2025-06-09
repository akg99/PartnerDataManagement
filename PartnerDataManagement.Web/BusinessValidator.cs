using FuzzySharp;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms;

namespace PartnerDataManagement.Web
{
    public class BusinessValidator(MLNameValidator mLNameValidator)
    {
        private readonly MLNameValidator _mLNameValidator = mLNameValidator;
        private readonly MLContext _mlContext = mLNameValidator.MLContext;
        private readonly PredictionEngine<TextData, NamePrediction> _predictionEngine = mLNameValidator.SetupPredictionEngine();

        public float CompareNamesUsingMicrosoftML(string name1, string name2)
        {
            var fuzzyMatcher = _mlContext.Transforms.Text.TokenizeIntoWords("Name")
                               .Append(_mlContext.Transforms.Text.NormalizeText("Name"))
                               .Append(_mlContext.Transforms.Conversion.MapValueToKey("Name"));

            var fuzzyScore = fuzzyMatcher.Fit(_mlContext.Data.LoadFromEnumerable(new[] { new { Name = name1 } }))
                                         .Transform(_mlContext.Data.LoadFromEnumerable(new[] { new { Name = name2 } }));

            return fuzzyScore.GetColumn<float>("Name").FirstOrDefault();
        }

        public bool PredictNameUsingMicrosoftML(string inputName)
        {
            var prediction = _predictionEngine.Predict(new TextData { Text = inputName });
            Console.WriteLine($"Predicted Name Match: {prediction.Label}");
            return prediction.Label;
        }


        public int MatchRatioForNames(string name1, string name2)
        {
            return Fuzz.Ratio(name1, name2);
        }

        public int WeightedRatioForNames(string name1, string name2)
        {
            return Fuzz.WeightedRatio(name1, name2);
        }
    }
}
