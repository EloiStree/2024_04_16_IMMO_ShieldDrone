using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace DroneIMMO
{

    public class SNAM16K_IntegerPlayerIndexClaim : SNAM16K_ObjectInteger
    {
        public Dictionary<int, int> m_dicoIntegerIndexToIndex = new Dictionary<int, int>();

        private new void Awake()
        {

            base.Awake();
            ResetValue();
        }



        public void GetNextFree(out int indexFree)
        {
            for (int i = 0; i < 128 * 128; i++)
            {
                if (GetNativeArray()[i] == 0)
                {
                    indexFree = i;
                    return;
                }
            }
            indexFree = -1;
        }


        public void UnclaimFromNativeArrayIndex(int index)
        {
            var array = GetNativeArray();
            int intergerIndexId = array[index];
            array[index] = 0;
            if (intergerIndexId != 0) { 
                m_dicoIntegerIndexToIndex.Remove(intergerIndexId);
            }
        }

        public void UnclaimFromUserIntegerIndex(int integerIndexId)
        {
            if (!m_dicoIntegerIndexToIndex.ContainsKey(integerIndexId) )
            {
                return;
            }

            int index = m_dicoIntegerIndexToIndex[integerIndexId];
            var array = GetNativeArray();
            array[index] = 0;
            m_dicoIntegerIndexToIndex.Remove(integerIndexId);
        }



        public void ResetValue()
        {
            m_dicoIntegerIndexToIndex.Clear();
            for (int i = 0; i < 128 * 128; i++)
            {
                var array = GetNativeArray();
                array[i] = 0;
            }
        }

        public void ClaimArrayWithUserIntegerIndex(int index, int newIntegerValue)
        {
            var array =  GetNativeArray();
            array[index] = newIntegerValue;

            if (m_dicoIntegerIndexToIndex.ContainsKey(newIntegerValue))
            {
                m_dicoIntegerIndexToIndex[newIntegerValue] = index;
            }
            else
            {
                m_dicoIntegerIndexToIndex.Add(newIntegerValue, index);
            }
        }


        public void GetFromIntegerIndex(int integerIndex, out bool found, out int index)
        {
            found = m_dicoIntegerIndexToIndex.TryGetValue(integerIndex, out index);
        }
        
        public void GetAtIndexTheIntegerIndex(int index, out int integerIndex)
        {

            integerIndex = GetNativeArray()[index];
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
                if (GetNativeArray()[i] != 0)
                {
                    maxIndexReach = i;
                    return;
                }

            }
            maxIndexReach = 0;
        }

        internal void GetRandomActivePlayer(out int integerIndex)
        {
            for (int i = UnityEngine.Random.Range(0,128*128) - 1; i >= 0; i--)
            {
                if (GetNativeArray()[i] != 0)
                {
                    integerIndex = i;
                    return;
                }

            }
            integerIndex = 0;
        }
    }


    //[System.Serializable]
    //public struct IndexToIndexInteger
    //{
    //    public int m_index;
    //    public int m_value;
    //}
    //[System.Serializable]
    //public struct IndexToUlong
    //{
    //    public int m_index;
    //    public ulong m_value;
    //}
}