using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FieldEnemySpawner_South : FieldEnemySpawner
{
    private GameManager gameManager;
    public Enemy enemyPrefab;

    public Transform spawnPosition_main_1;
    public Transform spawnPosition_main_2;
    public Transform spawnPosition_Right_1;
    public Transform spawnPosition_Right_2;
    public Transform spawnPosition_West_1;
    public Transform spawnPosition_West_2;

    public EnemyAttackTarget enemyAttackTarget;

    private Coroutine instantiateCoroutine;

    

    protected override void Awake()
    {
        base.Awake();
        gameManager = GetComponentInParent<GameManager>();
    }
    private void Start()
    {
    }
    protected override void OnGameOver()
    {
        base.OnGameOver();
        if (instantiateCoroutine != null)
        {
            StopCoroutine(instantiateCoroutine);
            instantiateCoroutine = null;
        }
    }
    protected override void OnRestartGame()
    {
        base.OnRestartGame();
        if (instantiateCoroutine != null)
        {
            StopCoroutine(instantiateCoroutine);
        }
        instantiateCoroutine = StartCoroutine(InstantiateEnemy());
    }

    private IEnumerator InstantiateEnemy()
    {
        float spawnRestTime = 0;
        float fixedTime = 0.3f;

        while (true)
        {
            if(spawnRestTime <= 0)
            {
                int randomInt = Random.Range(1, 11);
                if(randomInt <= 6)
                {
                    Vector3 direction = spawnPosition_main_2.position - spawnPosition_main_1.position;
                    float distance = Vector3.Distance(spawnPosition_main_2.position, spawnPosition_main_1.position);
                    Vector3 spawnPosition = spawnPosition_main_1.position + direction.normalized * Random.Range(0f, distance);

                    Enemy duck = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                    duck.GetAttackTarget(enemyAttackTarget);
                }
                else if(randomInt >= 7 && randomInt < 9)
                {
                    Vector3 direction = spawnPosition_Right_2.position - spawnPosition_Right_1.position;
                    float distance = Vector3.Distance(spawnPosition_Right_1.position, spawnPosition_Right_2.position);
                    Vector3 spawnPosition = spawnPosition_Right_1.position + direction.normalized * Random.Range(0, distance);

                    Enemy enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                    enemy.GetAttackTarget(enemyAttackTarget);
                }
                else
                {

                    Vector3 direction = spawnPosition_West_2.position - spawnPosition_West_1.position;
                    float distance = Vector3.Distance(spawnPosition_West_1.position, spawnPosition_West_2.position);
                    Vector3 spawnPosition = spawnPosition_West_1.position + direction.normalized * Random.Range(0, distance);

                    Enemy enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                    enemy.GetAttackTarget(enemyAttackTarget);
                }
                spawnRestTime = Random.Range(2.2f, 6.5f);
            }
            spawnRestTime -= fixedTime;

            yield return new WaitForSeconds(fixedTime);
        }
    }

}
