using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;

namespace Thinktecture.Extensions.Configuration.Legacy
{
   /// <summary>
   /// Configuration source for App.config and Web.config.
   /// </summary>
   public class LegacyConfigurationSource : FileConfigurationSource
   {
      /// <inheritdoc />
      [NotNull]
      public override IConfigurationProvider Build(IConfigurationBuilder builder)
      {
         EnsureDefaults(builder);
         return new LegacyConfigurationProvider(this);
      }
   }
}
