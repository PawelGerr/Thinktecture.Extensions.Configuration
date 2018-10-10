[![Build status](https://ci.appveyor.com/api/projects/status/964wx8kkanx1r383?svg=true)](https://ci.appveyor.com/project/PawelGerr/thinktecture-extensions-configuration)
[![Thinktecture.Extensions.Configuration.Legacy](https://img.shields.io/nuget/v/Thinktecture.Extensions.Configuration.Legacy.svg?maxAge=60)](https://www.nuget.org/packages/Thinktecture.Extensions.Configuration.Legacy/)

See **[wiki](https://github.com/PawelGerr/Thinktecture.Extensions.Configuration/wiki)** for documentation

## Thinktecture.Extensions.Configuration.Legacy
Configuration provider for `App.config` and `Web.config` for smoother migrations to .NET Core.

Nuget: `Install-Package Thinktecture.Extensions.Configuration.Legacy`

### Usage
Use one of the extension methods on `IConfigurationBuilder`.

```
var config = new ConfigurationBuilder()
                    // expecting "MyApplication.exe.config" in "bin" folder
                    // or rather in the same folder as the entry assembly (i.e. MyApplication.exe)
                    .AddAppConfig()

                    // expecting a "Web.config" in the same folder as the entry assembly
                    .AddWebConfig()

                    // Use this overload if the convention mentioned above differ to your application
                    .AddLegacyConfig("path/to/my/application.config")
                    .Build();
```
