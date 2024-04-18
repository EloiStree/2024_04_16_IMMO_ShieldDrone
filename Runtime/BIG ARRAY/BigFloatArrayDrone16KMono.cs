using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigFloatArrayDrone16KMono : MonoBehaviour
{
    public float[] GetFloatArray()
    {
       return BigFloatArrayDrone16K.GetFloatArray();
    }

    public int Length()
    {
        return BigFloatArrayDrone16K.Length();
    }
}
