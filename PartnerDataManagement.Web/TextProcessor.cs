using Microsoft.ML;
using Microsoft.ML.Data;

namespace PartnerDataManagement.Web
{

    public class TextProcessor
    {
        private static readonly MLContext _mlContext = new MLContext();

        public static IDataView ProcessText(string inputText)
        {
            var data = new[] { new TextData { Text = inputText } };
            var dataView = _mlContext.Data.LoadFromEnumerable(data);

            var pipeline = _mlContext.Transforms.Text.FeaturizeText("Features", "Text");
            var transformedData = pipeline.Fit(dataView).Transform(dataView);

            return transformedData;
        }
    }

    public class TextData
    {
        public string Text { get; set; }
    }
}
