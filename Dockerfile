FROM microsoft/aspnetcore:1.1
ENTRYPOINT ["dotnet", "performance_extractor.dll"]
ARG SOURCE=.
WORKDIR /app
EXPOSE 80
COPY $source .
