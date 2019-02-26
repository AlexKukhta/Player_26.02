using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Player
{
    public static class ListExtension
    {
        public static List<T> Shuffle<T>(this List<T> items, int initialStep) where T : PlayingItem
        {
            for (var i = initialStep; i > 0; i--)
            {
                var j = 0;
                for (j += initialStep; j < items.Count; j++)
                {
                    var temp = items[j];
                    items[j] = items[j - initialStep];
                    items[j - initialStep] = temp;
                }
            }

            return items;
        }
    }
}
