using System.Collections.Generic;
using System.Xml;

namespace Thinktecture.Extensions.Configuration.Legacy
{
   internal class NullifyingValueSelector : IValueSelector
   {
      /// <inheritdoc />
      public void Process(XmlReader reader, ILegacyConfigurationItem parent, IDictionary<string, string> data)
      {
         data[parent.ConfigurationPath] = null;
      }
   }
}