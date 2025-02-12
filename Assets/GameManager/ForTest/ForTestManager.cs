using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForTestManager : MonoBehaviour
{
    public static ForTestManager instance;

    [SerializeField]
    private SliderUpTransferValue timeScaleSlider;
    private int timeScaleSliderInt = 0;

    [SerializeField]
    private Text debugLiveText;
    private int debugLiveTextInt = 0;

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
        timeScaleSlider.gameObject.SetActive(false);
        timeScaleSlider.OnPointerUpEvent += TimeScaleValueChange;
    }

    public void OnOffTimeScaleSlider()
    {
        Debug.Log(timeScaleSliderInt);
        if(timeScaleSliderInt == 0)
        {
            Debug.Log("0 -> 1Check");
            timeScaleSlider.gameObject.SetActive(true);
            timeScaleSliderInt++;
        }
        else if(timeScaleSliderInt == 1)
        {
            timeScaleSlider.gameObject.SetActive(false);
            timeScaleSliderInt = 0;
            Debug.Log("Check");
        }
    }

    public void TimeScaleValueChange(float newValue)
    {
        Time.timeScale = newValue;
    }
    public void OnOffDebugtext()
    {
        if(debugLiveTextInt == 0)
        {
            debugLiveText.gameObject.SetActive(false);
            debugLiveTextInt++;
        }
        else if(debugLiveTextInt == 1)
        {
            debugLiveText.gameObject.SetActive(true);
            debugLiveTextInt = 0;
        }
    }
}
