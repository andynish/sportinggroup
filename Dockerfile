FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base
ENV ASPNETCORE_URLS http://*:44319

ENV ASPNETCORE_ENVIRONMENT=Development

EXPOSE 44319

WORKDIR /src
COPY ["SG-TechTest/SG-TechTest.csproj", "SG-TechTest/"]
RUN dotnet restore "SG-TechTest/SG-TechTest.csproj"
COPY . .
WORKDIR "/src/SG-TechTest"
RUN dotnet build "SG-TechTest.csproj" -c Release -o /app/build

FROM base AS publish
RUN dotnet publish "SG-TechTest.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SG-TechTest.dll"]

