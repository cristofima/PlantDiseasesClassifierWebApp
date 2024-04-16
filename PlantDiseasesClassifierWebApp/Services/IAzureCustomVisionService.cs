using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction.Models;
using Microsoft.Extensions.Options;
using PlantDiseasesClassifierWebApp.Options;

namespace PlantDiseasesClassifierWebApp.Services
{
    public interface IAzureCustomVisionService
    {
        IList<PredictionModel> ClassifyImage(Stream image);
    }

    public class AzureCustomVisionService : IAzureCustomVisionService
    {
        private readonly CustomVisionPredictionClient predictionApi;
        private readonly AzureVisionSettings settings;

        public AzureCustomVisionService(IOptions<AzureVisionSettings> options)
        {
            this.settings = options.Value;
            this.predictionApi = new CustomVisionPredictionClient(new ApiKeyServiceClientCredentials(this.settings.PredictionKey))
            {
                Endpoint = this.settings.Endpoint
            };
        }

        public IList<PredictionModel> ClassifyImage(Stream image)
        {
            var result = this.predictionApi.ClassifyImage(Guid.Parse(this.settings.ProjectId), this.settings.PublishedModelName, image);
            return result.Predictions;
        }
    }
}
