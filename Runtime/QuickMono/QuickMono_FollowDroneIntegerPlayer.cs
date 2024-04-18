using DroneIMMO;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Events;

public class QuickMono_FollowDroneIntegerPlayer : MonoBehaviour
{


    public int m_indexToFollow;
    public IndexType m_indexType;
    public enum IndexType { IntegerIndex, GameIndex}
    
    public Transform m_whatToMove;
    public UnityEvent<float> m_shieldState;
    public SNAM16K_ShieldDroneAsUnityFloatValue  m_droneToFollow;
    public SNAM16K_IntegerPlayerIndexClaim m_indexClaim;

    public int m_indexGameOfTarget;

    public STRUCT_ShieldDroneAsUnity m_droneFocus;

    [ContextMenu("Refresh")]
    public void Refresh() {


        ChangeFocus(m_indexToFollow, m_indexType);
    }
    void Start()
    {

        Refresh();
    }

    public void ChangeFocus(int index)
    {
        ChangeFocus(index, m_indexType);
    }
    public void ChangeFocusToPlayerIndex()
    {
        ChangeFocus(m_indexToFollow,IndexType.IntegerIndex);
    }
    public void ChangeFocusToGameIndex()
    {
        ChangeFocus(m_indexToFollow, IndexType.GameIndex);
    }
    public void ChangeFocus(int index, IndexType type)
    {
        m_indexToFollow = index;
        m_indexType = type;

        if (type == IndexType.IntegerIndex)
        {

            m_indexClaim.GetFromIntegerIndex(index, out bool found, out int indexFound);
            if(found)
            {
                m_indexGameOfTarget = indexFound;
            }
            else
            {
                m_indexGameOfTarget = 0;
            }
        }
        else { 
            m_indexGameOfTarget = index;
        }
    }

    public void FindRandomPlayer() {

        m_indexClaim.GetRandomActivePlayer(out int integerIndex);
        ChangeFocus(integerIndex, IndexType.IntegerIndex);

    }

    public void Update()
    {
        if (m_indexGameOfTarget < 0 || m_indexGameOfTarget >= IMMO16K.ARRAY_MAX_SIZE)
            return;
        m_droneFocus= m_droneToFollow.GetNativeArray()[m_indexGameOfTarget];
        Vector3 position = m_droneFocus.m_position;
        Vector3 direction = m_droneFocus.m_rotation*Vector3.forward;
        direction.y = 0;
        Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
        m_whatToMove.position = position;
        m_whatToMove.rotation = rotation;

    }
}
