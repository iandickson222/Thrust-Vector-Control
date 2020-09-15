using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera cinemachineVirtual1;
    public CinemachineVirtualCamera cinemachineVirtual2;

    private bool camera1Active = true;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if(camera1Active)
            {
                cinemachineVirtual1.Priority = 10;
                cinemachineVirtual2.Priority = 20;      
                camera1Active = !camera1Active;
            }
            else
            {
                cinemachineVirtual2.Priority = 10;
                cinemachineVirtual1.Priority = 20;
                camera1Active = !camera1Active;
            }       
        }
    }
}
