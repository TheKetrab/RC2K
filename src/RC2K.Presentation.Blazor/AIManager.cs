using Azure;
using Azure.AI.Vision.ImageAnalysis;
using System.Text.RegularExpressions;

namespace RC2K.Presentation.Blazor;

public class AIManager(ILogger<AIManager> logger, string endpoint, string apiKey) : IAIManager
{
    public async Task<(int, int, int, string)?> GetTimeAndDriverFromStageResultImageView(string imageUrl)
    {
        try
        {
            var client = new ImageAnalysisClient(new Uri(endpoint), new AzureKeyCredential(apiKey));

            logger.LogInformation($"Sending image URL for OCR analysis: {imageUrl}");
            ImageAnalysisResult result = await client.AnalyzeAsync(new Uri(imageUrl), VisualFeatures.Read);
            logger.LogInformation($"OCR analysis successful.");

            var lines = result.Read.Blocks.SelectMany(x => x.Lines).ToList();
            string timePattern = @"(\d{2}).+(\d{2}).+(\d{2})";
            for (int i=0; i<lines.Count; i++)
            {
                var match = Regex.Match(lines[i].Text, timePattern);
                if (match.Success)
                {
                    string author = i>0 ? lines[i-1].Text : "?";
                    logger.LogInformation("Found best time on the screen: {time} by {author}", lines[i].Text, author);

                    int min = int.Parse(match.Groups[1].Value);
                    int sec = int.Parse(match.Groups[2].Value);
                    int cc = int.Parse(match.Groups[3].Value);
                    return (min, sec, cc, author);
                }
            }
        }
        catch (RequestFailedException ex)
        {
            logger.LogError($"Azure API Error: {ex.Message}");
        }
        catch (Exception ex)
        {
            logger.LogError($"An unexpected error occurred: {ex.Message}");
        }
        return null;
    }
}
