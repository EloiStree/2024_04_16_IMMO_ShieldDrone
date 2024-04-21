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

        int value = 1100000000;
        value += 99;
        value += random.NextInt(00, 99) * 100;
        value += random.NextInt(00, 99) * 10000;
        value += random.NextInt(00, 99) * 1000000;
        m_input[index] = value;
    }
}
