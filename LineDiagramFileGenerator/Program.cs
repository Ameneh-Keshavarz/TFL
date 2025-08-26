using LineDiagramFileGenerator.LineDiagramModels;
using System.Text.Json;
using TflNetworkBuilder;

namespace LineDiagramFileGenerator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var line = "central";
            var geometry = new GeometryAnalyser(line);
            var fileWriter = new JsxFileWriter(line);
            var fileWriterJson = new JsonFileWriter(line);
            geometry.BuildLineDiagram(fileWriter);
            geometry.BuildLineDiagram(fileWriterJson);
            fileWriter.EndWriting();
            fileWriterJson.EndWriting();
        }
    }

    class JsonFileWriter : ILineDiagramClient, IDisposable
    {
        private readonly string _line;
        private readonly List<LineDiagram> _elements = [];

        public JsonFileWriter(string line)
        {
            _line = line;
        }

        public void AddStationName(int rowNo, int colNo, Station station)
        {
            _elements.Add(new StationName
            {
                Type = "stationName",
                Row = rowNo,
                Col = colNo,
                Name = station?.MatchedStop.FirstOrDefault()?.Name ?? string.Empty,
                StationId = station?.StationId ?? string.Empty,
                Url = $"/stations/{station?.StationId}"
            });
        }

        public void AddStopMarker(int rowNo, int colNo)
        {
            _elements.Add(new Marker
            {
                Type = "marker",
                Row = rowNo,
                Col = colNo
            });
        }

        public void AddTrackSection(int rowNo, int colNo, int targetColNo)
        {
            _elements.Add(new TrackSection
            {
                Type = "trackSection",
                Row = rowNo,
                Col = colNo,
                ColEnd = targetColNo
            });
        }

        public void EndWriting()
        {
            var json = JsonSerializer.Serialize(
                _elements,
                new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(_line + ".json", json);
        }

        public void Dispose()
        {
            // nothing to dispose anymore
        }
    }


    class JsxFileWriter : ILineDiagramClient, IDisposable
    {
        private string _line;
        private StreamWriter _fileStream;
        private bool disposedValue;

        private int _elementCounter = 0;

        public JsxFileWriter(string line)
        {
            _line = line;
            _fileStream = new StreamWriter(_line + ".jsx");
            _fileStream.WriteLine("import React from 'react';");
            _fileStream.WriteLine("import { LineSegment, StationMarker,StationName } from '../Components/LineDiagram';");
            _fileStream.WriteLine("export const content = [ ");
        }
        public void AddStationName(int rowNo, int colNo, Station station)
        {
            var name = station?.MatchedStop.First().Name ?? string.Empty;

            _fileStream.WriteLine("<StationName");
            _fileStream.WriteLine($"  key={{`name-{_elementCounter++}`}}");
            _fileStream.WriteLine($"  x={{{colNo}}}");
            _fileStream.WriteLine($"  y={{{rowNo}}}");
            _fileStream.WriteLine($"  name=\"{name}\"");
            _fileStream.WriteLine("  labelProps={{ fontSize: 12 }}");
            _fileStream.WriteLine("/>,");
        }

        public void AddStopMarker(int rowNo, int colNo)
        {
            _fileStream.WriteLine("<StationMarker");
            _fileStream.WriteLine($"  key={{`marker-{_elementCounter++}`}}");
            _fileStream.WriteLine($"  x={{{colNo}}}");
            _fileStream.WriteLine($"  y={{{rowNo}}}");
            _fileStream.WriteLine("  r={3}");
            _fileStream.WriteLine("  fill=\"#fff\"");
            _fileStream.WriteLine("  stroke=\"#000\"");
            _fileStream.WriteLine("  strokeWidth={2}");
            _fileStream.WriteLine("/>,");
        }

        public void AddTrackSection(int rowNo, int colNo, int targetColNo)
        {
            _fileStream.WriteLine("<LineSegment");
            _fileStream.WriteLine($"  key={{`seg-{_elementCounter++}`}}");
            _fileStream.WriteLine($"  x1={{{colNo}}}");
            _fileStream.WriteLine($"  y1={{{rowNo - 1}}}");
            _fileStream.WriteLine($"  x2={{{targetColNo}}}");
            _fileStream.WriteLine($"  y2={{{rowNo}}}");
            _fileStream.WriteLine("  stroke=\"#E32017\"");
            _fileStream.WriteLine("  strokeWidth={3}");
            _fileStream.WriteLine("  strokeLinecap=\"round\"");
            _fileStream.WriteLine("/>,");
        }

        public void EndWriting()
        {
            _fileStream.WriteLine("];");
            _fileStream.Close();
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _fileStream.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~JsonFileWriter()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
