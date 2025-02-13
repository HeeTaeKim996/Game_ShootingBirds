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
        Debug.Log("버튼닫기 디버그 확인. 추후 업데이트 예정");
    }
    public void OnStageStartButtonClicked()
    {
        GameManager.instance.InvokeStageStart();
        hubSelectBackground.SetActive(false);
    }
    public void OnStageUpButtonClicked()
    {
        Debug.Log("온스테이지업 클릭 확인. 추후 업데이트 예정");
    }
    public void OnStageDownButtonClicked()
    {
        Debug.Log("온스테이지다운 클릭 확인. 추후 업데이트 예정");
    }
    public void PostHubSelectInfos(int stageInt)
    {
        stageInformText.text = CommonMethods.StageLevelToString(stageInt);
    }

}
