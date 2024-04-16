using DroneIMMO;
using UnityEngine;

public class TDD_SetRandomlyShieldDrone16K_Ex0 : MonoBehaviour
{
    public NativeArrayMono_ShieldDrone16K m_drones;

    public int m_lastChangeIndex = -1;
    public ShieldDroneAsUShort m_lastChangeDrone;

    public int m_numberPerUpdate = 20;
    void Update()
    {
        for (int i = 0; i < m_numberPerUpdate; i++)
        {
            RandomizeRandomDrone();
        }
    }

    private void RandomizeRandomDrone()
    {
        m_drones.GetRandom(out int index, out ShieldDroneAsUShort drone);
        m_lastChangeIndex = index;
        m_lastChangeDrone = drone;
        ShieldDroneAsUShortUtility.SetRandom(ref drone);
        m_drones.Set(index, drone);
    }
}
