using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using JetBrains.Annotations;

namespace Thinktecture.Extensions.Configuration.Legacy
{
   /// <summary>
   /// Removes all values from configuration with a specific prefix.
   /// </summary>
   public class RemovingPreviousValuesValueSelector : IValueSelector
   {
      private readonly string _configurationPathPrefix;

      /// <summary>
      /// Initializes new instance of <see cref="RemovingPreviousValuesValueSelector"/>.
      /// </summary>
      /// <param name="configurationPathPrefix">The prefix to look for.</param>
      public RemovingPreviousValuesValueSelector([NotNull] string configurationPathPrefix)
      {
         _configurationPathPrefix = configurationPathPrefix ?? throw new ArgumentNullException(nameof(configurationPathPrefix));
      }

      /// <inheritdoc />
      public void Process(XmlReader reader, ILegacyConfigurationItem parent, IDictionary<string, string> data)
      {
         foreach (var key in data.Keys.ToList())
         {
            if (key.StartsWith(_configurationPathPrefix, StringComparison.OrdinalIgnoreCase))
               data.Remove(key);
         }
      }
   }
}
