FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["HotDeskAPI/HotDeskAPI.csproj", "HotDeskAPI/"]
RUN dotnet restore "HotDeskAPI/HotDeskAPI.csproj"
COPY . .
WORKDIR "/src/HotDeskAPI"
RUN dotnet build "HotDeskAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HotDeskAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HotDeskAPI.dll"]
