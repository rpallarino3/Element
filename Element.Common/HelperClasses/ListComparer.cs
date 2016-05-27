using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Element.Common.HelperClasses
{
    public static class ListComparer
    {

        // this doesn't work completely correctly but for our purposes it should work because of no duplicates
        public static bool AreListsEqual<T>(List<T> list1, List<T> list2)
        {
            if (list1.Count != list2.Count)
                return false;

            foreach (var obj in list1)
            {
                if (!list2.Contains(obj))
                    return false;
            }

            foreach (var obj in list2)
            {
                if (!list1.Contains(obj))
                    return false;
            }

            return true;
        }
    }
}
