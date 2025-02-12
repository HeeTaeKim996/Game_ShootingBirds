using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera mainCamera;
    private PlayerShooter playerShooter; 

    private void Awake()
    {
        playerShooter = FindObjectOfType<PlayerShooter>();
    }

    private void Start()
    {
        mainCamera.transform.SetParent(playerShooter.transform);
    }


}
