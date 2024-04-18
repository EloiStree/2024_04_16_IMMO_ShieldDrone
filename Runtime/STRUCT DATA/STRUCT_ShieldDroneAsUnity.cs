using DroneIMMO;
using UnityEngine;


[System.Serializable]
public struct STRUCT_ShieldDroneAsUnity
{

    //public float m_gameSate; Removed
    public Vector3 m_position;
    public Quaternion m_rotation;
    public float m_shield;
}


[System.Serializable]
public struct STRUCT_EasyDroneState
{
    public Vector3 m_position;
    public float m_panHorizontal;
}
