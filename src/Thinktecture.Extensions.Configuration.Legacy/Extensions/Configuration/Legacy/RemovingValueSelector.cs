using System.Collections.Generic;
using System.Xml;

namespace Thinktecture.Extensions.Configuration.Legacy
{
   /// <summary>
   /// Removes the values from configuration that has the same configuration key as the provided parent item.
   /// </summary>
   public class RemovingValueSelector : IValueSelector
   {
      /// <inheritdoc />
      public void Process(XmlReader reader, ILegacyConfigurationItem parent, IDictionary<string, string> data)
      {
         data.Remove(parent.ConfigurationPath);
      }
   }
}
