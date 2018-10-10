using FluentAssertions;
using Xunit;

namespace Thinktecture.Extensions.Configuration.Legacy.LegacyConfigurationProviderTests
{
   // ReSharper disable once InconsistentNaming
   public class Load_system_ServiceModel_bindings : LoadTestsBase
   {
      [Fact]
      public void Should_ignore_empty_bindings()
      {
         Parse(@"
<configuration>
   <system.serviceModel>
      <bindings>
      </bindings>
   </system.serviceModel>
</configuration>");

         GetData().Should().HaveCount(0);
      }

      [Fact]
      public void Should_ignore_binding_if_it_is_empty()
      {
         Parse(@"
<configuration>
   <system.serviceModel>
      <bindings>
         <basicHttpBinding>
         </basicHttpBinding>
      </bindings>
   </system.serviceModel>
</configuration>");

         GetData().Should().HaveCount(0);
      }

      [Fact]
      public void Should_parse_binding()
      {
         Parse(@"
<configuration>
   <system.serviceModel>
      <bindings>
         <basicHttpBinding>
            <binding maxBufferSize=""100"" maxReceiveBufferSize=""200"" />
         </basicHttpBinding>
      </bindings>
   </system.serviceModel>
</configuration>");

         GetData().Should().HaveCount(2)
                  .And.Contain("system.serviceModel:bindings:basicHttpBinding:binding:0:maxBufferSize", "100")
                  .And.Contain("system.serviceModel:bindings:basicHttpBinding:binding:0:maxReceiveBufferSize", "200");
      }

      [Fact]
      public void Should_parse_multiple_bindings()
      {
         Parse(@"
<configuration>
   <system.serviceModel>
      <bindings>
         <basicHttpBinding>
            <binding maxBufferSize=""100"" maxReceiveBufferSize=""200"" />
            <binding name=""MyBindingConfig"" closeTimeout=""00:01:00"" />
         </basicHttpBinding>
      </bindings>
   </system.serviceModel>
</configuration>");

         GetData().Should().HaveCount(4)
                  .And.Contain("system.serviceModel:bindings:basicHttpBinding:binding:0:maxBufferSize", "100")
                  .And.Contain("system.serviceModel:bindings:basicHttpBinding:binding:0:maxReceiveBufferSize", "200")
                  .And.Contain("system.serviceModel:bindings:basicHttpBinding:binding:1:name", "MyBindingConfig")
                  .And.Contain("system.serviceModel:bindings:basicHttpBinding:binding:1:closeTimeout", "00:01:00")
            ;
      }

      [Fact]
      public void Should_parse_multiple_binding_types()
      {
         Parse(@"
<configuration>
   <system.serviceModel>
      <bindings>
         <basicHttpBinding>
            <binding maxBufferSize=""100"" maxReceiveBufferSize=""200"" />
         </basicHttpBinding>
         <netTcpBinding>
            <binding name=""MyBindingConfig"" closeTimeout=""00:01:00"" />
         </netTcpBinding>
      </bindings>
   </system.serviceModel>
</configuration>");

         GetData().Should().HaveCount(4)
                  .And.Contain("system.serviceModel:bindings:basicHttpBinding:binding:0:maxBufferSize", "100")
                  .And.Contain("system.serviceModel:bindings:basicHttpBinding:binding:0:maxReceiveBufferSize", "200")
                  .And.Contain("system.serviceModel:bindings:netTcpBinding:binding:0:name", "MyBindingConfig")
                  .And.Contain("system.serviceModel:bindings:netTcpBinding:binding:0:closeTimeout", "00:01:00")
            ;
      }
   }
}
