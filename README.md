### Performance Extractor
Together with the log analyzer (which it needs to run), this provides
an endpoint and a progress bar to parse the performance counters. To build:

To run:
	env "EntrySource:GetUrlTemplate=http://localhost:4999/api/LogEntry?start={start}" dotnet run

