using System.Collections.Generic;
using JetBrains.Annotations;

namespace Thinktecture.Extensions.Configuration.Legacy
{
   internal interface ILegacyConfigurationItem
   {
      /// <summary>
      /// Path to use for population of <see cref="Microsoft.Extensions.Configuration.IConfiguration"/>.
      /// </summary>
      [NotNull]
      string ConfigurationPath { get; }

      /// <summary>
      /// XML path of the item.
      /// </summary>
      [NotNull]
      string XmlPath { get; }

      /// <summary>
      /// Indexes of child elements of current item.
      /// </summary>
      [NotNull]
      IDictionary<string, int> ChildIndexesByName { get; }

      /// <summary>
      /// Builds child XML path.
      /// </summary>
      /// <param name="childName">The name of the child.</param>
      /// <returns>Child XML path.</returns>
      string BuildXmlPath(string childName);

      /// <summary>
      /// Builds child configuration path.
      /// </summary>
      /// <param name="childName">The name of the child.></param>
      /// <returns>Child configuration path.</returns>
      string BuildConfigurationPath(string childName);
   }
}
