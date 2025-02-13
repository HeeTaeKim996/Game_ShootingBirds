using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private CameraController cameraController;
    private UIManager uiManager;
    public GameOverPanel gameOverPanel { get; private set; }
    public PlayerShooter playerShooter { get; private set; }
    public HubSelectPanel hubSelectPanel { get; private set; }
    public PlayerControllerManager playerControllerManager { get; private set; }

    public event Action startStageEvent;
    public event Action gameOverEvent;
    public event Action resetFieldEvent;
    public StageResultPanel stageResultPanel;
    public EnemySpawnerManager enemySpawnManager { get; private set; }


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
        uiManager = FindObjectOfType<UIManager>();
        cameraController = GetComponentInParent<CameraController>();
        playerShooter = uiManager.GetComponentInChildren<PlayerShooter>();
        gameOverPanel = uiManager.GetComponentInChildren<GameOverPanel>();
        hubSelectPanel = uiManager.GetComponentInChildren<HubSelectPanel>();
        enemySpawnManager = FindObjectOfType<EnemySpawnerManager>();
        stageResultPanel = uiManager.GetComponentInChildren<StageResultPanel>();
        playerControllerManager = FindObjectOfType<PlayerControllerManager>();
    }
    private void Start()
    {
        InvokeHubSelectOn();
    }


    public void RestartGame()
    {
        resetFieldEvent?.Invoke();
        startStageEvent?.Invoke();
        gameOverPanel.gameOverUI.SetActive(false);
    }

    public void InvokeGameOver()
    {
        gameOverEvent?.Invoke();
        gameOverPanel.gameOverUI.SetActive(true);
    }

    public void InvokeReturnToHubSelect()
    {
        InvokeHubSelectOn();
        resetFieldEvent?.Invoke();
        gameOverPanel.gameOverUI.SetActive(false);
    }
    public void InvokeStageStart()
    {
        startStageEvent?.Invoke();
    }

    public void InvokeHubSelectOn()
    {
        hubSelectPanel.hubSelectBackground.SetActive(true);
        hubSelectPanel.PostHubSelectInfos(enemySpawnManager.stageLevel);
    }

    public void InvokeStageClear()
    {
        Debug.Log("InvokeStageClear Check");
        resetFieldEvent?.Invoke();
        stageResultPanel.panelBackground.SetActive(true);
        stageResultPanel.PostClearStageInfoText(enemySpawnManager.stageLevel++);
    }
}
