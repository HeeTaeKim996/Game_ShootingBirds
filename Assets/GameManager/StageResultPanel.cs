using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageResultPanel : MonoBehaviour
{
    public GameObject panelBackground;
    private CanvasGroup stageResultPanelBackgroundCanvasGroup;
    public ButtonOnDownTransfer hubSelectPanelButton;
    public Text clearStageInfoText;

    private void Awake()
    {
        hubSelectPanelButton.OnPointerUpEvent += OnHubSelctPanelButton;
        stageResultPanelBackgroundCanvasGroup = panelBackground.GetComponent<CanvasGroup>();
        stageResultPanelBackgroundCanvasGroup.alpha = 1;
    }
    private void Start()
    {
        panelBackground.SetActive(false);
    }


    private void OnHubSelctPanelButton()
    {
        GameManager.instance.InvokeHubSelectOn();
        panelBackground.SetActive(false);
    }
    public void PostClearStageInfoText(int stageInt)
    {
        clearStageInfoText.text = $" Clear Stage : {CommonMethods.StageLevelToString(stageInt)}";
    }
}
