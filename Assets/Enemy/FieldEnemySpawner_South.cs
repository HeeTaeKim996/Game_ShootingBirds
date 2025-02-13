using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FieldEnemySpawner_South : FieldEnemySpawner
{

    public Transform spawnPosition_main_1;
    public Transform spawnPosition_main_2;
    public Transform spawnPosition_Right_1;
    public Transform spawnPosition_Right_2;
    public Transform spawnPosition_West_1;
    public Transform spawnPosition_West_2;

    public EnemyAttackTarget enemyAttackTarget;


    

    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
    }

    protected override void OnGameOver()
    {
        base.OnGameOver();

    }
    public override void InvokeSpawnEnemy()
    {
        base.InvokeSpawnEnemy();
    }

    protected override void SpawnEnemyAtRandomPosition(Enemy spawnEnemy)
    {
        int randomInt = Random.Range(1, 11);
        if (randomInt <= 6)
        {
            Vector3 direction = spawnPosition_main_2.position - spawnPosition_main_1.position;
            float distance = Vector3.Distance(spawnPosition_main_2.position, spawnPosition_main_1.position);
            Vector3 spawnPosition = spawnPosition_main_1.position + direction.normalized * Random.Range(0f, distance);

            InstantiateEnemy(spawnEnemy, spawnPosition, Quaternion.identity);

        }
        else if (randomInt >= 7 && randomInt < 9)
        {
            Vector3 direction = spawnPosition_Right_2.position - spawnPosition_Right_1.position;
            float distance = Vector3.Distance(spawnPosition_Right_1.position, spawnPosition_Right_2.position);
            Vector3 spawnPosition = spawnPosition_Right_1.position + direction.normalized * Random.Range(0, distance);

            InstantiateEnemy(spawnEnemy, spawnPosition, Quaternion.identity);
        }
        else
        {

            Vector3 direction = spawnPosition_West_2.position - spawnPosition_West_1.position;
            float distance = Vector3.Distance(spawnPosition_West_1.position, spawnPosition_West_2.position);
            Vector3 spawnPosition = spawnPosition_West_1.position + direction.normalized * Random.Range(0, distance);

            InstantiateEnemy(spawnEnemy, spawnPosition, Quaternion.identity);
        }
    }

    protected override void InstantiateEnemy(Enemy enemyPrefab, Vector3 instantiatePosition, Quaternion instantiateRotation)
    {
        Enemy enemy = Instantiate(enemyPrefab, instantiatePosition, instantiateRotation);
        enemy.GetAttackTarget(enemyAttackTarget);
        enemy.GetSpawnerInfo(this);
    }

}
