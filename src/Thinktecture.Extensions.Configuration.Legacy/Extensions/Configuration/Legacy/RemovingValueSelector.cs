using System.Collections.Generic;
using System.Xml;

namespace Thinktecture.Extensions.Configuration.Legacy
{
   internal class RemovingValueSelector : IValueSelector
   {
      /// <inheritdoc />
      public void Process(XmlReader reader, ILegacyConfigurationItem parent, IDictionary<string, string> data)
      {
         data.Remove(parent.ConfigurationPath);
      }
   }
}
