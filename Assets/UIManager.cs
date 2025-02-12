using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public Text timeText;
    private float elapsedTime = 0;
    private Coroutine elapsedTimeCheckCoroutine;

    private int score = 0;
    public Text scoreText;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if(elapsedTimeCheckCoroutine != null)
        {
            StopCoroutine(elapsedTimeCheckCoroutine);
        }
        elapsedTimeCheckCoroutine = StartCoroutine(ElapsedTimeCheckCoroutine());
    }
    private IEnumerator ElapsedTimeCheckCoroutine()
    {
        while (true)
        {
            elapsedTime += 0.5f;
            timeText.text = $"경과시간 : {(int)elapsedTime}";
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void GetScoreChanged(int score)
    {
        this.score += score;
        scoreText.text = $"점수 : {this.score}";
    }

}
