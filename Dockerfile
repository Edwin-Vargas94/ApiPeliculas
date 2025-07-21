FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY ./publish .
EXPOSE 8080
ENV PORT=8080
ENTRYPOINT ["dotnet", "ApiPeliculas.dll"]
