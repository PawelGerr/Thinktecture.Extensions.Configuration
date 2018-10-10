using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;

namespace Thinktecture.Extensions.Configuration.Legacy
{
   /// <summary>
   /// Configuration provider for App.config and Web.config
   /// </summary>
   public class LegacyConfigurationProvider : FileConfigurationProvider
   {
      private static readonly HashSet<string> _pathsOfCollections;
      private static readonly Dictionary<string, string> _keyAttributeByPath;
      private static Dictionary<string, IValueSelector> _valueSelectorByPath;

      static LegacyConfigurationProvider()
      {
         _pathsOfCollections = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                               {
                                  "runtime:assemblyBinding",
                                  "runtime:assemblyBinding:dependentAssembly",
                                  "system.serviceModel:client:endpoint",
                               };
         _keyAttributeByPath = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                               {
                                  ["appSettings:add"] = "key",
                                  ["appSettings:remove"] = "key",
                                  ["connectionStrings:add"] = "name",
                                  ["connectionStrings:remove"] = "name",
                                  ["system.serviceModel:extensions:bindingElementExtensions:add"] = "key",
                                  ["system.serviceModel:bindings:basicHttpBinding:binding"] = "name",
                                  ["system.serviceModel:bindings:customBinding:binding"] = "name",
                                  ["system.serviceModel:bindings:netTcpBinding:binding"] = "name"
                               };

         _valueSelectorByPath = new Dictionary<string, IValueSelector>(StringComparer.OrdinalIgnoreCase)
                                {
                                   ["appSettings:add"] = new AttributeScalarValueSelector("value", true),
                                   ["appSettings:remove"] = new RemovingPreviousParentValuesValueSelector(),
                                   ["appSettings:clear"] = new RemovingPreviousValuesValueSelector("appSettings:"),
                                   ["connectionStrings:add"] = new DefaultAttributeValueSelector("name"),
                                   ["connectionStrings:remove"] = new RemovingPreviousParentValuesValueSelector(),
                                   ["connectionStrings:clear"] = new RemovingPreviousValuesValueSelector("connectionStrings:")
                                };
      }

      /// <summary>
      /// Initializes a new instance of <see cref="LegacyConfigurationProvider"/>.
      /// </summary>
      /// <param name="source">File configuration source to use.</param>
      public LegacyConfigurationProvider(FileConfigurationSource source)
         : base(source)
      {
      }

      [NotNull]
      private XmlReader CreateReader([NotNull] Stream stream)
      {
         if (stream == null)
            throw new ArgumentNullException(nameof(stream));

         var readerSettings = new XmlReaderSettings
                              {
                                 CloseInput = false,
                                 DtdProcessing = DtdProcessing.Prohibit,
                                 IgnoreComments = true,
                                 IgnoreWhitespace = true
                              };

         return XmlReader.Create(stream, readerSettings);
      }

      /// <inheritdoc />
      public override void Load([NotNull] Stream stream)
      {
         var configValues = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
         var stack = new Stack<ILegacyConfigurationItem>();

         using (var reader = CreateReader(stream))
         {
            var preNodeType = FastForwardToRootElement(reader, stack);

            while (reader.Read())
            {
               switch (reader.NodeType)
               {
                  case XmlNodeType.Element:
                     ReadElementStart(stack, reader, configValues);
                     break;

                  case XmlNodeType.EndElement:
                     ReadElementEnd(stack, preNodeType, configValues);
                     break;

                  case XmlNodeType.CDATA:
                  case XmlNodeType.Text:
                  {
                     ReadElementContent(reader, stack, configValues);
                     break;
                  }

                  case XmlNodeType.XmlDeclaration:
                  case XmlNodeType.ProcessingInstruction:
                  case XmlNodeType.Comment:
                  case XmlNodeType.Whitespace:
                     break;

                  default:
                     throw new FormatException($"The node of type \"{reader.NodeType}\" is not supported.{reader.GetLineInfo()}");
               }

               preNodeType = reader.NodeType;

               // If this element is a self-closing element,
               // we pretend that we just processed an EndElement node
               // because a self-closing element contains an end within itself
               if (preNodeType == XmlNodeType.Element && reader.IsEmptyElement)
                  preNodeType = XmlNodeType.EndElement;
            }
         }

         Data = configValues;
      }

