Test - Hanna Ojaveer

* Prerequisities
    * Install .NET 8 SDK latest version.
    * Use Visual studio 2022 or newer to build/run.

# MyIotService
.NET 8

# DataService.Tests:
Tests for DataService project

Demo works, if You use "Multiple Startup Projects"
These are:
1) DeviceApi
2) ServiceApi
3) DeviceImitator - must be last to Start

* Building
    * Use Visual Studio to build solution.

* Running projects
    * Use Visual Studio to start "Multiple Startup Projects"

* Running unit tests
    * Use Visual Studio to run all unit tests at once

 Note - if there are errors in APIs works - check localhost uri-s

# Swagger  
To use APIs, it important to authorize:
To authorise use "Bearer {token}"
You can get token from "api/login"
you can use:
{
  "username": "test",
  "password": "test1"
}

or create new user and log in with created username and password