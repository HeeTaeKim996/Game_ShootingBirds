using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HubSelectPanel : MonoBehaviour
{
    public GameObject hubSelectBackground;
    private CanvasGroup backGroundCanvasGroup;
    public ButtonOnDownTransfer closeButton;
    public ButtonOnDownTransfer stageStartButton;
    public ButtonOnDownTransfer stageUpButton;
    public ButtonOnDownTransfer stageDownButton;
    public Text stageInformText;

    private void Awake()
    {
        backGroundCanvasGroup = hubSelectBackground.GetComponent<CanvasGroup>();
        backGroundCanvasGroup.alpha = 1;

        closeButton.OnPointerUpEvent += OnCloseButtonClicked;
        stageStartButton.OnPointerUpEvent += OnStageStartButtonClicked;
        stageUpButton.OnPointerUpEvent += OnStageUpButtonClicked;
        stageDownButton.OnPointerUpEvent += OnStageDownButtonClicked;
    }
    public void OnCloseButtonClicked()
    {
        Debug.Log("��ư�ݱ� ����� Ȯ��. ���� ������Ʈ ����");
    }
    public void OnStageStartButtonClicked()
    {
        GameManager.instance.InvokeStageStart();
        hubSelectBackground.SetActive(false);
    }
    public void OnStageUpButtonClicked()
    {
        Debug.Log("�½��������� Ŭ�� Ȯ��. ���� ������Ʈ ����");
    }
    public void OnStageDownButtonClicked()
    {
        Debug.Log("�½��������ٿ� Ŭ�� Ȯ��. ���� ������Ʈ ����");
    }
    public void PostHubSelectInfos(int stageInt)
    {
        stageInformText.text = CommonMethods.StageLevelToString(stageInt);
    }

}
