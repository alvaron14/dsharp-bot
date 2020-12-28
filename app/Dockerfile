FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.sln .
WORKDIR /.
COPY Bot_Discord_CSharp/*.csproj ./aspnetapp/
WORKDIR /app
#COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY Bot_Discord_CSharp/. ./aspnetapp/
WORKDIR /app/aspnetapp
#COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 as runtime
WORKDIR /app
COPY --from=build /app/aspnetapp/out ./
#COPY --from=build-env /app/out .


#COPY config.json ./
COPY Properties/launchSettings.json ./

ENTRYPOINT ["dotnet", "Bot_Discord_CSharp.dll"]