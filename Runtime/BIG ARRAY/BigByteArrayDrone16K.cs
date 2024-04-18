public static class BigByteArrayDrone16K
{

    static byte [] m_bigByteArrayOfShieldDrone;


    public static byte[] GetBytesArray()
    {
        if (m_bigByteArrayOfShieldDrone == null || m_bigByteArrayOfShieldDrone.Length == 0)
            CreateNative();
        return m_bigByteArrayOfShieldDrone;
    }


    private static void CreateNative()
    {

        m_bigByteArrayOfShieldDrone = new byte[IMMO16K.ARRAY_MAX_SIZE*8*4];
    }

    
    public static void DestroyMemory()
    {
        m_bigByteArrayOfShieldDrone = null;
    }
}




