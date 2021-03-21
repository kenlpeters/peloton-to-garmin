FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

ARG DOTNETRUNTIME

COPY . /build
WORKDIR /build
RUN dotnet publish /build/src/PelotonToGarminConsole/PelotonToGarminConsole.csproj -c Release -r $DOTNETRUNTIME -o /build/published

FROM mcr.microsoft.com/dotnet/runtime:5.0-alpine

ENV PYTHONUNBUFFERED=1
RUN apk add --update --no-cache python3 && ln -sf python3 /usr/bin/python
RUN python3 -m ensurepip
RUN pip3 install --no-cache --upgrade pip setuptools

WORKDIR /app

COPY --from=build /build/published .
COPY --from=build /build/requirements.txt ./requirements.txt
COPY --from=build /build/LICENSE /app/LICENSE
COPY --from=build /build/configuration.example.json /app/configuration.local.json

RUN mkdir -p /app/output
RUN mkdir -p /app/working

RUN touch /app/syncHistory.json
RUN echo "{}" >> /app/syncHistory.json

RUN pip3 install -r requirements.txt
RUN ls -l
ENTRYPOINT ["./PelotonToGarminConsole"]