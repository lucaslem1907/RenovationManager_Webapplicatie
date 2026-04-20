FROM mcr.microsoft.com/dotnet/sdk:9.0

WORKDIR /app

COPY . .
RUN dotnet restore Reno/Reno.csproj

WORKDIR /app/Reno

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_LAUNCH_PROFILE=""
EXPOSE 8080

CMD ["dotnet", "watch", "run"]
