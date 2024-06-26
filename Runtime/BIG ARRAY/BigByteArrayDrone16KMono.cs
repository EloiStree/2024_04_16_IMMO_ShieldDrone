﻿using Unity.Collections;
using UnityEngine;

public class BigByteArrayDrone16KMono :MonoBehaviour{

    public byte[] GetBytesArray() { return BigByteArrayDrone16K.GetBytesArray(); }
}

public static class BigByteNativeArrayIntegerId16K
{

    static NativeArray<byte> m_bigByteArrayOfShieldDrone;


    public static NativeArray<byte> GetBytesNativeArray()
    {
        if (m_bigByteArrayOfShieldDrone == null || m_bigByteArrayOfShieldDrone.Length == 0)
            CreateNative();
        return m_bigByteArrayOfShieldDrone;
    }


    private static void CreateNative()
    {
        if (m_bigByteArrayOfShieldDrone.IsCreated)
            m_bigByteArrayOfShieldDrone.Dispose();
        m_bigByteArrayOfShieldDrone = new NativeArray<byte>(IMMO16K.ARRAY_MAX_SIZE * 4, Allocator.Persistent);
    }


    public static void DestroyMemory()
    {
        if (m_bigByteArrayOfShieldDrone.IsCreated)
            m_bigByteArrayOfShieldDrone.Dispose();
    }
}
