using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPanel : MonoBehaviour
{
    public GameObject gameOverUI;
    private CanvasGroup gameOverUICanvasGroup;
    public ButtonOnDownTransfer restartButton;
    public ButtonOnDownTransfer returnToHubSelectButton;

    private void Awake()
    {
        restartButton.OnPointerUpEvent += OnRestartButtonClicked;
        gameOverUICanvasGroup = gameOverUI.GetComponent<CanvasGroup>();
        returnToHubSelectButton.OnPointerUpEvent += OnReturnToHubSelectButtonClicked;
    }
    private void Start()
    {
        gameOverUICanvasGroup.alpha = 1f;
        gameOverUI.SetActive(false);
    }

    public void OnRestartButtonClicked()
    {
        GameManager.instance.RestartGame();
        Debug.Log("OnRestartButtonClicked");
    }

    public void OnReturnToHubSelectButtonClicked()
    {
        GameManager.instance.InvokeReturnToHubSelect();
        Debug.Log("OnReturnTOHubSelectButtonClicked");
    }


}
