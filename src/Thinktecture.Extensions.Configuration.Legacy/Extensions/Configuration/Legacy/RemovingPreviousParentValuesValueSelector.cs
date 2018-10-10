using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Thinktecture.Extensions.Configuration.Legacy
{
   /// <summary>
   /// Removes the values from configuration that has the same configuration key as the provided parent item.
   /// </summary>
   public class RemovingPreviousParentValuesValueSelector : IValueSelector
   {
      /// <inheritdoc />
      public void Process(XmlReader reader, ILegacyConfigurationItem parent, IDictionary<string, string> data)
      {
         foreach (var key in data.Keys.ToList())
         {
            if (key.StartsWith(parent.ConfigurationPath, StringComparison.OrdinalIgnoreCase))
               data.Remove(key);
         }
      }
   }
}
