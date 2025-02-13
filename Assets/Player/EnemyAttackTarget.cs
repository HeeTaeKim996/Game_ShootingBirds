using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAttackTarget : MonoBehaviour
{
    public abstract void GetDamage(float damage);

    protected virtual void Awake()
    {

        GameManager.instance.startStageEvent += OnRestartGame;
        GameManager.instance.gameOverEvent += OnGameOver;
        GameManager.instance.resetFieldEvent += OnResetField;
    }


    protected virtual void OnRestartGame()
    {

    }

    protected virtual void OnGameOver()
    {
        
    }
    protected virtual void OnResetField()
    {

    }
}
