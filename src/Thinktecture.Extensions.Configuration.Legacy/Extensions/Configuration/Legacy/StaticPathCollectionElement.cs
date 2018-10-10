using System;
using JetBrains.Annotations;

namespace Thinktecture.Extensions.Configuration.Legacy
{
   /// <summary>
   /// Identifies a collection using a static XML path.
   /// </summary>
   public class StaticPathCollectionElement : ICollectionElement
   {
      private readonly string _path;

      /// <summary>
      /// Initializes new instance of <see cref="StaticPathCollectionElement"/>.
      /// </summary>
      /// <param name="path">XML path of the collection.</param>
      public StaticPathCollectionElement([NotNull] string path)
      {
         _path = path ?? throw new ArgumentNullException(nameof(path));
      }

      /// <inheritdoc />
      public bool IsCollection(string xmlPath)
      {
         return StringComparer.OrdinalIgnoreCase.Equals(_path, xmlPath);
      }
   }
}