### Performance Extractor
Together with the log analyzer (which it needs to run), this provides
an endpoint and a progress bar to parse the performance counters. To build:

To run:
	env "EntrySource:GetUrlTemplate=http://localhost:4999/api/LogEntry?start={start}" dotnet run

or in Docker:

	dotnet publish
	cp Dockerfile bin/Debug/netcoreapp1.1/publish
	docker build bin/Debug/netcoreapp1.1/publish -t performance_extractor
	
