using System;
using System.Collections.Generic;
using System.Xml;
using JetBrains.Annotations;

namespace Thinktecture.Extensions.Configuration.Legacy
{
   internal interface IValueSelector
   {
      void Process([NotNull] XmlReader reader, [NotNull] ILegacyConfigurationItem parent, [NotNull] IDictionary<string, string> data);
   }
}
