using System;
using FluentAssertions;
using Xunit;

namespace Thinktecture.Extensions.Configuration.Legacy.LegacyConfigurationProviderTests
{
   // ReSharper disable once InconsistentNaming
   public class Load_runtime_assemblyBinding_dependentAssembly : LoadTestsBase
   {
      [Fact]
      public void Should_ignore_self_closing_dependentAssembly()
      {
         Parse(@"
<configuration>
   <runtime>
      <assemblyBinding>
         <dependentAssembly />
      </assemblyBinding>
   </runtime>
</configuration>");

         GetData().Should().BeEmpty();
      }

      [Fact]
      public void Should_ignore_empty_dependentAssembly()
      {
         Parse(@"
<configuration>
   <runtime>
      <assemblyBinding>
         <dependentAssembly></dependentAssembly>
      </assemblyBinding>
   </runtime>
</configuration>");

         GetData().Should().BeEmpty();
      }

      [Fact]
      public void Should_parse_assemblyIdentity()
      {
         Parse(@"
<configuration>
   <runtime>
      <assemblyBinding>
         <dependentAssembly>
            <assemblyIdentity name=""myAssembly"" publicKeyToken=""token"" culture=""neutral"" />
         </dependentAssembly>
      </assemblyBinding>
   </runtime>
</configuration>");

         // https://docs.microsoft.com/en-us/dotnet/framework/deployment/configuring-assembly-binding-redirection
         // They may be multiple assemblyBinding within runtime and multiple dependentAssembly within assemblyBinding
         GetData().Should().HaveCount(3)
                  .And.Contain("runtime:assemblyBinding:0:dependentAssembly:0:assemblyIdentity:name", "myAssembly")
                  .And.Contain("runtime:assemblyBinding:0:dependentAssembly:0:assemblyIdentity:publicKeyToken", "token")
                  .And.Contain("runtime:assemblyBinding:0:dependentAssembly:0:assemblyIdentity:culture", "neutral");
      }

      [Fact]
      public void Should_parse_bindingRedirect()
      {
         Parse(@"
<configuration>
   <runtime>
      <assemblyBinding>
         <dependentAssembly>
            <bindingRedirect oldVersion=""0.0.0.0 - 2.0.0.0"" newVersion=""2.0.0.0"" />
         </dependentAssembly>
      </assemblyBinding>
   </runtime>
</configuration>");

         // https://docs.microsoft.com/en-us/dotnet/framework/deployment/configuring-assembly-binding-redirection
         // There may be multiple assemblyBinding within runtime and multiple dependentAssembly within assemblyBinding
         GetData().Should().HaveCount(2)
                  .And.Contain("runtime:assemblyBinding:0:dependentAssembly:0:bindingRedirect:oldVersion", "0.0.0.0 - 2.0.0.0")
                  .And.Contain("runtime:assemblyBinding:0:dependentAssembly:0:bindingRedirect:newVersion", "2.0.0.0");
      }

      [Fact]
      public void Should_parse_multiple_dependent_assemblies()
      {
         Parse(@"
<configuration>
   <runtime>
      <assemblyBinding>
         <dependentAssembly>
            <assemblyIdentity name=""myAssembly"" publicKeyToken=""token"" culture=""neutral"" />
            <bindingRedirect oldVersion=""0.0.0.0 - 2.0.0.0"" newVersion=""2.0.0.0"" />
         </dependentAssembly>
         <dependentAssembly>
            <assemblyIdentity name=""myAssembly2"" publicKeyToken=""token2"" culture=""neutral2"" />
            <bindingRedirect oldVersion=""1.0.0.0 - 3.0.0.0"" newVersion=""3.0.0.0"" />
         </dependentAssembly>
      </assemblyBinding>
   </runtime>
</configuration>");

         // https://docs.microsoft.com/en-us/dotnet/framework/deployment/configuring-assembly-binding-redirection
         // There may be multiple assemblyBinding within runtime and multiple dependentAssembly within assemblyBinding
         GetData().Should().HaveCount(10)
                  .And.Contain("runtime:assemblyBinding:0:dependentAssembly:0:assemblyIdentity:name", "myAssembly")
                  .And.Contain("runtime:assemblyBinding:0:dependentAssembly:0:assemblyIdentity:publicKeyToken", "token")
                  .And.Contain("runtime:assemblyBinding:0:dependentAssembly:0:assemblyIdentity:culture", "neutral")
                  .And.Contain("runtime:assemblyBinding:0:dependentAssembly:0:bindingRedirect:oldVersion", "0.0.0.0 - 2.0.0.0")
                  .And.Contain("runtime:assemblyBinding:0:dependentAssembly:0:bindingRedirect:newVersion", "2.0.0.0")
                  .And.Contain("runtime:assemblyBinding:0:dependentAssembly:1:assemblyIdentity:name", "myAssembly2")
                  .And.Contain("runtime:assemblyBinding:0:dependentAssembly:1:assemblyIdentity:publicKeyToken", "token2")
                  .And.Contain("runtime:assemblyBinding:0:dependentAssembly:1:assemblyIdentity:culture", "neutral2")
                  .And.Contain("runtime:assemblyBinding:0:dependentAssembly:1:bindingRedirect:oldVersion", "1.0.0.0 - 3.0.0.0")
                  .And.Contain("runtime:assemblyBinding:0:dependentAssembly:1:bindingRedirect:newVersion", "3.0.0.0");
      }
   }
}
