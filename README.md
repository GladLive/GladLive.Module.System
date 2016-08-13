# GladLive.Payload

GladLive is network service comparable to Xboxlive or Steam. 

GladLive.Module.System is a module system library and server base for creating modular ASP applications providing:
  - Ability to modularly register controllers
  - Ability to modularly register services
  - Ability to modularlity augment the HTTP pipeline with middlewares
  - Allow configuration without recompilation using *.json files
  - Let's you build either a Monolithic server application or several microservices all without recreating an ASP server
  
## How does it work?

This module system provides 3 key modules that can be implemented in your library:
  - ServiceRegistrationModule - IServiceCollection
  - MvcBuilderServiceRegistrationModule - IMvcBuilder
  - ApplicationConfigurationModule - IApplicationBuilder

Those three modules mirror the typical setup for an ASP core service. By implementing those modules in the GladLive libraries, or your own libraries, you can augment the a GladLive module server.

Registering your modules is as easy as adding them to the GladLive module server's modules.json file. Provide the path and the rest is handled by the application.

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
|**master**| N/A | N/A | N/A | [![Build Status](https://travis-ci.org/GladLive/GladLive.Module.System.svg?branch=master)](https://travis-ci.org/GladLive/GladLive.Payload) |
|**dev**| N/A | N/A | N/A | [![Build Status](https://travis-ci.org/GladLiveGladLive.Module.System.svg?branch=dev)](https://travis-ci.org/GladLive/GladLive.Module.System)|

#### Windows - Unit Tests

(Done locally)

##Licensing

This project is licensed under the MIT license with the additional requirement of adding the GladLive splashscreen to your product.
