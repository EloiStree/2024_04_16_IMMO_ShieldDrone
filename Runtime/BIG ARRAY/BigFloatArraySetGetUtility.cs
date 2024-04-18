using UnityEngine;

public class BigFloatArraySetGetUtility {


    public void SetDrone(ref float[] bytesRef, int index, in float[] droneValues)
    {
        for (int i = 0; i < 9; i++)
        {
            bytesRef[index * 9 + i] = droneValues[i];
        }
    }


    public void SetDroneGameState(ref float[] bytesRef, in int index, in float gameState)
    {
        bytesRef[index * 9] = gameState;
    }
    public void SetDronePosition(ref float[] bytesRef, in int index, in float x, in float y, in float z)
    {
        bytesRef[index * 9 + 1] = x;
      bytesRef[index * 9 + 2] = y;
      bytesRef[index * 9 + 3] = z;
    }
    public void SetDronePosition(ref float[] bytesRef, in int index, in Vector3 position)
    {
        bytesRef[index * 9 + 1] = position.x;
       bytesRef[index * 9 + 2] = position.y;
       bytesRef[index * 9 + 3] = position.z;
    }

    public void SetDroneRotation(ref float[] bytesRef, in int index, in float qx, in float qy, in float qz, in float qw)
    {
        bytesRef[index * 9 + 4] = qx;
       bytesRef[index * 9 + 5] = qy;
       bytesRef[index * 9 + 6] = qz;
       bytesRef[index * 9 + 7] = qw;
    }
    public void SetDroneRotation(ref float[] bytesRef, in int index, in Quaternion rotation)
    {
        bytesRef[index * 9 + 4] = rotation.x;
        bytesRef[index * 9 + 5] = rotation.y;
        bytesRef[index * 9 + 6] = rotation.z;
        bytesRef[index * 9 + 7] = rotation.w;
    }

    public void SetDroneShield(ref float[] bytesRef, in int indexDrone, in float shield)
    {

        bytesRef[indexDrone * 9 + 8] = shield;
    }
}