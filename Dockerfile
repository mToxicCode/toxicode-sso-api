FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        libc6 \
        libgcc1 \
        libgssapi-krb5-2 \
        libssl1.1 \
        libstdc++6 \
        zlib1g \
        gss-ntlmssp \
        jq \
# Clean
    && rm -rf /tmp/* \
        /usr/share/man/?? \
        /usr/share/man/??_* \
        /var/lib/apt-get/lists/* \
        /var/lib/apt/lists/* 

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG GITHUB_USERNAME
ARG GITHUB_TOKEN
ARG REPOSITORY_OWNER
WORKDIR /src
COPY . ./
RUN dotnet nuget add source --username ${GITHUB_USERNAME} --password ${GITHUB_TOKEN} --store-password-in-clear-text --name github "https://nuget.pkg.github.com/${REPOSITORY_OWNER}/index.json"
RUN dotnet restore "src/ToxiCode.SSO.Api/ToxiCode.SSO.Api.csproj"
RUN dotnet build "src/ToxiCode.SSO.Api/ToxiCode.SSO.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "src/ToxiCode.SSO.Api/ToxiCode.SSO.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ToxiCode.SSO.Api.dll"]
