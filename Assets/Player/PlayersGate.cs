using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class PlayersGate : EnemyAttackTarget
{
    private float maxHealth = 100;
    private float gateHealth;
    public Slider healthSlider;
    public Slider easeHealthSlider;
    private Coroutine easeHealthSliderCoroutine;

    protected override void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        gateHealth = maxHealth;
        healthSlider.maxValue = gateHealth;
        healthSlider.minValue = 0;
        healthSlider.value = gateHealth;
        easeHealthSlider.maxValue = gateHealth;
        easeHealthSlider.minValue = 0;
        easeHealthSlider.value = gateHealth;
    }

    public override void GetDamage(float damage)
    {
        gateHealth -= damage;
        healthSlider.value -= damage;
        if(easeHealthSliderCoroutine != null)
        {
            StopCoroutine(easeHealthSliderCoroutine);
        }
        easeHealthSliderCoroutine = StartCoroutine(EaseHealthSliderCoroutine(gateHealth));
        if(gateHealth <= 0)
        {
            GameManager.instance.InvokeGameOver();
        }
    }

    protected override void OnRestartGame()
    {
        base.Awake();
    }
    protected override void OnGameOver()
    {
        base.OnGameOver();
        if(easeHealthSliderCoroutine != null)
        {
            StopCoroutine(easeHealthSliderCoroutine);
            easeHealthSliderCoroutine = null;
        }
    }
    protected override void OnResetField()
    {
        base.OnResetField();
        gateHealth = maxHealth;
        healthSlider.value = gateHealth;
        easeHealthSlider.value = gateHealth;
    }

    private IEnumerator EaseHealthSliderCoroutine(float value)
    {
        while(easeHealthSlider.value > gateHealth)
        {
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, value, 1f * Time.deltaTime);

            yield return null;
        }

        easeHealthSlider.value = value;
        easeHealthSliderCoroutine = null;
    }
}
