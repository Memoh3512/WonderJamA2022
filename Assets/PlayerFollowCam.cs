using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerFollowCam : MonoBehaviour
{
    private CinemachineVirtualCamera vcam;

    [Header("Cam params")] public float minZoom;
    public float maxZoom;
    
    // Start is called before the first frame update
    void Start()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
    }

    public void Zoom(float delta)
    {

        vcam.m_Lens.OrthographicSize = Mathf.Clamp(vcam.m_Lens.OrthographicSize +delta, minZoom, maxZoom);

    }

}
