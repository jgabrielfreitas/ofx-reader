using System;
using System.Collections.Generic;
using System.Linq;

namespace OFX.Reader.Common {

    public static class LinqExtensions {

        public static IEnumerable<T> ExceptWhere<T>(this IEnumerable<T> source, Predicate<T> predicate)
        {
            return source.Where(x=>!predicate(x));
        }

    }

}