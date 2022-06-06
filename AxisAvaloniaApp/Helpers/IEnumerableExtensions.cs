using System.Collections.Generic;
using System.Linq;

namespace AxisAvaloniaApp.Helpers
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> Add<T>(this IEnumerable<T> e, T value)
        {
            foreach (var cur in e)
            {
                yield return cur;
            }
            yield return value;
        }

        public static int IndexOf<T>(this IEnumerable<T> obj, T value)
        {
            return obj.IndexOf(value, null);
        }

        public static int IndexOf<T>(this IEnumerable<T> obj, T value, IEqualityComparer<T> comparer)
        {
            comparer = comparer ?? EqualityComparer<T>.Default;
            var found = obj
                .Select((a, i) => new { a, i })
                .FirstOrDefault(x => comparer.Equals(x.a, value));
            return found == null ? -1 : found.i;
        }
    }
}
