using FluentAssertions;
using Xunit;

namespace Thinktecture.Extensions.Configuration.Legacy.LegacyConfigurationProviderTests
{
   // ReSharper disable once InconsistentNaming
   public class Load_system_ServiceModel_services : LoadTestsBase
   {
      [Fact]
      public void Should_ignore_empty_services()
      {
         Parse(@"
<configuration>
   <system.serviceModel>
      <services>
      </services>
   </system.serviceModel>
</configuration>");

         GetData().Should().HaveCount(0);
      }

      [Fact]
      public void Should_ignore_service_if_it_has_nothing_besides_name()
      {
         Parse(@"
<configuration>
   <system.serviceModel>
      <services>
         <service name=""MyService"">
         </service>
      </services>
   </system.serviceModel>
</configuration>");

         GetData().Should().HaveCount(0);
      }

      [Fact]
      public void Should_parse_service()
      {
         Parse(@"
<configuration>
   <system.serviceModel>
      <services>
         <service name=""MyService"" behaviorConfiguration=""behaviorConfig"">
         </service>
      </services>
   </system.serviceModel>
</configuration>");

         GetData().Should().HaveCount(1)
                  .And.Contain("system.serviceModel:services:MyService:behaviorConfiguration", "behaviorConfig");
      }

      [Fact]
      public void Should_parse_service_endpoint()
      {
         Parse(@"
<configuration>
   <system.serviceModel>
      <services>
         <service name=""MyService"">
            <endpoint address=""address1"" binding=""basicHttpBinding"" contract=""IMyService"" bindingConfiguration=""MyBindingConfig"" />
         </service>
      </services>
   </system.serviceModel>
</configuration>");

         GetData().Should().HaveCount(4)
                  .And.Contain("system.serviceModel:services:MyService:endpoint:0:address", "address1")
                  .And.Contain("system.serviceModel:services:MyService:endpoint:0:binding", "basicHttpBinding")
                  .And.Contain("system.serviceModel:services:MyService:endpoint:0:contract", "IMyService")
                  .And.Contain("system.serviceModel:services:MyService:endpoint:0:bindingConfiguration", "MyBindingConfig");
      }

      [Fact]
      public void Should_parse_multiple_service_endpoints()
      {
         Parse(@"
<configuration>
   <system.serviceModel>
      <services>
         <service name=""MyService"">
            <endpoint address=""address1"" binding=""basicHttpBinding"" contract=""IMyService"" bindingConfiguration=""MyBindingConfig"" />
            <endpoint address=""address2"" binding=""basicHttpBinding2"" contract=""IMyService2"" bindingConfiguration=""MyBindingConfig2"" />
         </service>
      </services>
   </system.serviceModel>
</configuration>");

         GetData().Should().HaveCount(8)
                  .And.Contain("system.serviceModel:services:MyService:endpoint:0:address", "address1")
                  .And.Contain("system.serviceModel:services:MyService:endpoint:0:binding", "basicHttpBinding")
                  .And.Contain("system.serviceModel:services:MyService:endpoint:0:contract", "IMyService")
                  .And.Contain("system.serviceModel:services:MyService:endpoint:0:bindingConfiguration", "MyBindingConfig")
                  .And.Contain("system.serviceModel:services:MyService:endpoint:1:address", "address2")
                  .And.Contain("system.serviceModel:services:MyService:endpoint:1:binding", "basicHttpBinding2")
                  .And.Contain("system.serviceModel:services:MyService:endpoint:1:contract", "IMyService2")
                  .And.Contain("system.serviceModel:services:MyService:endpoint:1:bindingConfiguration", "MyBindingConfig2");
      }

      [Fact]
      public void Should_parse_multiple_services()
      {
         Parse(@"
<configuration>
   <system.serviceModel>
      <services>
         <service name=""MyService"">
            <endpoint address=""address1"" binding=""basicHttpBinding"" contract=""IMyService"" bindingConfiguration=""MyBindingConfig"" />
         </service>
         <service name=""MyService2"">
            <endpoint address=""address2"" binding=""basicHttpBinding2"" contract=""IMyService2"" bindingConfiguration=""MyBindingConfig2"" />
         </service>
      </services>
   </system.serviceModel>
</configuration>");

         GetData().Should().HaveCount(8)
                  .And.Contain("system.serviceModel:services:MyService:endpoint:0:address", "address1")
                  .And.Contain("system.serviceModel:services:MyService:endpoint:0:binding", "basicHttpBinding")
                  .And.Contain("system.serviceModel:services:MyService:endpoint:0:contract", "IMyService")
                  .And.Contain("system.serviceModel:services:MyService:endpoint:0:bindingConfiguration", "MyBindingConfig")
                  .And.Contain("system.serviceModel:services:MyService2:endpoint:0:address", "address2")
                  .And.Contain("system.serviceModel:services:MyService2:endpoint:0:binding", "basicHttpBinding2")
                  .And.Contain("system.serviceModel:services:MyService2:endpoint:0:contract", "IMyService2")
                  .And.Contain("system.serviceModel:services:MyService2:endpoint:0:bindingConfiguration", "MyBindingConfig2");
      }
   }
}
