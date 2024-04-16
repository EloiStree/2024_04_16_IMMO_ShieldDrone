using Unity.Collections;
using UnityEngine;


public class ShieldDroneAsUnity16K {

    static NativeArray<STRUCT_ShieldDroneAsUnity> m_dronesAsStructNative;


    public static NativeArray<STRUCT_ShieldDroneAsUnity> GetNative()
    {
        if (m_dronesAsStructNative == null)
            CreateNative();
        return m_dronesAsStructNative;
    }

    private static void CreateNative()
    {
        if (m_dronesAsStructNative != null)
            m_dronesAsStructNative.Dispose();
        m_dronesAsStructNative = new NativeArray<STRUCT_ShieldDroneAsUnity>(128 * 128 , Allocator.Persistent);
    }
   

    public static void CreateMemory()
    {
        CreateNative();
    }
    public static void DestroyMemory()
    {
        if (m_dronesAsStructNative != null)
            m_dronesAsStructNative.Dispose();
    }
}




public static class FloatArrayDrone16K
{
     static float[] m_dronesAsFloatArray ;
     static NativeArray<float> m_dronesAsFloatNative ;


        public static float[] GetAsArray() {
        if (m_dronesAsFloatArray == null || m_dronesAsFloatNative.Length == 0)
            CreateArray();
        return m_dronesAsFloatArray;   
    }

    public static NativeArray<float> GetNative() {
        if (m_dronesAsFloatNative == null || m_dronesAsFloatNative.Length==0)
            CreateNative();
        return m_dronesAsFloatNative;   
    }

    private static void CreateNative()
    {
        if (m_dronesAsFloatNative != null)
            m_dronesAsFloatNative.Dispose();
        m_dronesAsFloatNative = new NativeArray<float>(128 * 128 * 9, Allocator.Persistent);
    }
    private static void CreateArray()
    {

        m_dronesAsFloatArray= new float[128 * 128 * 9];
    }

    public static void CreateMemory() {
        CreateNative();
        CreateArray();
    }
    public static void DestroyMemory() {
        m_dronesAsFloatArray = null;
        if(m_dronesAsFloatNative!=null)
            m_dronesAsFloatNative.Dispose();
    }
}
