FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["ielts-writing-microservices-taskSampleAPI/ielts_writing_microservices_taskSampleAPI.csproj", "ielts-writing-microservices-taskSampleAPI/"]

RUN dotnet restore "ielts-writing-microservices-taskSampleAPI/ielts_writing_microservices_taskSampleAPI.csproj"
COPY . .
WORKDIR "/src/ielts-writing-microservices-taskSampleAPI"
RUN dotnet build "ielts_writing_microservices_taskSampleAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ielts_writing_microservices_taskSampleAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ielts_writing_microservices_taskSampleAPI.dll"]