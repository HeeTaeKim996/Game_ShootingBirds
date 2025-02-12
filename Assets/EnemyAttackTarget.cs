using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAttackTarget : MonoBehaviour
{
    public abstract void GetDamage(float damage);
}
