using Unity.Collections;
using Unity.Jobs;

public struct STRUCT_Job_IntegerValueToDroneGamepad : IJobParallelFor
{


    [ReadOnly]
    public NativeArray<int> m_dronesValue;

    [WriteOnly]
    public NativeArray<STRUCT_DroneGamepad> m_gamepads;


    public void Execute(int index)
    {

        STRUCT_DroneGamepad drone = new STRUCT_DroneGamepad();

        int value = m_dronesValue[index];
        float value99 = 0;
        
        value99 = (value / 1000000) % 100;
        if (value99 == 0) drone.m_leftRightRotationPercent11 = 0;
        else Convert99ToPercent11(value99, out drone.m_leftRightRotationPercent11);

        value99 = (value / 10000) % 100;
        if (value99 == 0) drone.m_downUpMovementPercent11 = 0;
        else Convert99ToPercent11(value99, out drone.m_downUpMovementPercent11);

        value99 = (value / 100) % 100;
        if (value99 == 0) drone.m_leftRightMovementPercent11 = 0;
        else Convert99ToPercent11(value99, out drone.m_leftRightMovementPercent11);

        value99 = (value) % 100;
        if (value99 == 0) drone.m_backForwardMovementPercent11 = 0;
        else Convert99ToPercent11(value99, out drone.m_backForwardMovementPercent11);

        m_gamepads[index] = drone;
    }

    private static float Convert99ToPercent11(float value99, out float result)
        =>result = (((value99 - 1f) / 98f) - 0.5f) * 2f;
}
