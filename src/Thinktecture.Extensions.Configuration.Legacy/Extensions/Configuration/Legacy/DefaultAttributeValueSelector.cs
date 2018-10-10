using System;
using System.Collections.Generic;
using System.Xml;
using JetBrains.Annotations;

namespace Thinktecture.Extensions.Configuration.Legacy
{
   /// <summary>
   /// Adds all attributes to the configuration.
   /// </summary>
   public class DefaultAttributeValueSelector : IValueSelector
   {
      /// <summary>
      /// An instance of <see cref="DefaultAttributeValueSelector"/> without any attribute to ignore.
      /// </summary>
      public static readonly IValueSelector Instance = new DefaultAttributeValueSelector(null);

      private readonly string _attributeToIgnore;

      /// <summary>
      /// Initializes new instance of <see cref="DefaultAttributeValueSelector"/>.
      /// </summary>
      /// <param name="attributeToIgnore">Name of an attribute to ignore.</param>
      public DefaultAttributeValueSelector([CanBeNull]string attributeToIgnore)
      {
         _attributeToIgnore = attributeToIgnore;
      }

      /// <inheritdoc />
      public void Process(XmlReader reader, ILegacyConfigurationItem parent, IDictionary<string, string> data)
      {
         if (reader.NodeType != XmlNodeType.Attribute)
            return;

         if (StringComparer.OrdinalIgnoreCase.Equals(reader.LocalName, _attributeToIgnore))
            return;

         var configPath = parent.BuildConfigurationPath(reader.LocalName);

         if (data.ContainsKey(configPath))
            throw new FormatException($"A duplicate key '{configPath}' was found.{reader.GetLineInfo()}");

         data[configPath] = reader.Value;
      }
   }
}
