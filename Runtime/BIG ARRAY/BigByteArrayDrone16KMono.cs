using Unity.Collections;
using UnityEngine;

public class BigByteArrayDrone16KMono :MonoBehaviour{

    public byte[] GetBytesArray() { return BigByteArrayDrone16K.GetBytesArray(); }
}





public static class BigByteArrayCompressedDrone16K
{

    static byte[] m_bigByteArrayOfShieldDrone;


    public static byte[] GetBytesArray()
    {
        if (m_bigByteArrayOfShieldDrone == null || m_bigByteArrayOfShieldDrone.Length == 0)
            CreateNative();
        return m_bigByteArrayOfShieldDrone;
    }


    private static void CreateNative()
    {

        m_bigByteArrayOfShieldDrone = new byte[IMMO16K.ARRAY_MAX_SIZE * 11];
    }


    public static void DestroyMemory()
    {
        m_bigByteArrayOfShieldDrone = null;
    }
}

public static class BigByteNativeArrayCompressedDrone16K
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
        m_bigByteArrayOfShieldDrone = new NativeArray<byte>(IMMO16K.ARRAY_MAX_SIZE * 11, Allocator.Persistent);
    }


    public static void DestroyMemory()
    {
        if (m_bigByteArrayOfShieldDrone.IsCreated)
            m_bigByteArrayOfShieldDrone.Dispose();
    }
}
