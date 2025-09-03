FROM mcr.microsoft.com/dotnet/sdk:8.0-jammy AS build
WORKDIR /src

# set access to to internal articfact provider
RUN curl -L https://aka.ms/install-artifacts-credprovider.sh | sh
ARG FEED_ACCESSTOKEN
ENV VSS_NUGET_EXTERNAL_FEED_ENDPOINTS="{\"endpointCredentials\": [{\"endpoint\":\"https://pkgs.dev.azure.com/insurwave/_packaging/iw-packages/nuget/v3/index.json\", \"username\":\"docker\", \"password\":\"${FEED_ACCESSTOKEN}\"}]}"

COPY . .
WORKDIR "/src/src/Insurwave.Movie.Api"

FROM build AS publish
RUN dotnet publish "Insurwave.Movie.Api.csproj" \
    --self-contained true \
    -c Release \
    -o /app/out \
    /p:PublishSingleFile=true \
    /p:DebugType=None \
    /p:DebugSymbols=false

# Create runtime image
FROM mcr.microsoft.com/dotnet/runtime-deps:8.0-jammy-chiseled-extra as final

# Install deps
ENV ASPNETCORE_URLS=http://*:5000 \
    DOTNET_RUNNING_IN_CONTAINER=true \
    COMPlus_EnableDiagnostics=0

WORKDIR /app
COPY --from=publish --chown=app:app /app/out ./
USER app:app

EXPOSE 5000
ENTRYPOINT ["./Insurwave.Movie.Api"]