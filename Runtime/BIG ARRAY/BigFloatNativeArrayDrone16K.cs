using Unity.Collections;

public static class BigFloatNativeArrayDrone16K
{
     
     static NativeArray<float> m_dronesAsFloatNative ;


    public static NativeArray<float> GetNativeArray() {
        if (m_dronesAsFloatNative == null || m_dronesAsFloatNative.Length==0)
            CreateNative();
        return m_dronesAsFloatNative;   
    }

    private static void CreateNative()
    {
        if (m_dronesAsFloatNative != null)
            m_dronesAsFloatNative.Dispose();
        m_dronesAsFloatNative = new NativeArray<float>(IMMO16K.ARRAY_MAX_SIZE * 8, Allocator.Persistent);
    }
  
    public static void CreateMemory() {
        CreateNative();
    }
    public static void DestroyMemory() {
        if(m_dronesAsFloatNative!=null)
            m_dronesAsFloatNative.Dispose();
    }
}
