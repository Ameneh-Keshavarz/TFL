using tfl_stats.Server.Services;

namespace tfl_stats.Server.MiddleWare
{
    public class RecorderMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ResponseRecorderService _recordService;

        public RecorderMiddleware(RequestDelegate next, ResponseRecorderService recordService)
        {
            _next = next;
            _recordService = recordService;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            if (!_recordService.IsRecordingEnabled && !_recordService.IsPlayingEnabled)
            {
                await _next(context);
                return;
            }

            var key = context.Request.Path + context.Request.QueryString.ToString();

            if (context.Request.Method == HttpMethods.Post)
            {
                context.Request.EnableBuffering();
                using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
                var body = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;
                key += body;
            }

            if (_recordService.IsPlayingEnabled &&
                _recordService.TryGetRecorded(key, out var recordedResponse))
            {
                context.Response.StatusCode = 200;
                context.Response.ContentType = "application/json";
                if (!string.IsNullOrEmpty(recordedResponse))
                    await context.Response.WriteAsync(recordedResponse);
                return;
            }

            var responseBody = await CaptureResponseAsync(context);


            if (_recordService.IsRecordingEnabled && context.Response.StatusCode == 200)
            {
                _recordService.Record(key, responseBody);
            }
        }

        private async Task<string> CaptureResponseAsync(HttpContext context)
        {
            // Response.Body is the stream ASP.NET Core uses to write the HTTP response.
            // It is connected to socket stream so data goes to the browser.
            // We save it in originalBody so we can restore it later and still deliver the response to the client.
            var originalBody = context.Response.Body;

            // Replace the response stream with this buffer.
            // From this point, anything written to the response will go into memory, not directly to the browser.
            using var buffer = new MemoryStream();
            context.Response.Body = buffer;

            await _next(context);

            buffer.Position = 0;
            using var reader = new StreamReader(buffer, leaveOpen: true);
            string body = await reader.ReadToEndAsync();

            // Reset the cursor again, because reading moved it to the end.
            buffer.Position = 0;
            await buffer.CopyToAsync(originalBody);
            // Since originalBody is the Kestrel stream wired to the socket, this actually sends the response to the browser.
            // Restore Response.Body to point back to the original stream, so future writes behave normally.
            context.Response.Body = originalBody;

            return body;
        }
    }
}
