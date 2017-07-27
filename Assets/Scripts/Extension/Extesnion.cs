using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtesnionMethod  {
    public static void Swap( this int math, ref int rhs, ref int lhs)
    {
        int temp = rhs;
        rhs = lhs;
        lhs = temp;

    }
    public static bool Empty<T>( this List<T> lst ) { return lst.Count == 0; }
}
