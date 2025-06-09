using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms;

namespace PartnerDataManagement.Web
{
    public class MLNameValidator(MLContext mLContext)
    {
        public readonly MLContext MLContext = mLContext;

        public PredictionEngine<TextData, NamePrediction> SetupPredictionEngine()
        {

            var data = new[] {
                new BusinessNameData { Name = "Microsoft Corporation", IsValidMatch = true },
                new BusinessNameData { Name = "Microsft", IsValidMatch = false }, // Intentional typo
            };

            var dataView = MLContext.Data.LoadFromEnumerable(data);
            var pipeline = MLContext.Transforms.Text.FeaturizeText("Features", "Name")
                            .Append(MLContext.Transforms.Conversion.MapValueToKey("IsValidMatch"));

            var model = pipeline.Fit(dataView);
            var predictionEngine = MLContext.Model.CreatePredictionEngine<TextData, NamePrediction>(model);
            return predictionEngine;
        }
    }
    public class NamePrediction { [ColumnName("Label")] public bool Label { get; set; } }

    public class BusinessNameData
    {
        [LoadColumn(0)] public string Name { get; set; }
        [LoadColumn(1)] public bool IsValidMatch { get; set; }
    }
}
