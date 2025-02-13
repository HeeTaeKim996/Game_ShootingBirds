using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class EnemySpawnerManager : MonoBehaviour
{
    private FieldEnemySpawner_South enemySpawner_South;

    public Enemy firstEnemy;

    public int stageLevel { get; set; } = 1_001;

    private void Awake()
    {
        enemySpawner_South = GetComponentInChildren<FieldEnemySpawner_South>();
        GameManager.instance.startStageEvent += OnStageStart;
    }

    public void OnStageStart()
    {
        List<EnemySpawnInform> enemySpawnInforms = new List<EnemySpawnInform>();
        enemySpawnInforms.Add(new EnemySpawnInform { enemyPrefab = firstEnemy, spawnCount = 14 + (int) (stageLevel - ( (int)(stageLevel / 1000) * 1000) ) });
        StageInform stageInform = new StageInform { enemySpawnInforms = enemySpawnInforms, stageTime = 50f };
        Debug.Log($"Spawn Count : {stageInform.enemySpawnInforms[0].spawnCount}");

        enemySpawner_South.GetEnemySpawnInforms(stageInform);
        enemySpawner_South.InvokeSpawnEnemy();
    }

    public void GetClearedSpawnerInfo(FieldEnemySpawner fieldEnemySpawner)
    {
        if(fieldEnemySpawner == enemySpawner_South)
        {
            GameManager.instance.InvokeStageClear(); // 추후 4방향 모두 Clear 때 InvokeClear 처리
        }
    }

}
