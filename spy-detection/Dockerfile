FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["spy-detection/spy-detection.csproj", "spy-detection/"]
RUN dotnet restore "spy-detection/spy-detection.csproj"
COPY . .
WORKDIR "/src/spy-detection"
RUN dotnet build "spy-detection.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "spy-detection.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "spy-detection.dll"]