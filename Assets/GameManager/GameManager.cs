using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private CameraController cameraController;
    public GameOverManager gameOverManager{ get; private set; }
    public PlayerShooter playerShooter { get; private set; }




    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        cameraController = GetComponentInParent<CameraController>();
        playerShooter = FindObjectOfType<PlayerShooter>();
        gameOverManager = GetComponentInChildren<GameOverManager>();
    }

}
