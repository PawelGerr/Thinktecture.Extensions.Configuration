using System;
using System.Collections.Generic;

namespace Thinktecture.Extensions.Configuration.Legacy
{
   /// <summary>
   /// Contains information of parsed item of the App.config/Web.config.
   /// </summary>
   internal class LegacyConfigurationItem : ILegacyConfigurationItem
   {
      /// <summary>
      /// Path to use for population of <see cref="Microsoft.Extensions.Configuration.IConfiguration"/>.
      /// </summary>
      public string ConfigurationPath { get; }

      /// <summary>
      /// XML path of the item.
      /// </summary>
      public string XmlPath { get; }

      private Dictionary<string, int> _childIndexesByName;

      /// <summary>
      /// Indexes of child elements of current item.
      /// </summary>
      public IDictionary<string, int> ChildIndexesByName => _childIndexesByName ?? (_childIndexesByName = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase));

      /// <inheritdoc />
      public string BuildXmlPath(string childName)
      {
         return Microsoft.Extensions.Configuration.ConfigurationPath.Combine(XmlPath, childName);
      }

      /// <inheritdoc />
      public string BuildConfigurationPath(string childName)
      {
         return Microsoft.Extensions.Configuration.ConfigurationPath.Combine(ConfigurationPath, childName);
      }

      /// <summary>
      /// Initializes new instance of <see cref="LegacyConfigurationItem"/>
      /// </summary>
      /// <param name="configurationPath">Path to use for population of <see cref="Microsoft.Extensions.Configuration.IConfiguration"/>.</param>
      /// <param name="xmlPath">XML path of the item.</param>
      public LegacyConfigurationItem(string configurationPath, string xmlPath)
      {
         ConfigurationPath = configurationPath ?? throw new ArgumentNullException(nameof(configurationPath));
         XmlPath = xmlPath ?? throw new ArgumentNullException(nameof(xmlPath));
      }
   }
}
