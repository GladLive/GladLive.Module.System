# GladLive.Payload

GladLive is network service comparable to Xboxlive or Steam. 

GladLive.Payload provides a centralized repo for common payloads or payload logic for the GladLive distributed network providing:
  - Simplified payload metadata marking
  - GladLive message type information
  - Defines the Hello request/response scheme

## GladLive Services

GladLive.PatchingService: https://github.com/GladLive/GladLive.PatchingService

GladLive.AuthService.ASP: https://github.com/GladLive/GladLive.AuthService.ASP

## Setup

To use this project you'll first need a couple of things:
  - Visual Studio 2015 RC 3
  - ASP Core VS Tools
  - Dotnet SDK
  - Add Nuget Feed https://www.myget.org/F/hellokitty/api/v2 in VS (Options -> NuGet -> Package Sources)

## Builds

Available on HelloKitty Nuget feed:  https://www.myget.org/F/hellokitty/api/v2

##Tests

#### Linux/Mono - Unit Tests
||Debug x86|Debug x64|Release x86|Release x64|
|:--:|:--:|:--:|:--:|:--:|:--:|
|**master**| N/A | N/A | N/A | [![Build Status](https://travis-ci.org/GladLive/GladLive.Payload.svg?branch=master)](https://travis-ci.org/GladLive/GladLive.Payload) |
|**dev**| N/A | N/A | N/A | [![Build Status](https://travis-ci.org/GladLive/GladLive.Payload.svg?branch=dev)](https://travis-ci.org/GladLive/GladLive.Payload)|

#### Windows - Unit Tests

(Done locally)

##Licensing

This project is licensed under the MIT license with the additional requirement of adding the GladLive splashscreen to your product.
