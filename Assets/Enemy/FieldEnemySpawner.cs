using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public abstract class FieldEnemySpawner : MonoBehaviour
{
    private EnemySpawnerManager enemySpawnManager;
    protected StageInform stageInform;
    protected int sumSpawnRatio;
    protected List<List<float>> enemySpawnTimes = new List<List<float>>();
    protected Coroutine instantiateCoroutine;
    protected Coroutine checkWheterFieldClearCoroutine;
    public List<Enemy> onFieldEnemies { get; set; } = new List<Enemy>();
    public event Action checkOnFieldEnemyEvent;

    protected virtual void Awake()
    {
        enemySpawnManager = GetComponentInParent<EnemySpawnerManager>();
        GameManager.instance.gameOverEvent += OnGameOver;
    }
    protected virtual void Start()
    {

    }

    protected virtual void OnGameOver()
    {
        if(checkWheterFieldClearCoroutine != null)
        {
            StopCoroutine(checkWheterFieldClearCoroutine);
            checkWheterFieldClearCoroutine = null;
        }
    }

    public virtual void InvokeSpawnEnemy()
    {
        enemySpawnTimes.Clear();
        for (int i = 0; i < stageInform.enemySpawnInforms.Count; i++)
        {
            List<float> newSpawnTimes = new List<float>();
            for (int j = 0; j < stageInform.enemySpawnInforms[i].spawnCount; j++)
            {
                newSpawnTimes.Add(UnityEngine.Random.Range(0f, stageInform.stageTime));
            }
            newSpawnTimes.Sort();
            enemySpawnTimes.Add(newSpawnTimes);
        }

        if (instantiateCoroutine != null)
        {
            StopCoroutine(instantiateCoroutine);
        }
        instantiateCoroutine = StartCoroutine(InstantiateEnemy());

    }

    protected virtual IEnumerator InstantiateEnemy()
    {
        float elapsedTime = 0f;
        float fixedTime = 0.1f;

        List<int> spawnIndexes = new List<int>();
        for (int i = 0; i < stageInform.enemySpawnInforms.Count; i++)
        {
            spawnIndexes.Add(0);
        }

        while (elapsedTime <= stageInform.stageTime)
        {
            elapsedTime += fixedTime;

            for (int i = 0; i < stageInform.enemySpawnInforms.Count; i++)
            {
                while (spawnIndexes[i] < enemySpawnTimes[i].Count && elapsedTime >= enemySpawnTimes[i][spawnIndexes[i]])
                {
                    SpawnEnemyAtRandomPosition(stageInform.enemySpawnInforms[i].enemyPrefab);
                    spawnIndexes[i]++;
                }
            }


            bool isInstantiatleAll = true;
            for(int i = 0; i < stageInform.enemySpawnInforms.Count; i++)
            {
                if (spawnIndexes[i] < enemySpawnTimes[i].Count)
                {
                    isInstantiatleAll = false;
                }
            }

            if (isInstantiatleAll)
            {
                Debug.Log("FieldEnemySpawner : SpawnAll Check");
                checkWheterFieldClearCoroutine = StartCoroutine(CheckWhetherFieldClear());
                instantiateCoroutine = null;
                yield break;
            }

            yield return new WaitForSeconds(fixedTime);
        }
    }

    protected IEnumerator CheckWhetherFieldClear()
    {
        checkOnFieldEnemyEvent?.Invoke();
        yield return null;

        Debug.Log($"OnFieldEnemies.Count : {onFieldEnemies.Count}");

        float fixedTime = 0.1f;

        while (onFieldEnemies.Count > 0)
        {
            yield return new WaitForSeconds(fixedTime);
        }

        enemySpawnManager.GetClearedSpawnerInfo(this);
        checkWheterFieldClearCoroutine = null;
    }


    public virtual void GetEnemySpawnInforms(StageInform stageInform)
    {
        this.stageInform = stageInform;
    }
    protected abstract void SpawnEnemyAtRandomPosition(Enemy enemyPrefab);
    protected abstract void InstantiateEnemy(Enemy enemyPrefab, Vector3 instantiatePosition, Quaternion instantiateRotation); // 나중에 매서드 InstantiateEnemy 매서드랑 SpawnEnemy매서드(옵젝풀에서생성) 구분 필요
}
