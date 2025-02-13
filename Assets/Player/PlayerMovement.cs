using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerShooter playerShooter;
    private int playerRoationInt = 0;
    private Coroutine playerRotationCoroutine;

    private Vector3 playerDefaultPosition = new Vector3(188.81f, -13.76f, -79.92f);
    private Vector3 playerDefaultRotation = new Vector3(12.798f, 180, 0);

    private Vector3 playerOnWallPosition = new Vector3(188.81f, -13.76f, -83.6f);
    private Vector3 playerOnWallRotation = new Vector3(53.815f, 180, 0);

    private void Awake()
    {
        playerShooter = GetComponent<PlayerShooter>();
    }

    private void Start()
    {
        transform.position = playerDefaultPosition;
        transform.rotation = Quaternion.Euler(playerDefaultRotation);
    }

    public void OnRotationSwitchButtonClicekd()
    {
        if (playerRoationInt == 0)
        {
            if (playerRotationCoroutine != null)
            {
                StopCoroutine(playerRotationCoroutine);
            }
            playerRotationCoroutine = StartCoroutine(ChangePlayerRotation(playerOnWallPosition, playerOnWallRotation));
            playerRoationInt++;
        }
        else if (playerRoationInt == 1)
        {
            if (playerRotationCoroutine != null)
            {
                StopCoroutine(playerRotationCoroutine);
            }
            playerRotationCoroutine = StartCoroutine(ChangePlayerRotation(playerDefaultPosition, playerDefaultRotation));
            playerRoationInt = 0;
        }
    }
    private IEnumerator ChangePlayerRotation(Vector3 position, Vector3 rotation)
    {
        float totalSwitchTime = 0.7f;
        float rotationStartTime = 0.2f;
        float elapsedTime = 0;
        while (elapsedTime < totalSwitchTime)
        {
            elapsedTime += Time.deltaTime;

            transform.position = Vector3.Slerp(transform.position, position, elapsedTime / totalSwitchTime);
            if (elapsedTime >= rotationStartTime)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(rotation), (elapsedTime - rotationStartTime) / (totalSwitchTime - rotationStartTime));
            }
            yield return null;
        }
        playerRotationCoroutine = null;
    }
}
