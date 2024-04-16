
using UnityEngine;

public class SingleUshortShieldDroneDemoMono : MonoBehaviour
{
    public ShieldDroneAsUShort m_droneInfo;
    public Transform m_whatToMove;
    public Transform m_shieldToScale;
    public Renderer m_renderer;
    public Color m_colorFromMin = Color.green*0.5f;
    public Color m_colorToMax = Color.green*1f;

    [Range(-1,1)]
    public float m_tilt;

    [Range(-1, 1)]
    public float m_roll;
    public float m_tiltRollAngle = 75f;
    public float m_droneShieldRadius = 0.2f;

    void Update()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        m_shieldToScale.localScale = Vector3.one * m_droneShieldRadius;
        m_whatToMove.localPosition = new Vector3(
            m_droneInfo.m_quadrantRightX * 0.001f,
            m_droneInfo.m_quadrantHeightY * 0.001f,
            m_droneInfo.m_quadrantDepthZ * 0.001f
            );

        //m_droneInfo.m_percentDronePitch = (sbyte)Mathf.Clamp(m_droneInfo.m_percentDronePitch, -127, 127);
        //m_droneInfo.m_percentDroneRoll = (sbyte)Mathf.Clamp(m_droneInfo.m_percentDroneRoll, -127, 127);

        float angle = (m_droneInfo.m_angleLeftRight360/(float)36000) *360f;   
        m_whatToMove.localRotation = Quaternion.Euler(0, angle, 0);

        if(Application.isPlaying)
            m_renderer.material.color = Color.Lerp(m_colorFromMin, m_colorToMax, m_droneInfo.m_percentShieldState/(float)ushort.MaxValue);


        m_tilt = m_droneInfo.m_percentDronePitch / 127f;//((m_droneInfo.m_percentDronePitch/255f) - 0.5f) * 2f;
        m_roll = m_droneInfo.m_percentDroneRoll / 127f;//((m_droneInfo.m_percentDroneRoll/255f) - 0.5f) * 2f;

        m_whatToMove.Rotate(m_tiltRollAngle * m_tilt, 0, -m_tiltRollAngle * m_roll, Space.Self);
    }

    private void OnValidate()
    {
        UpdatePosition();
    }
}
