using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    private GameManager gameManager;
    public GameObject gameOverUI;
    public CanvasGroup gameOverUICanvasGroup;
    public ButtonOnDownTransfer restartButton;
    public event Action restartGameEvent;
    public event Action gameOverEvent;

    private void Awake()
    {
        gameManager = GetComponentInParent<GameManager>();
        restartButton.OnPointerUpEvent += RestartGame;
        gameOverUICanvasGroup = gameOverUI.GetComponent<CanvasGroup>();
    }
    private void Start()
    {
        gameOverUICanvasGroup.alpha = 1f;
        RestartGame();
    }

    public void RestartGame()
    {
        restartGameEvent?.Invoke();
        gameOverUI.SetActive(false);
    }

    public void GameOverMethod()
    {
        gameOverEvent?.Invoke();
        gameOverUI.SetActive(true);
    }


}
