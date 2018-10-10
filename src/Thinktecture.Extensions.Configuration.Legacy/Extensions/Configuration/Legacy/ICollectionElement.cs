using System;

namespace Thinktecture.Extensions.Configuration.Legacy
{
   /// <summary>
   /// Determines whether an XML path belongs to a collection or not.
   /// </summary>
   public interface ICollectionElement
   {
      /// <summary>
      /// Determines whether an XML path belongs to a collection or not.
      /// </summary>
      /// <param name="xmlPath">XML path to test.</param>
      /// <returns>
      /// <c>true</c> if the path is a collection; otherwise <c>false</c>.
      /// </returns>
      bool IsCollection(string xmlPath);
   }
}
