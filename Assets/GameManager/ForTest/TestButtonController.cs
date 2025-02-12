using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestButtonController : MonoBehaviour
{
    public GameObject testButtonsObject;
    public List<RectTransform> testButtons = new List<RectTransform>();

    private int testFingerId = -1;
    private bool didTouchTestButton = false;

    private void Start()
    {
        testButtonsObject.gameObject.SetActive(false);
    }

    private void Update()
    {
        for(int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);

            if(testFingerId == -1 || testFingerId == touch.fingerId)
            {
                if(touch.phase == TouchPhase.Began && IsTouchWithinRect(touch.position, testButtons[0]))
                {
                    testFingerId = -1;
                    didTouchTestButton = true;
                    testButtonsObject.gameObject.SetActive(true);
                }

                else if( (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) && didTouchTestButton)
                {
                    if(IsTouchWithinRect(touch.position, testButtons[0]))
                    {
                        ForTestManager.instance.OnOffTimeScaleSlider();
                    }
                    else if(IsTouchWithinRect(touch.position, testButtons[1]))
                    {
                        ForTestManager.instance.OnOffDebugtext();
                    }
                    else if(IsTouchWithinRect(touch.position, testButtons[2]))
                    {
                        Debug.Log("TestButton 2 Check");
                    }

                    testFingerId = -1;
                    didTouchTestButton = false;
                    testButtonsObject.gameObject.SetActive(false);
                }
            }

        }
    }

    private bool IsTouchWithinRect(Vector2 touchPosition, RectTransform rectTransform)
    {
        Vector2 localPoint;
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, touchPosition, null, out localPoint))
        {
            return rectTransform.rect.Contains(localPoint);
        }
        return false;
    }

}
