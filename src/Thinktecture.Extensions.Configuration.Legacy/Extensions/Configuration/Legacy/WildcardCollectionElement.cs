using System;
using JetBrains.Annotations;

namespace Thinktecture.Extensions.Configuration.Legacy
{
   /// <summary>
   /// Supports a wildcard in XML path.
   /// </summary>
   public class WildcardCollectionElement : ICollectionElement
   {
      private readonly string[] _fragments;

      /// <summary>
      /// Initializes new instance of <see cref="WildcardCollectionElement"/>.
      /// </summary>
      /// <param name="pathWithWildcard">XML path containing 1 wildcard '*'.</param>
      /// <exception cref="ArgumentNullException">Provided path is null.</exception>
      /// <exception cref="ArgumentException">Provided path doesn't contains any or more than 1 wildcard. </exception>
      public WildcardCollectionElement([NotNull] string pathWithWildcard)
      {
         if (pathWithWildcard == null)
            throw new ArgumentNullException(nameof(pathWithWildcard));

         _fragments = pathWithWildcard.Split('*');

         if (_fragments.Length == 1)
            throw new ArgumentException($"The path contains no wildcards. Use {nameof(StaticPathCollectionElement)} instead.", nameof(pathWithWildcard));

         if (_fragments.Length > 2)
            throw new ArgumentException("Only 1 wildcard is supported.", nameof(pathWithWildcard));
      }

      /// <inheritdoc />
      public bool IsCollection(string xmlPath)
      {
         return xmlPath.StartsWith(_fragments[0], StringComparison.OrdinalIgnoreCase) &&
                xmlPath.EndsWith(_fragments[1], StringComparison.OrdinalIgnoreCase);
      }
   }
}