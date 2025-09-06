using System.Collections.Concurrent;

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

        }

        public async Task LoadAsync()
        {

        }

        public bool TryGetRecorded(string key, out string? response) =>
           _records.TryGetValue(key, out response);
    }
}
