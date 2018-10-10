using System.Collections.Generic;

namespace Thinktecture.Extensions.Configuration.Legacy
{
   internal class RootLegacyConfigurationItem : ILegacyConfigurationItem
   {
      private readonly string _name;

      /// <inheritdoc />
      public string ConfigurationPath => _name;

      /// <inheritdoc />
      public string XmlPath => _name;

      /// <inheritdoc />
      public IDictionary<string, int> ChildIndexesByName { get; }

      /// <inheritdoc />
      public string BuildXmlPath(string childName)
      {
         return childName;
      }

      /// <inheritdoc />
      public string BuildConfigurationPath(string childName)
      {
         return childName;
      }

      public RootLegacyConfigurationItem(string name)
      {
         _name = name;
         ChildIndexesByName = new Dictionary<string, int>();
      }
   }
}
