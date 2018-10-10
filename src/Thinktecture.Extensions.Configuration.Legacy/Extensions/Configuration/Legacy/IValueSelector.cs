using System;
using System.Collections.Generic;
using System.Xml;
using JetBrains.Annotations;

namespace Thinktecture.Extensions.Configuration.Legacy
{
   /// <summary>
   /// Adds values to configuration.
   /// </summary>
   public interface IValueSelector
   {
      /// <summary>
      /// Adds values to <paramref name="data"/>.
      /// </summary>
      /// <param name="reader">XML reader to read from.</param>
      /// <param name="parent">Parent configuration item.</param>
      /// <param name="data">Configuration values.</param>
      void Process([NotNull] XmlReader reader, [NotNull] ILegacyConfigurationItem parent, [NotNull] IDictionary<string, string> data);
   }
}
