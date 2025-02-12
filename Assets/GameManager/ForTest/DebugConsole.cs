using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugConsole : MonoBehaviour
{
    public Text liveText;
    private List<string> liveStrings = new List<string>();

    private void Start()
    {
        Application.logMessageReceived += HandleLog;
    }

    public void HandleLog(string logString, string stackTrace, LogType type)
    {
        liveStrings.Add(logString);

        StartCoroutine(RemoveStringAfterTime(logString));
        if(liveStrings.Count > 10)
        {
            liveStrings.RemoveAt(0);
        }
        if (liveText.gameObject.activeSelf)
        {
            liveText.text = string.Join("\n", liveStrings.ToArray());
        }
    }

    private IEnumerator RemoveStringAfterTime(string newString)
    {
        yield return new WaitForSeconds(5f);

        if (liveStrings.Contains(newString))
        {
            liveStrings.Remove(newString);

            if (liveText.gameObject.activeSelf)
            {
                liveText.text = string.Join("\n", liveStrings.ToArray());
            }
        }
    }
}
