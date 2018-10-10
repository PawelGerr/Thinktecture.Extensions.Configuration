using System;
using System.Collections.Generic;
using System.Xml;
using JetBrains.Annotations;

namespace Thinktecture.Extensions.Configuration.Legacy
{
   /// <summary>
   /// Fetching just 1 attribute.
   /// </summary>
   public class AttributeScalarValueSelector : IValueSelector
   {
      private readonly string _attributeName;
      private readonly bool _allowDuplicates;

      /// <summary>
      /// Initializes new instance of <see cref="AttributeScalarValueSelector"/>.
      /// </summary>
      /// <param name="attributeName">The name of the attribute to add to configuration.</param>
      /// <param name="allowDuplicates">
      /// If <c>true</c> then the last duplicate overwrites the previous ones; otherwise a <see cref="FormatException"/> is thrown.
      /// </param>
      public AttributeScalarValueSelector([NotNull] string attributeName, bool allowDuplicates)
      {
         _attributeName = attributeName ?? throw new ArgumentNullException(nameof(attributeName));
         _allowDuplicates = allowDuplicates;
      }

      /// <inheritdoc />
      public void Process(XmlReader reader, ILegacyConfigurationItem parent, IDictionary<string, string> data)
      {
         if (reader.NodeType == XmlNodeType.Attribute && StringComparer.OrdinalIgnoreCase.Equals(reader.LocalName, _attributeName))
         {
            if (!_allowDuplicates && data.ContainsKey(parent.ConfigurationPath))
               throw new FormatException($"A duplicate key '{parent.ConfigurationPath}' was found.{reader.GetLineInfo()}");

            data[parent.ConfigurationPath] = reader.Value;
         }
      }
   }
}
