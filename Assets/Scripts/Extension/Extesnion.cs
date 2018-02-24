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
    public static bool IsEmpty<T>( this List<T> lst ) { return lst.Count == 0; }

    public static bool IsEmpty<T>( this T[] array ) { return array.Length == 0; }
}
