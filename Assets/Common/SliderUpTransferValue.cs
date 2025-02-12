using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class SliderUpTransferValue : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public event Action<float> OnPointerUpEvent;
    private Slider slider;

    [SerializeField]
    private bool doesResetValue;
    [SerializeField]
    private float resetValue;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }
    private void Start()
    {
        slider.minValue = 0f;
        slider.maxValue = 1f;
        slider.value = resetValue;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnPointerUpEvent?.Invoke(slider.value);
        if (doesResetValue)
        {
            slider.value = (float)resetValue;
        }
    }


}
