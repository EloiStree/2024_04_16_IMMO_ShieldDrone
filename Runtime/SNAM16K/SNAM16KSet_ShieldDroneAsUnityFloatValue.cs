using System;

public class SNAM16KSet_ShieldDroneAsUnityFloatValue : StaticNativeArrayMono_SetDebugGeneric16K<STRUCT_ShieldDroneAsUnity>
{
    public override STRUCT_ShieldDroneAsUnity GenerateRandomValue()
    {
        return new STRUCT_ShieldDroneAsUnity()
        {
            m_position = new UnityEngine.Vector3(
                GetRandomDistance(), GetRandomDistance(), GetRandomDistance()),
            m_rotation = new UnityEngine.Quaternion(GetRandomQuad(), GetRandomQuad(), GetRandomQuad(), GetRandomQuad()),
            m_shield = UnityEngine.Random.Range(0, ushort.MaxValue)

        };
    }

    private static float GetRandomQuad()
    {
        return UnityEngine.Random.Range(-1f, 1f);
    }

    private static float GetRandomDistance()
    {
        return UnityEngine.Random.Range(0f, 250f);
    }
}
