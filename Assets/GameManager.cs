using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private CameraController cameraController;
    private PlayerShooter playerShooter;


    #region 임시관리
    // 추후 플레이어 로테이션 관리 클래스 만들면 옮기기
    public Button playerRotationSwitchButton;
    private int playerRoationInt = 0;
    private Coroutine playerRotationCoroutine;

    private Vector3 playerDefaultPosition = new Vector3(188.81f, -13.76f, -79.92f);
    private Vector3 playerDefaultRotation = new Vector3(12.798f, 180, 0);

    private Vector3 playerOnWallPosition = new Vector3(188.81f, -13.76f, -83.6f);
    private Vector3 playerOnWallRotation = new Vector3(53.815f, 180, 0);
    #endregion

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
        cameraController = GetComponentInParent<CameraController>();
        playerShooter = FindObjectOfType<PlayerShooter>();

        //playerRotationSwitchButton.onClick.AddListener(OnRotationSwitchButtonClicekd);
    }
    private void Start()
    {
        playerShooter.transform.position = playerDefaultPosition;
        playerShooter.transform.rotation = Quaternion.Euler(playerDefaultRotation);
    }


    #region 추후여기도 옮기기
    public void OnRotationSwitchButtonClicekd()
    {
        if(playerRoationInt == 0)
        {
            if(playerRotationCoroutine != null)
            {
                StopCoroutine(playerRotationCoroutine);
            }
            playerRotationCoroutine = StartCoroutine(ChangePlayerRotation( playerOnWallPosition, playerOnWallRotation));
            playerRoationInt++;
        }
        else if(playerRoationInt == 1)
        {
            if (playerRotationCoroutine != null)
            {
                StopCoroutine(playerRotationCoroutine);
            }
            playerRotationCoroutine = StartCoroutine(ChangePlayerRotation(playerDefaultPosition, playerDefaultRotation));
            playerRoationInt = 0;
        }
    }
    private IEnumerator ChangePlayerRotation(Vector3 position , Vector3 rotation)
    {
        float totalSwitchTime = 0.7f;
        float rotationStartTime = 0.2f;
        float elapsedTime = 0;
        while(elapsedTime < totalSwitchTime)
        {
            elapsedTime += Time.deltaTime;

            playerShooter.transform.position = Vector3.Slerp(playerShooter.transform.position, position, elapsedTime / totalSwitchTime);
            if(elapsedTime >= rotationStartTime)
            {
                playerShooter.transform.rotation = Quaternion.Slerp(playerShooter.transform.rotation, Quaternion.Euler(rotation), (elapsedTime - rotationStartTime)  / (totalSwitchTime - rotationStartTime) );
            }
            yield return null;
        }
        playerRotationCoroutine = null;
    }

    #endregion
}
