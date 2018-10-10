using System;
using System.Collections.Generic;
using System.Xml;

namespace Thinktecture.Extensions.Configuration.Legacy
{
   internal class DefaultAttributeValueSelector : IValueSelector
   {
      public static readonly IValueSelector Instance = new DefaultAttributeValueSelector();

      private DefaultAttributeValueSelector()
      {
      }

      /// <inheritdoc />
      public void Process(XmlReader reader, ILegacyConfigurationItem parent, IDictionary<string, string> data)
      {
         if (reader.NodeType != XmlNodeType.Attribute)
            return;

         var configPath = parent.BuildConfigurationPath(reader.LocalName);

         if (data.ContainsKey(configPath))
            throw new FormatException($"A duplicate key '{configPath}' was found.{reader.GetLineInfo()}");

         data[configPath] = reader.Value;
      }
   }
}
