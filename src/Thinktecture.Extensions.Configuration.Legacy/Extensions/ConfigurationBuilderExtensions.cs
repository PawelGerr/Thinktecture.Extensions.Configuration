using System;
using System.IO;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Thinktecture.Extensions.Configuration.Legacy;

// ReSharper disable once CheckNamespace
namespace Thinktecture
{
   /// <summary>
   /// Extensions for <see cref="IConfigurationBuilder"/>.
   /// </summary>
   public static class ConfigurationBuilderExtensions
   {
      /// <summary>
      /// Adds values of a App.config to <see cref="IConfiguration"/> using default conventions.
      /// It is expected that the configuration file resides in the same folder as the executable assembly
      /// and has the same name ending with ".config"
      /// </summary>
      /// <param name="builder">Configuration builder.</param>
      /// <param name="optional">Determines if loading the file is optional.</param>
      /// <param name="reloadOnChange">Determines whether the source will be loaded if the underlying file changes.</param>
      /// <returns>Provided <paramref name="builder"/> for chaining.</returns>
      public static IConfigurationBuilder AddAppConfig([NotNull] this IConfigurationBuilder builder, bool optional = false, bool reloadOnChange = false)
      {
         return AddLegacyConfig(builder, GetDefaultAppConfigPath(), optional, reloadOnChange);
      }

      /// <summary>
      /// Adds values of a Web.config to <see cref="IConfiguration"/> using default conventions.
      /// It is expected that the configuration file resides in the same folder as the executable assembly
      /// and has the name "Web.config"
      /// </summary>
      /// <param name="builder">Configuration builder.</param>
      /// <param name="optional">Determines if loading the file is optional.</param>
      /// <param name="reloadOnChange">Determines whether the source will be loaded if the underlying file changes.</param>
      /// <returns>Provided <paramref name="builder"/> for chaining.</returns>
      public static IConfigurationBuilder AddWebConfig([NotNull] this IConfigurationBuilder builder, bool optional = false, bool reloadOnChange = false)
      {
         return AddLegacyConfig(builder, GetDefaultWebConfigPath(), optional, reloadOnChange);
      }

      /// <summary>
      /// Adds values of a App.config or Web.config residing in <paramref name="path"/> to <see cref="IConfiguration"/>.
      /// </summary>
      /// <param name="builder">Configuration builder.</param>
      /// <param name="path">The path of the App.config or Web.config</param>
      /// <param name="optional">Determines if loading the file is optional.</param>
      /// <param name="reloadOnChange">Determines whether the source will be loaded if the underlying file changes.</param>
      /// <returns>Provided <paramref name="builder"/> for chaining.</returns>
      public static IConfigurationBuilder AddLegacyConfig([NotNull] this IConfigurationBuilder builder, [NotNull] string path, bool optional = false, bool reloadOnChange = false)
      {
         return AddLegacyConfig(builder, builder.GetFileProvider(), path, optional, reloadOnChange);
      }

      /// <summary>
      /// Adds values of a App.config or Web.config residing in <paramref name="path"/> to <see cref="IConfiguration"/>.
      /// </summary>
      /// <param name="builder">Configuration builder.</param>
      /// <param name="provider">The file provider to use for loading the configuration file.</param>
      /// <param name="path">The path of the App.config or Web.config</param>
      /// <param name="optional">Determines if loading the file is optional.</param>
      /// <param name="reloadOnChange">Determines whether the source will be loaded if the underlying file changes.</param>
      /// <returns>Provided <paramref name="builder"/> for chaining.</returns>
      public static IConfigurationBuilder AddLegacyConfig([NotNull] this IConfigurationBuilder builder, [NotNull] IFileProvider provider, [NotNull] string path, bool optional = false, bool reloadOnChange = false)
      {
         if (builder == null)
            throw new ArgumentNullException(nameof(builder));
         if (provider == null)
            throw new ArgumentNullException(nameof(provider));
         if (path == null)
            throw new ArgumentNullException(nameof(path));

         path = GetDefaultAppConfigPath();

         return builder.Add<LegacyConfigurationSource>(s =>
                                                       {
                                                          s.FileProvider = provider;
                                                          s.Path = path;
                                                          s.Optional = optional;
                                                          s.ReloadOnChange = reloadOnChange;
                                                          s.ResolveFileProvider();
                                                       });
      }

      [NotNull]
      private static string GetDefaultAppConfigPath()
      {
         var (exeDirectory, exeFileName) = GetExecutableAssemblyInfos();

         return BuildConfigurationFilePath(exeDirectory, $"{exeFileName}.config");
      }

      [NotNull]
      private static string GetDefaultWebConfigPath()
      {
         var (exeDirectory, _) = GetExecutableAssemblyInfos();

         return BuildConfigurationFilePath(exeDirectory, "Web.config");
      }

      [NotNull]
      private static string BuildConfigurationFilePath([CanBeNull] string exeDirectory, string configFileName)
      {
         if (String.IsNullOrWhiteSpace(exeDirectory))
            return configFileName;

         return Path.Combine(exeDirectory, configFileName);
      }

      private static (string Directory, string FileName) GetExecutableAssemblyInfos()
      {
         var exeAssembly = Assembly.GetEntryAssembly();
         var exeFileName = Path.GetFileName(exeAssembly.Location);
         var exeDirectory = Path.GetDirectoryName(exeAssembly.Location);

         return (exeDirectory, exeFileName);
      }
   }
}
