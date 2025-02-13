using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerManager : MonoBehaviour
{
    private PlayerShootController playerShootController;

    private void Awake()
    {
        playerShootController = GetComponentInChildren<PlayerShootController>();
        GameManager.instance.gameOverEvent += OnGameOver;
        GameManager.instance.resetFieldEvent += OnResetField;
        GameManager.instance.startStageEvent += OnStartStage;
    }

    private void Start()
    {
        playerShootController.gameObject.SetActive(false);
    }

    private void OnStartStage()
    {
        Debug.Log("Check");
        playerShootController.gameObject.SetActive(true);
    }
    private void OnGameOver()
    {
        playerShootController.gameObject.SetActive(false);
    }
    private void OnResetField()
    {
        playerShootController.gameObject.SetActive(false);
    }
}
