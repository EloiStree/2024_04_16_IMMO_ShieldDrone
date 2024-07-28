using Unity.Collections;

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
