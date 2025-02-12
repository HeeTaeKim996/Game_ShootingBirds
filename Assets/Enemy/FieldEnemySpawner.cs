using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldEnemySpawner : MonoBehaviour
{

    protected virtual void Awake()
    {
        GameManager.instance.gameOverManager.gameOverEvent += OnGameOver;
        GameManager.instance.gameOverManager.restartGameEvent += OnRestartGame;
    }

    protected virtual void OnGameOver()
    {

    } 

    protected virtual void OnRestartGame()
    {
    }
}
