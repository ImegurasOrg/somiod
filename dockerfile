FROM mcr.microsoft.com/dotnet/sdk:6.0 as build-env
WORKDIR /App

COPY . ./

RUN dotnet restore
RUN dotnet dev-certs https

ENV ASPNETCORE_URLS="http://*:5150;https://*:7290"
ENV DOTNET_URLS="http://*:5150;https://*:7290"
EXPOSE 5150/tcp

EXPOSE 7290/tcp

CMD [ "dotnet" , "run" ]