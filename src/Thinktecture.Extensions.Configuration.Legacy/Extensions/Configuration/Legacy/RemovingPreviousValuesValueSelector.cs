using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using JetBrains.Annotations;

namespace Thinktecture.Extensions.Configuration.Legacy
{
   internal class RemovingPreviousValuesValueSelector : IValueSelector
   {
      private readonly string _configurationPathPrefix;

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
