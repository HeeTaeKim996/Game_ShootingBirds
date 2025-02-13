
using UnityEngine;

public class PlayerShootController : MonoBehaviour
{
    private PlayerShooter playerShooter;

    private int shooterId = -1;
    private bool didTouchShooter;
    public RectTransform shootableTransform;

    public RectTransform playerRotationSwitchButton;

    private void Awake()
    {
        playerShooter = FindObjectOfType<PlayerShooter>();
    }

    private void Update()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);

            if (shooterId == -1 || shooterId == touch.fingerId)
            {
                if (touch.phase == TouchPhase.Began && WhetherRectContainsTouchPosition(touch.position, shootableTransform))
                {
                    shooterId = touch.fingerId;
                    didTouchShooter = true;
                    playerShooter.GetShooingCharge();
                }

                if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    if (didTouchShooter)
                    {
                        playerShooter.GetShooterVector(touch.position);
                        didTouchShooter = false;
                        shooterId = -1;
                    }
                }
            }


            if (touch.phase == TouchPhase.Began && WhetherRectContainsTouchPosition(touch.position, playerRotationSwitchButton))
            {
                playerShooter.playerMovement.OnRotationSwitchButtonClicekd();
            }
        }
    }

    private bool WhetherRectContainsTouchPosition(Vector2 touchPosition, RectTransform rectTransform)
    {
        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, touchPosition, null, out localPoint))
        {
            return rectTransform.rect.Contains(localPoint);
        }
        return false;
    }
}
