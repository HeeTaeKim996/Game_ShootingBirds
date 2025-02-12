using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyWeakInformation
{
    public Transform weakTransform;
    public float weakDistance;    
}

public abstract class Enemy : MonoBehaviour
{
    public abstract void GetAttackTarget(EnemyAttackTarget enemyAttackTarget);
    public abstract void GetDamage(float damage);
    [HideInInspector]
    public List<EnemyWeakInformation> weaknessPivots = new List<EnemyWeakInformation>();
    public Transform LowPivot;
    public Transform highPivot;
    public float arrowSpawnDistance { get; protected set;}

    public void ForTestMethod_CheckHurtColliders(float damageRatio, int layerInt, Transform uniScaleTransform)
    {
        Debug.Log($"{name} - {damageRatio} - {layerInt} - {uniScaleTransform.gameObject.name}");
    }
}
