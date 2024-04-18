public static class BigFloatArrayDrone16K
{

    static float[] m_bigFloatArrayOfShieldDrone;


    public static float[] GetFloatArray()
    {
        if (m_bigFloatArrayOfShieldDrone == null || m_bigFloatArrayOfShieldDrone.Length == 0)
            CreateNative();
        return m_bigFloatArrayOfShieldDrone;
    }


    private static void CreateNative()
    {

        m_bigFloatArrayOfShieldDrone = new float[IMMO16K.ARRAY_MAX_SIZE * 8 * 4];
    }


    public static void DestroyMemory()
    {
        m_bigFloatArrayOfShieldDrone = null;
    }

    public static int Length()
    {
        return m_bigFloatArrayOfShieldDrone.Length;
    }
}
