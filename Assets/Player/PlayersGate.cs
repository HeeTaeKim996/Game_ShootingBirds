using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayersGate : EnemyAttackTarget
{
    private float gateHealth = 100;
    public Slider healthSlider;

    private void Start()
    {
        healthSlider.maxValue = gateHealth;
        healthSlider.value = gateHealth;
        healthSlider.minValue = 0;
    }

    public override void GetDamage(float damage)
    {
        gateHealth -= damage;
        healthSlider.value -= damage;
    }
}
