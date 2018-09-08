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

}