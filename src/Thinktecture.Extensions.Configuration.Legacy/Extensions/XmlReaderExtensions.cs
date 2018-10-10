using System;
using System.Xml;
using JetBrains.Annotations;

// ReSharper disable once CheckNamespace
namespace Thinktecture
{
   /// <summary>
   /// Extensions for <see cref="XmlReader"/>.
   /// </summary>
   internal static class XmlReaderExtensions
   {
      [NotNull]
      public static string GetLineInfo([NotNull] this XmlReader reader)
      {
         if (reader == null)
            throw new ArgumentNullException(nameof(reader));

         return reader is IXmlLineInfo lineInfo ? $"Line {lineInfo.LineNumber}, position {lineInfo.LinePosition}." : String.Empty;
      }
   }
}
