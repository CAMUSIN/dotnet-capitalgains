FROM mono:latest AS base
#FROM mcr.microsoft.com/dotnet/runtime:7.0
WORKDIR /CapitalGainsTaxes
FROM mono:latest AS build
WORKDIR /CapitalGainsTaxes/CapitalGainsTaxes

COPY ./*.csproj ./

CMD ["dotnet restore" , "./CapitalGainsTaxes.csproj"]
#RUN dotnet restore "./CapitalGainsTaxes.csproj"

COPY . .

WORKDIR "/CapitalGainsTaxes/CapitalGainsTaxes/."
RUN dotnet build -c Release -0 out
RUN dotnet publish -c Release -0 out


FROM mono:latest
WORKDIR /CapitalGainsTaxes
COPY --from=build /CapitalGainsTaxes/out .
#COPY --from=build /CapitalGainsTaxes/out .
ENTRYPOINT ["dotnet","CapitalGainsTaxes.dll"]
dotnet