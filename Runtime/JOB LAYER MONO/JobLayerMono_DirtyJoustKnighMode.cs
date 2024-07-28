using Eloi.WatchAndDate;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class JobLayerMono_DirtyJoustKnighMode : MonoBehaviour
{


    public SNAM16K_ObjectBool m_activeInGame;
    public SNAM16K_ObjectBool m_inCollision;

    public SNAM16K_ObjectVector3 m_joustingPoint;
    public SNAM16K_UpdateRandomSeed m_seeds;
    public SNAM16K_ShieldDroneAsUnityFloatValue m_dronesInGame;
    public SNAM16K_EasyDronePositionState m_dronesInGameEasy;
    public DicoClaimToSNAMIndexClaimMono m_claimIndex;

    public float m_radiusInactiveSpanwKill = 7;
    public float m_droneSizeRadius=0.5f;
    public float m_joustSizeRadius = 0.2f;
    public float m_joustFrontDistance = 1.5f;

    public float m_maxTeleportDistance = 256;
    public int m_playerInGame;
    public WatchAndDateTimeActionResult m_totalTime;

    public int m_maxPlayerToWork = 512;

    public bool m_use = false;
    public void LateUpdate()
    {
        if (!m_use)
            return;
        m_totalTime.StartCounting();
        m_claimIndex.GetMaxPlayerReach(out m_playerInGame);
        if (m_playerInGame > m_maxPlayerToWork)
            return;
        STRUCTJob_ResetBooleanToValue job_ResetBooleanToValue = new STRUCTJob_ResetBooleanToValue()
        {
            m_inCollision = m_inCollision.GetNativeArray(),
            m_newValue = false
        };
        job_ResetBooleanToValue.Schedule(IMMO16K.ARRAY_MAX_SIZE, 64).Complete();

        STRUCTJob_DirtyCollisionChecker job_jousting = new STRUCTJob_DirtyCollisionChecker()
        {
            m_inCollision = m_inCollision.GetNativeArray(),
            m_activeInGame = m_activeInGame.GetNativeArray(),
            m_dronesInGame = m_dronesInGame.GetNativeArray(),
            m_joustPoint = m_joustingPoint.GetNativeArray(),
            m_maxPlayerInGame = m_playerInGame,
            m_droneSizeRadius = m_droneSizeRadius,
            m_joustFrontDistance = m_joustFrontDistance,
            m_joustSize = m_joustSizeRadius,
        };
        job_jousting.Schedule(IMMO16K.ARRAY_MAX_SIZE, 64).Complete();


        STRUCTJob_DirtyRespawnInCollision job_teleportCollision = new STRUCTJob_DirtyRespawnInCollision()
        {
            m_inCollision = m_inCollision.GetNativeArray(),
            m_dronesInGame = m_dronesInGameEasy.GetNativeArray(),
            m_seed = m_seeds.GetNativeArray(),
            m_maxDistance = Vector3.one * m_maxTeleportDistance
        };
        job_teleportCollision.Schedule(IMMO16K.ARRAY_MAX_SIZE, 64).Complete();

        m_totalTime.StopCounting();
    }
}

public struct STRUCTJob_DirtyCollisionChecker : IJobParallelFor {

    [NativeDisableParallelForRestriction]
    [ReadOnly]
    public NativeArray<bool> m_activeInGame;
    [NativeDisableParallelForRestriction]
    [ReadOnly]
    public NativeArray<STRUCT_ShieldDroneAsUnity> m_dronesInGame;
    [WriteOnly]
    public NativeArray<Vector3> m_joustPoint;
    [NativeDisableParallelForRestriction]
    public NativeArray<bool> m_inCollision;


    public float m_droneSizeRadius ;
    public float m_joustSize ;
    public float m_joustFrontDistance;
    public int m_maxPlayerInGame;

    public void Execute(int index)
    {
        if (index >= m_maxPlayerInGame) return;
        if (m_inCollision[index]) return;
        //if (!m_activeInGame[index]) return;
        //if (m_dronesInGame[index].m_shield <= 0) return;


        Vector3 currentPosition = m_dronesInGame[index].m_position;
        

        Vector3 joustingPosition = m_dronesInGame[index].m_position
            + (m_dronesInGame[index].m_rotation * Vector3.forward) * m_joustFrontDistance;

        float touchingDistanceDroneJousting = m_droneSizeRadius + m_joustSize;
        m_joustPoint[index] = joustingPosition;
        for (int i = 0; i < m_maxPlayerInGame; i++)
        {

            if (i == index) { }

            else
            {
                if (Vector3.Distance(currentPosition, m_dronesInGame[i].m_position) - m_droneSizeRadius < 0f)
                {
                    m_inCollision[i] = true;
                    m_inCollision[index] = true;
                    return;
                }
                if (Vector3.Distance(joustingPosition, m_dronesInGame[i].m_position) - (touchingDistanceDroneJousting) < 0f)
                {
                    m_inCollision[i] = true;
                    return;
                }
            }
        }
    }

    //private readonly bool IsInBattleZoneOutOfBorder(Vector3 currentPosition)
    //{
    //    return (
    //                currentPosition.x > m_borderOfSafeZone &&
    //                currentPosition.x < (256f - m_borderOfSafeZone) &
    //                currentPosition.y > m_borderOfSafeZone &&
    //                currentPosition.y < (256f - m_borderOfSafeZone) &&
    //                currentPosition.z > m_borderOfSafeZone &&
    //                currentPosition.z < (256f - m_borderOfSafeZone)
    //                );
    //}
}


public struct STRUCTJob_DirtyRespawnInCollision : IJobParallelFor
{
    [ReadOnly]
    public NativeArray<bool> m_inCollision;
    [ReadOnly]
    public NativeArray<uint> m_seed;
    
    public NativeArray<STRUCT_EasyDroneState> m_dronesInGame;


    public Vector3 m_maxDistance;
    public void Execute(int index)
    {
        STRUCT_EasyDroneState drone = m_dronesInGame[index];
        if (m_inCollision[index]) {
            Unity.Mathematics.Random r = new Unity.Mathematics.Random(m_seed[index]);
            drone.m_position = r.NextFloat3(Vector3.zero, m_maxDistance);
            //drone.m_position = new Vector3(128, 128, 128);
        }
        m_dronesInGame[index] = drone;
    }
}
