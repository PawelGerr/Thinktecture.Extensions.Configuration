using System;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Thinktecture.Extensions.Configuration.Legacy.LegacyConfigurationProviderTests
{
   // ReSharper disable once InconsistentNaming
   public class Load_appSettings : LoadTestsBase
   {
      [Fact]
      public void Should_ignore_self_closing_appSettings()
      {
         Parse(@"
<configuration>
   <appSettings />
</configuration>");

         GetData().Should().BeEmpty();
      }

      [Fact]
      public void Should_ignore_empty_appSettings()
      {
         Parse(@"
<configuration>
   <appSettings></appSettings>
</configuration>");

         GetData().Should().BeEmpty();
      }

      [Fact]
      public void Should_throw_if_key_is_not_present()
      {
         Action action = () => Parse(@"
<configuration>
   <appSettings>
      <add value=""Value1"" />
   </appSettings>
</configuration>");

         action.Should().Throw<FormatException>();
      }

      [Fact]
      public void Should_ignore_element_without_a_value()
      {
         Parse(@"
<configuration>
   <appSettings>
      <add key=""Key1"" />
   </appSettings>
</configuration>");

         GetData().Should().BeEmpty();
      }

      [Fact]
      public void Should_add_value()
      {
         Parse(@"
<configuration>
   <appSettings>
      <add key=""Key1"" value=""Value1"" />
   </appSettings>
</configuration>");

         GetData().Should().HaveCount(1)
                  .And.Contain("appSettings:Key1", "Value1");
      }

      [Fact]
      public void Should_add_both_values()
      {
         Parse(@"
<configuration>
   <appSettings>
      <add key=""Key1"" value=""Value1"" />
      <add key=""Key2"" value=""Value2"" />
   </appSettings>
</configuration>");

         GetData().Should().HaveCount(2)
                  .And.Contain("appSettings:Key1", "Value1")
                  .And.Contain("appSettings:Key2", "Value2");
      }

      [Fact]
      public void Should_set_value_to_null_if_key_is_not_present()
      {
         Parse(@"
<configuration>
   <appSettings>
      <remove key=""Key1"" />
   </appSettings>
</configuration>");

         GetData().Should().HaveCount(1)
                  .And.Contain("appSettings:Key1", null);
      }

      [Fact]
      public void Should_set_value_to_null_if_key_is_present()
      {
         Parse(@"
<configuration>
   <appSettings>
      <add key=""Key1"" value=""Value1"" />
      <remove key=""Key1"" />
   </appSettings>
</configuration>");

         GetData().Should().HaveCount(1)
                  .And.Contain("appSettings:Key1", null);
      }

      [Fact]
      public void Should_set_value_coming_after_remove()
      {
         Parse(@"
<configuration>
   <appSettings>
      <remove key=""Key1"" />
      <add key=""Key1"" value=""Value1"" />
   </appSettings>
</configuration>");

         GetData().Should().HaveCount(1)
                  .And.Contain("appSettings:Key1", "Value1");
      }

      [Fact]
      public void Should_do_nothing_if_nothing_to_clear()
      {
         Parse(@"
<configuration>
   <appSettings>
      <clear />
   </appSettings>
</configuration>");

         GetData().Should().HaveCount(0);
      }

      [Fact]
      public void Should_set_value_to_null_before_clear()
      {
         Parse(@"
<configuration>
   <appSettings>
      <add key=""Key1"" value=""Value1"" />
      <clear />
   </appSettings>
</configuration>");

         GetData().Should().HaveCount(1)
                  .And.Contain("appSettings:Key1", null);
      }

      [Fact]
      public void Should_set_value_coming_after_clear()
      {
         Parse(@"
<configuration>
   <appSettings>
      <clear />
      <add key=""Key1"" value=""Value1"" />
   </appSettings>
</configuration>");

         GetData().Should().HaveCount(1)
                  .And.Contain("appSettings:Key1", "Value1");
      }
   }
}
