using System.Collections.Concurrent;
using System.Text.Json;

namespace tfl_stats.Server.Services
{
    public class ResponseRecorderService
    {
        private readonly ILogger<ResponseRecorderService> _logger;

        public bool IsRecordingEnabled { get; private set; }
        public bool IsPlayingEnabled { get; private set; }

        private ConcurrentDictionary<string, string> _records = new();

        public ResponseRecorderService(ILogger<ResponseRecorderService> logger)
        {
            _logger = logger;
        }

        public void EnableRecording() => IsRecordingEnabled = true;
        public void DisableRecording() => IsRecordingEnabled = false;

        public void EnablePlayback() => IsPlayingEnabled = true;
        public void DisablePlayback() => IsPlayingEnabled = false;

        public void Record(string key, string response)
        {
            _records[key] = response;
            _logger.LogInformation("Recorded response for {key}", key);
        }
        public async Task SaveAsync()
        {

            var json = JsonSerializer.Serialize(this._records,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                });
            await File.WriteAllTextAsync("recorder.json", json);
        }

        public async Task LoadAsync()
        {
            if (!File.Exists("recorder.json"))
            {
                _logger.LogWarning("Recorder file not found, skipping load.");
                return;
            }

            var json = await File.ReadAllTextAsync("recorder.json");
            if (string.IsNullOrWhiteSpace(json))
            {
                _logger.LogWarning("Recorder file is empty, skipping load.");
                return;
            }

            var temp = JsonSerializer.Deserialize<ConcurrentDictionary<string, string>>(json);
            if (temp != null)
            {
                _records = temp;
                _logger.LogInformation("Loaded {count} recorded responses.", _records.Count);
            }
        }

        public bool TryGetRecorded(string key, out string? response) =>
           _records.TryGetValue(key, out response);
    }
}
