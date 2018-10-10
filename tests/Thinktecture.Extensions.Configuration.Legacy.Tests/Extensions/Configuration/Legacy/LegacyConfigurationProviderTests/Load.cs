using System;
using System.Linq;
using System.Text;
using FluentAssertions;
using Xunit;

namespace Thinktecture.Extensions.Configuration.Legacy.LegacyConfigurationProviderTests
{
   public class Load : LoadTestsBase
   {
      [Fact]
      public void Should_ignore_xml_declaration()
      {
         Parse(@"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""no"" ?> <configuration/>");
         SUT.GetChildKeys(Enumerable.Empty<string>(), null).Should().BeEmpty();
      }

      [Fact]
      public void Should_ignore_processing_instruction()
      {
         Parse(@"<?xml-stylesheet type=""text/xsl"" href=""style.xsl""?> <configuration/>");
         SUT.GetChildKeys(Enumerable.Empty<string>(), null).Should().BeEmpty();
      }

      [Fact]
      public void Should_ignore_comment()
      {
         Parse(@"<!-- comment --> <configuration/>");
         SUT.GetChildKeys(Enumerable.Empty<string>(), null).Should().BeEmpty();
      }

      [Fact]
      public void Should_not_emit_any_values_having_self_closing_root_only()
      {
         Parse(@"<configuration/>");
         SUT.GetChildKeys(Enumerable.Empty<string>(), null).Should().BeEmpty();
      }

      [Fact]
      public void Should_not_emit_any_values_having_root_only()
      {
         Parse(@"<configuration></configuration>");
         SUT.GetChildKeys(Enumerable.Empty<string>(), null).Should().BeEmpty();
      }
   }
}