      private static XmlNodeType FastForwardToRootElement([NotNull] XmlReader reader, Stack<ILegacyConfigurationItem> stack)
      {
         var preNodeType = XmlNodeType.None;

         while (reader.Read())
         {
            preNodeType = reader.NodeType;

            if (reader.NodeType != XmlNodeType.XmlDeclaration && reader.NodeType != XmlNodeType.ProcessingInstruction)
            {
               stack.Push(new RootLegacyConfigurationItem(reader.LocalName));
               break;
            }
         }

         return preNodeType;
      }

      private static void ReadElementContent([NotNull] XmlReader reader, [NotNull] Stack<ILegacyConfigurationItem> stack, [NotNull] IDictionary<string, string> data)
      {
         var last = stack.Peek();

         if (data.ContainsKey(last.ConfigurationPath))
            throw new FormatException($"A duplicate key \"{last.ConfigurationPath}\" was found.{reader.GetLineInfo()}");

         data[last.ConfigurationPath] = reader.Value;
      }

      private static void ReadElementEnd(Stack<ILegacyConfigurationItem> stack, XmlNodeType preNodeType, Dictionary<string, string> data)
      {
         var last = stack.Pop();

         // TODO: has to be evaluated whether app.config has something like that
         // If this EndElement node comes right after an Element node and it is not the root element,
         // it means there is no text/CDATA node in current element
         // if (preNodeType == XmlNodeType.Element && stack.Count > 0)
         //    data[last.ConfigurationPath] = String.Empty;
      }

      private static void ReadElementStart([NotNull] Stack<ILegacyConfigurationItem> stack, [NotNull] XmlReader reader, [NotNull] IDictionary<string, string> data)
      {
         stack.Push(CreateStackItem(stack, reader.LocalName));
         AddKeyPrefix(reader, stack);

         var stackItem = stack.Peek();

         if (!_valueSelectorByPath.TryGetValue(stackItem.XmlPath, out var valueSelector))
            valueSelector = DefaultAttributeValueSelector.Instance;

         valueSelector.Process(reader, stackItem, data);

         ProcessAttributes(reader, stackItem, data, valueSelector);

         // If current element is self-closing
         if (reader.IsEmptyElement)
            stack.Pop();
      }

      [NotNull]
      private static ILegacyConfigurationItem CreateStackItem([NotNull] Stack<ILegacyConfigurationItem> stack, [NotNull] string childName)
      {
         var parent = stack.Peek();
         var xmlPath = parent.BuildXmlPath(childName);

         if (_keyAttributeByPath.ContainsKey(xmlPath))
            return new LegacyConfigurationItem(parent.ConfigurationPath, xmlPath);

         var configPath = parent.BuildConfigurationPath(childName);

         if (_pathsOfCollections.Contains(xmlPath))
         {
            if (!parent.ChildIndexesByName.TryGetValue(childName, out var index))
               index = -1;

            parent.ChildIndexesByName[childName] = ++index;
            configPath = ConfigurationPath.Combine(configPath, index.ToString());
         }

         return new LegacyConfigurationItem(configPath, xmlPath);
      }

      private static void ProcessAttributes([NotNull] XmlReader reader,
                                            ILegacyConfigurationItem parent,
                                            IDictionary<string, string> data,
                                            IValueSelector valueSelector)
      {
         for (var i = 0; i < reader.AttributeCount; i++)
         {
            reader.MoveToAttribute(i);
            valueSelector.Process(reader, parent, data);
         }

         // Go back to the element containing the attributes we just processed
         reader.MoveToElement();
      }

      private static void AddKeyPrefix([NotNull] XmlReader reader, [NotNull] Stack<ILegacyConfigurationItem> stack)
      {
         var xmlPath = stack.Peek().XmlPath;

         if (!_keyAttributeByPath.TryGetValue(xmlPath, out var keyAttrName))
            return;

         try
         {
            for (var i = 0; i < reader.AttributeCount; i++)
            {
               reader.MoveToAttribute(i);

               if (StringComparer.OrdinalIgnoreCase.Equals(reader.LocalName, keyAttrName))
               {
                  var last = stack.Pop();
                  stack.Push(new LegacyConfigurationItem(last.BuildConfigurationPath(reader.Value), last.XmlPath));
                  return;
               }
            }

            throw new FormatException($"The element \"{xmlPath}\" must have the attribute \"{keyAttrName}\".{reader.GetLineInfo()}");
         }
         finally
         {
            reader.MoveToElement();
         }
      }
   }
}
