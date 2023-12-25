using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixaiBot.Bussines_Logic.Data_Handling
{
    public static class EnumerableSplitter
    {

        /// <summary>
        /// Splits the <paramref name="source"/> into <paramref name="numberOfLists"/> lists, attempting to evenly distribute elements among them.
        /// </summary>
        /// <param name="source">The source IEnumerable to be split.</param>
        /// <param name="numberOfLists">The desired number of lists to split the source into.</param>
        /// <returns>An IEnumerable of IEnumerable&lt;T&gt; representing the divided lists.</returns>
        public static IEnumerable<IEnumerable<T>> SplitList<T>(this IEnumerable<T> source, int numberOfLists)
       {
           var enumerable = source.ToList();
          
           var chunkSize = enumerable.Count / numberOfLists;

           return enumerable.Chunk(chunkSize);
       }
    }
}
