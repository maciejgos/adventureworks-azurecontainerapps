FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["AdventureWorksCore.Api.csproj", "."]
RUN dotnet restore "AdventureWorksCore.Api.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "AdventureWorksCore.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AdventureWorksCore.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AdventureWorksCore.Api.dll"]
