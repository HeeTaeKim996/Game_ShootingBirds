using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnInform
{
    public Enemy enemyPrefab;
    public int spawnCount;
}

public class SpecialEnemyInform
{

}
public class StageInform 
{
    public List<EnemySpawnInform> enemySpawnInforms;
    public List<SpecialEnemyInform> specialEnemyInforms;
    public float stageTime;
}

