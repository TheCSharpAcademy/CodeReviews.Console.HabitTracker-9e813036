using Microsoft.CognitiveServices.Speech;
using Microsoft.Extensions.Configuration;

namespace HabitTracker.Infrastructure
{
    public class SpeechService : IDisposable
    {
        private readonly IConfiguration _configuration;
        private readonly SpeechRecognizer? _recognizer;

        public SpeechService(IConfiguration configuration)
        {
            _configuration = configuration;
            _recognizer = InitializeRecognizer();
        }

        public bool IsEnabled => _recognizer != null;

        private SpeechRecognizer? InitializeRecognizer()
        {
            string? speechKey = _configuration["AzureSpeachService:Key"];
            string? speechLocation = _configuration["AzureSpeachService:Location"];

            if (!string.IsNullOrEmpty(speechKey) &&
                !string.IsNullOrEmpty(speechLocation) &&
                speechKey != "YourKey")
            {
                var config = SpeechConfig.FromSubscription(speechKey, speechLocation);
                return new SpeechRecognizer(config);
            }

            return null;
        }

        public async Task<string?> RecognizeAsync()
        {
            if (_recognizer == null) return null;

            var result = await _recognizer.RecognizeOnceAsync();

            if (result.Reason == ResultReason.RecognizedSpeech)
            {
                return result.Text.TrimEnd('.', '?', '!');
            }

            return null;
        }

        public void Dispose()
        {
            _recognizer?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}