using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extension
{
    public static Vector3 Clone(this Vector3 v)
    {
        return new Vector3()
        {
            x = v.x,
            y = v.y,
            z = v.z
        };
    }
    
    public static void ChangeProperty( this Vector3 v, float x, float y, float z)
    {
        if( x != float.NaN)
        {
            v.x = x; 
        }
        if (y != float.NaN)
        {
            v.y = y;
        }
        if (z != float.NaN)
        {
            v.z = z;
        }
    }

}