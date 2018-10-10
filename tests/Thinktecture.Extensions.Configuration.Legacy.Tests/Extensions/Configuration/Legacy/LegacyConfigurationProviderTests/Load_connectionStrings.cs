using System;
using FluentAssertions;
using Xunit;

namespace Thinktecture.Extensions.Configuration.Legacy.LegacyConfigurationProviderTests
{
   // ReSharper disable once InconsistentNaming
   public class Load_connectionStrings : LoadTestsBase
   {
      [Fact]
      public void Should_ignore_self_closing_connectionStrings()
      {
         Parse(@"
<configuration>
   <connectionStrings />
</configuration>");

         GetData().Should().BeEmpty();
      }

      [Fact]
      public void Should_ignore_empty_connectionStrings()
      {
         Parse(@"
<configuration>
   <connectionStrings></connectionStrings>
</configuration>");

         GetData().Should().BeEmpty();
      }

      [Fact]
      public void Should_throw_if_name_is_not_present()
      {
         Action action = () => Parse(@"
<configuration>
   <connectionStrings>
      <add />
   </connectionStrings>
</configuration>");

         action.Should().Throw<FormatException>();
      }

      [Fact]
      public void Should_ignore_element_without_anything_besides_name()
      {
         Parse(@"
<configuration>
   <connectionStrings>
      <add name=""Name1"" />
   </connectionStrings>
</configuration>");

         GetData().Should().BeEmpty();
      }

      [Fact]
      public void Should_add_connectionString()
      {
         Parse(@"
<configuration>
   <connectionStrings>
      <add name=""Name1"" connectionString=""ConnString1"" providerName=""Provider1"" />
   </connectionStrings>
</configuration>");

         GetData().Should().HaveCount(2)
                  .And.Contain("connectionStrings:Name1:connectionString", "ConnString1")
                  .And.Contain("connectionStrings:Name1:providerName", "Provider1");
      }

      [Fact]
      public void Should_add_both_connectionStrings()
      {
         Parse(@"
<configuration>
   <connectionStrings>
      <add name=""Name1"" connectionString=""ConnString1"" providerName=""Provider1"" />
      <add name=""Name2"" connectionString=""ConnString2"" providerName=""Provider2"" />
   </connectionStrings>
</configuration>");

         GetData().Should().HaveCount(4)
                  .And.Contain("connectionStrings:Name1:connectionString", "ConnString1")
                  .And.Contain("connectionStrings:Name1:providerName", "Provider1")
                  .And.Contain("connectionStrings:Name2:connectionString", "ConnString2")
                  .And.Contain("connectionStrings:Name2:providerName", "Provider2");
      }

      [Fact]
      public void Should_do_nothing_if_no_connection_string_is_set()
      {
         Parse(@"
<configuration>
   <connectionStrings>
      <remove name=""Name1"" />
   </connectionStrings>
</configuration>");

         GetData().Should().HaveCount(0);
      }

      [Fact]
      public void Should_remove_connection_string_if_name_is_present()
      {
         Parse(@"
<configuration>
   <connectionStrings>
      <add name=""Name1"" connectionString=""ConnString1"" providerName=""Provider1"" />
      <remove name=""Name1"" />
   </connectionStrings>
</configuration>");

         GetData().Should().HaveCount(0);
      }

      [Fact]
      public void Should_not_touch_other_connection_string()
      {
         Parse(@"
<configuration>
   <connectionStrings>
      <add name=""Name1"" connectionString=""ConnString1"" providerName=""Provider1"" />
      <remove name=""Name2"" />
   </connectionStrings>
</configuration>");

         GetData().Should().HaveCount(2)
                  .And.Contain("connectionStrings:Name1:connectionString", "ConnString1")
                  .And.Contain("connectionStrings:Name1:providerName", "Provider1");
      }

      [Fact]
      public void Should_set_connectionString_coming_after_remove()
      {
         Parse(@"
<configuration>
   <connectionStrings>
      <remove name=""Name1"" />
      <add name=""Name1"" connectionString=""ConnString1"" providerName=""Provider1"" />
   </connectionStrings>
</configuration>");

         GetData().Should().HaveCount(2)
                  .And.Contain("connectionStrings:Name1:connectionString", "ConnString1")
                  .And.Contain("connectionStrings:Name1:providerName", "Provider1");
      }

      [Fact]
      public void Should_do_nothing_if_nothing_to_clear()
      {
         Parse(@"
<configuration>
   <connectionStrings>
      <clear />
   </connectionStrings>
</configuration>");

         GetData().Should().HaveCount(0);
      }

      [Fact]
      public void Should_remove_previously_set_connection_strings()
      {
         Parse(@"
<configuration>
   <connectionStrings>
      <add name=""Name1"" connectionString=""ConnString1"" providerName=""Provider1"" />
      <add name=""Name2"" connectionString=""ConnString2"" providerName=""Provider2"" />
      <clear />
   </connectionStrings>
</configuration>");

         GetData().Should().HaveCount(0);
      }

      [Fact]
      public void Should_set_connection_string_coming_after_clear()
      {
         Parse(@"
<configuration>
   <connectionStrings>
      <clear />
      <add name=""Name1"" connectionString=""ConnString1"" providerName=""Provider1"" />
   </connectionStrings>
</configuration>");

         GetData().Should().HaveCount(2)
                  .And.Contain("connectionStrings:Name1:connectionString", "ConnString1")
                  .And.Contain("connectionStrings:Name1:providerName", "Provider1");
      }
   }
}
