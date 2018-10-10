using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Thinktecture.Extensions.Configuration.Legacy.LegacyConfigurationProviderTests
{
   public abstract class LoadTestsBase
   {
      private readonly Mock<FileConfigurationSource> _fileConfigSourceMock;

      private LegacyConfigurationProvider _sut;
      protected LegacyConfigurationProvider SUT => _sut ?? (_sut = new LegacyConfigurationProvider(_fileConfigSourceMock.Object));

      protected LoadTestsBase()
      {
         _fileConfigSourceMock = new Mock<FileConfigurationSource>(MockBehavior.Strict);
      }

      protected void Parse(string xml)
      {
         SUT.Load(GetStream(xml));
      }

      protected IDictionary<string, string> GetData()
      {
         var values = new Dictionary<string, string>();

         ExtractData(null, values);

         return values;
      }

      private void ExtractData(string parentKey, IDictionary<string, string> values)
      {
         var childKeys = SUT.GetChildKeys(Enumerable.Empty<string>(), parentKey);

         foreach (var childKey in childKeys.Distinct())
         {
            var childPath = parentKey == null ? childKey : ConfigurationPath.Combine(parentKey, childKey);

            if (SUT.TryGet(childPath, out var value))
               values.Add(childPath, value);

            ExtractData(childPath, values);
         }
      }

      private static Stream GetStream(string xml)
      {
         var stream = new MemoryStream();

         using (var writer = new StreamWriter(stream, Encoding.UTF8, 4 * 1024, true))
         {
            writer.Write(xml);
         }

         stream.Position = 0;
         return stream;
      }
   }
}
