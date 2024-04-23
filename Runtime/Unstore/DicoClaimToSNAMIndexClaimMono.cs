using DroneIMMO;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Unity.Jobs;
using static UnityEditor.Experimental.GraphView.Port;
using System;

public class DicoClaimToSNAMIndexClaimMono : MonoBehaviour
{
    public SNAM16K_IntegerPlayerIndexClaim m_affectedClaim;
    public SNAM16K_ObjectInt m_affectedValue;

    //
    //public NativeMultiHashMap<int, int> m_hashIntegerIndexToIndex = new NativeMultiHashMap<int, int>(capacity, Allocator.TempJob);
    public Dictionary<int, int> m_dicoIntegerIndexToIndex = new Dictionary<int, int>();

    private  void Awake()
    {

        ResetValue();
    }



    public void GetNextFree(out int indexFree)
    {
        for (int i = 0; i < 128 * 128; i++)
        {
            if (m_affectedClaim.GetNativeArray()[i] == 0)
            {
                indexFree = i;
                m_maxIndexReach = indexFree;
                return;
            }
        }
        indexFree = -1;
        m_maxIndexReach = IMMO16K.ARRAY_MAX_SIZE;
    }


    public void UnclaimFromNativeArrayIndex(int index)
    {
        var array = m_affectedClaim.GetNativeArray();
        int intergerIndexId = array[index];
        array[index] = 0;
        if (intergerIndexId != 0)
        {
            m_dicoIntegerIndexToIndex.Remove(intergerIndexId);
        }
    }

    public void UnclaimFromUserIntegerIndex(int integerIndexId)
    {
        if (!m_dicoIntegerIndexToIndex.ContainsKey(integerIndexId))
        {
            return;
        }

        int index = m_dicoIntegerIndexToIndex[integerIndexId];
        var array = m_affectedClaim.GetNativeArray();
        array[index] = 0;
        m_dicoIntegerIndexToIndex.Remove(integerIndexId);
    }



    public void ResetValue()
    {
        m_dicoIntegerIndexToIndex.Clear();
        for (int i = 0; i < 128 * 128; i++)
        {
            var array = m_affectedClaim.GetNativeArray();
            array[i] = 0;
        }
    }

    public void ClaimArrayIndexWithNewIntegerOwner(int arrayIndex, int newOwner)
    {
        var array = m_affectedClaim.GetNativeArray();
        array[arrayIndex] = newOwner;

        if (m_dicoIntegerIndexToIndex.ContainsKey(newOwner))
        {
            m_dicoIntegerIndexToIndex[newOwner] = arrayIndex;
        }
        else
        {
            m_dicoIntegerIndexToIndex.Add(newOwner, arrayIndex);
        }
        
    }


    public void GetFromIntegerIndex(int integerIndex, out bool found, out int index)
    {
        found = m_dicoIntegerIndexToIndex.TryGetValue(integerIndex, out index);
    }

    public void GetAtIndexTheIntegerIndex(int index, out int integerIndex)
    {

        integerIndex = m_affectedClaim.GetNativeArray()[index];
    }


    public int m_maxIndexReach;
    public void ComputeGetMaxPlayerReach()
    {
        GetMaxPlayerReach(out m_maxIndexReach);
    }

    public bool ContaintsInteger(in int integer)
    {
        return m_dicoIntegerIndexToIndex.ContainsKey(integer);
    }

    public void Update()
    {
        //Should I ?
        ComputeGetMaxPlayerReach();
    }


    public void GetMaxPlayerReach(out int maxIndexReach)
    {
        for (int i = (128 * 128) - 1; i >= 0; i--)
        {
            if (m_affectedClaim.GetNativeArray()[i] != 0)
            {
                maxIndexReach = i;
                return;
            }

        }
        maxIndexReach = 0;
    }

    internal void GetRandomActivePlayer(out int integerIndex)
    {
        for (int i = UnityEngine.Random.Range(0, 128 * 128) - 1; i >= 0; i--)
        {
            if (m_affectedClaim.GetNativeArray()[i] != 0)
            {
                integerIndex = i;
                return;
            }

        }
        integerIndex = 0;
    }

    public void SetOrAddFromIntegerindex(in int indexInteger, in int value)
    {
        var n = m_affectedValue.GetNativeArray();
        GetFromIntegerIndex(indexInteger, out bool found, out int indexArray);
        if (found) {
            n[indexArray] = value;
        }
        else {
            GetNextFree(out int index);
            ClaimArrayIndexWithNewIntegerOwner(index, indexInteger);
            n[indexArray] = value;

        }
    }
}


