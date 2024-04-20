using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

[BurstCompile]
public struct JobStruct_SetRandomIntegerInput : IJobParallelFor
{
    [WriteOnly]
    public NativeArray<int> m_input;

    [NativeDisableParallelForRestriction]
    [ReadOnly]
    public NativeArray<uint> m_randomSeed;
    public bool m_useFromTo; 
 public int m_from;
    public int m_to;

    public int m_offset;
    public void Execute(int index)
    {

        if (m_useFromTo && (index < m_from || index > m_to))
            return;

        int rIndex = index + m_offset;
        if(rIndex<0) rIndex *= -1;
        
        uint seed = m_randomSeed[rIndex%m_randomSeed.Length];
        if (seed == 0)
            seed = 42;

        Unity.Mathematics.Random random = new Unity.Mathematics.Random(seed);
        m_input[index] = random.NextInt(1100000000, 1199999999);

    }
}
