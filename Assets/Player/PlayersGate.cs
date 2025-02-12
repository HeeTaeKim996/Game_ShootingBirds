using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayersGate : EnemyAttackTarget
{
    private float maxHealth = 100;
    private float gateHealth;
    public Slider healthSlider;
    public Slider easeHealthSlider;
    private Coroutine easeHeatlhSliderCoroutine;

    private void Awake()
    {
        GameManager.instance.gameOverManager.restartGameEvent += OnRestartGame;
    }
    private void Start()
    {

    }

    public override void GetDamage(float damage)
    {
        gateHealth -= damage;
        healthSlider.value -= damage;
        if(easeHeatlhSliderCoroutine != null)
        {
            StopCoroutine(easeHeatlhSliderCoroutine);
        }
        easeHeatlhSliderCoroutine = StartCoroutine(EaseHealthSliderCoroutine(gateHealth));
        if(gateHealth <= 0)
        {
            GameManager.instance.gameOverManager.GameOverMethod();
        }
    }

    public void OnRestartGame()
    {
        gateHealth = maxHealth;
        healthSlider.maxValue = gateHealth;
        healthSlider.value = gateHealth;
        healthSlider.minValue = 0;
        easeHealthSlider.maxValue = gateHealth;
        easeHealthSlider.value = gateHealth;
        easeHealthSlider.minValue = 0;
    }

    private IEnumerator EaseHealthSliderCoroutine(float value)
    {
        while(easeHealthSlider.value > gateHealth)
        {
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, value, 1f * Time.deltaTime);

            yield return null;
        }

        easeHealthSlider.value = value;
        easeHeatlhSliderCoroutine = null;
    }
}
