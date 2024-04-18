using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Experiment_QuaternionAndEuleurAsBytesCompressed : MonoBehaviour
{
    public Transform m_original;
    public Transform m_euleurCompressed;
    public Transform m_quaternionCompressed;
    public Transform m_minByteToUse;

 
    [Range(0, 255)]
    public float m_minBytes = 125;

    //11 Game state 11 x 11 y 11 z
    //11111111 shield
    //11111111 11111111 (11 x 262143 x)
    //11111111 11111111 (11 x 262143 y)
    //11111111 11111111 (11 x 262143 z)
    //11111111 ex
    //11111111 ey
    //11111111 ez


    public byte[] minBytes = new byte[12]; 
    
    // Update is called once per frame
    void Update()
    {
        Refresh();
    }
    public byte euleurX;
    public byte euleurY;
    public byte euleurZ;
    public byte mineuleurX;
    public byte mineuleurY;
    public byte mineuleurZ;
    public byte qX;
   public byte qY;
   public byte qZ;
   public byte qW;
    public float floatEuleurX;
    public float floatEuleurY;
    public float floatEuleurZ;
    public float minFloatEuleurX ;
    public float minFloatEuleurY ;
    public float minFloatEuleurZ ;
    public float floatQX;
    public float floatQY;
    public float floatQZ;
    public float floatQW;
    private void Refresh()
    {

        Quaternion r = m_original.rotation;
        Vector3 euler = r.eulerAngles;

        euleurX = (byte)(((euler.x % 360) / 360f) * 255f);
        euleurY = (byte)(((euler.y % 360) / 360f) * 255f);
        euleurZ = (byte)(((euler.z % 360) / 360f) * 255f);
        mineuleurX = (byte)(((euler.x % 360) / 360f) *m_minBytes);
        mineuleurY = (byte)(((euler.y % 360) / 360f) *m_minBytes);
        mineuleurZ = (byte)(((euler.z % 360) / 360f) *m_minBytes);

        qX =         (byte)(((r.x + 1f) * 0.5f) * 255f);
        qY=         (byte)(((r.y + 1f) * 0.5f) * 255f); 
        qZ=         (byte)(((r.z + 1f) * 0.5f) * 255f); 
        qW=         (byte)(((r.w + 1f) * 0.5f) * 255f);


        floatEuleurX = (euleurX / 255f) * 360f;
        floatEuleurY = (euleurY / 255f) * 360f;
        floatEuleurZ = (euleurZ / 255f) * 360f;
        minFloatEuleurX = (mineuleurX / m_minBytes) * 360f;
        minFloatEuleurY = (mineuleurY/ m_minBytes) * 360f;
        minFloatEuleurZ = (mineuleurZ/ m_minBytes) * 360f;
        floatQX = ((qX - 122.5f)/122.5f);
        floatQY = ((qY - 122.5f) / 122.5f);
        floatQZ = ((qZ - 122.5f) / 122.5f);
        floatQW = ((qW - 122.5f) / 122.5f);



        m_euleurCompressed.rotation = Quaternion.Euler(
            floatEuleurX,
            floatEuleurY,
            floatEuleurZ
            );
        m_minByteToUse.rotation = Quaternion.Euler(
           minFloatEuleurX,
           minFloatEuleurY,
           minFloatEuleurZ
           );
        m_quaternionCompressed.rotation = new Quaternion(
             floatQX,
             floatQY,
             floatQZ,
             floatQW
            ); 
     
    }
}
