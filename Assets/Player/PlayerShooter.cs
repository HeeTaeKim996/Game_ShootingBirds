using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShooter : MonoBehaviour
{
    public PlayerMovement playerMovement { get; private set; }
    public PlayerArrow BulletPrefab;
    public Slider powerChargeSlider;
    private CanvasGroup sliderCanvasGroup;
    private Coroutine shootingChargeCoroutine;
    public Transform shootTransform;

    private ShooterState shooterState;
    public bool isGameover { get; private set; }

    private const float minShootableSliderValue = 40f;
    private bool isEarlyShooted;
    private Vector3 reservedOrigin;
    private Vector3 reservedDirection;

    private Coroutine reloadingCoroutine;
    private Coroutine reloadReserveCoroutine;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        sliderCanvasGroup = powerChargeSlider.GetComponent<CanvasGroup>();
        GameManager.instance.gameOverManager.restartGameEvent += OnRestartGame;
        GameManager.instance.gameOverManager.gameOverEvent += OnGameOver;
    }

    private void Start()
    {
        powerChargeSlider.maxValue = 100;
        powerChargeSlider.minValue = 0;
        powerChargeSlider.value = 0;
        sliderCanvasGroup.alpha = 0;
        shooterState = ShooterState.Normal;
    }

    public void GetShooterVector(Vector2 touchPosition)
    {
        if (!isGameover)
        {
            Ray ray = Camera.main.ScreenPointToRay(touchPosition);
            #region ��������
            /* ������ ���� ����� �۽��ÿ����� Ray ��, Ray ray = new Ray( (Vector3)rayStartPosition, (Vector3)rayDirection)); ���� Ray�� �����µ�,
             * �������� �ٸ� ������� Ray �� ����Ǵµ�, ��ó�� Camera.main.ScreePointToRay( (Vector2)touchPosition); �� �����ϸ�, ray.origin = (��Ŭ���������� ������Ʈ�� �ƴ�) ī�޶��� ��ġ, ray.direction �� ray.origin�� ���ؿ���, touchPosition�� �������� ���ư��� Vector3 �� �ȴ�.
             * �̴� ī�޶� Orthographic �� �ƴ� Perspective�� ��, ī�޶��� ���� ������ ������� ���� ��. ���⼭ �ҽ���? �� ȭ���� �߾��̹Ƿ�, ray.origin�� ȭ���� �߾��̴�.
             * 
             * �� ���� ����ü�� �߻��ϴ� ���� TPS����ó�� ȭ���� �߾��� �ƴ� �÷��̾��� �ѱ� �����̶��, Ray ray �� ��ݸ����δ� �߻�ü�� ��������, Vector3 Direction�� ���� �� ����. ���� �� ������ Ray ray �� ���پ�, 
             * RaycastHit hit; 
             * if(Physics.RayCast(ray, out hit, Mathf.Infinity))
             * { out ��µ� hit �� �������, �߻�ü�� ��������, Direction�� ��� 
             * }
             * else
             * { ���� Ray ray �� rayDirection�� ������� Direction���� ����ؼ�, �߻�ü ���������� �߻�. (�̷��� �ϸ�, �÷��̾ ������ �����ͷ� �߻�Ǵ°� �ƴ϶�, ray.origin�� �ѱ��߻����� ���� ��ŭ, ������ �ִ� ������ �߻�ü�� ����)
             * }
             * �̷��� ó���� �� ���� �Ͱ���. �̷��� �ϸ� Physics.RayCast ���� ray�� ���� ���� ��ġ������ ������� ������� ������ ��Ȯ�� �߻�ü�� �� �������� ���� �� ������, ������ �� ������ �ƴ� �ٸ� ���� ���� �ִٸ�,
             * ���� ray.origin���� �߻�������� ������ �ִٸ�, ���� ray ��ġ�������� �����ٸ�, �߻�ü�� �÷��̾ ���� ���� ���� �������� ���ϰ�, ���� ray ��ġ�������� �ָ� �ִٸ�, �߻�ü�� �÷��̾ ���� ���� ���� ������ ���Ѵ�.
             * ����, �ﰢ�Լ� ����ó�� �۵��ϴ� Physics.RasCast(ray, out hit,...) �� �߻�ü�� �������� ray.origin�� �ƴ� ����, Physics.RayCast�� �ִ� ��ġ������ ���� �÷��̾ ��ǥ�� ��� ���� ��ġ������ ������ ������ ��Ȯ�ϰ� �� �������� �߻�ü�� �߻��� �� ������,
             * �÷��̾ ��ǥ�� �ϴ� ����, Physics.RayCast�� �ִ� ����� ��ġ������ �ٸ� �ÿ�, �߻�ü�� ������ ������ ���ư� ��.
             * 
             * ���� �� ����� ������� TPS ���� ������ ����ٸ�, �� ������ �����ϴ� ��Ȳ( �ַ� �߻�ü�� �߷��� ������ ���� �ʴ� ��Ȳ) �̶��, ũ�� ������ ���� ���� ������,
             * ���� �߻�ü�� �߷��� ������ �޴� ���� �帣���, RayCast�� �ִ� ��ġ������ �÷��̾ ��ǥ�� �ϴ� ����� ��ġ ������ ��ġ���� �ʴ� ��찡 ���� ���̱� ������, �÷��̾ �ǵ��� �������� �߻�ü�� ������ ���� ���ɼ��� ũ��.
             * ����, �̹� ���ӿ�����, Physics.Raycast�� ������� �ʰ�, Ray ray�� ray.origin�� ray.direction�� ������θ� �߻�ü�� �߻��ؾ� �Ѵ�.
             * 
             * �� �߰���, ī�޶��� �ҽ���? �� ȭ�� �߾��� �ƴ�, �÷��̾��� �߻�ü�� �ѱ� Vector2 �� Ŀ���͸���¡ �Ѵٸ�, ray.origin�� ȭ�� �߾��� �ƴ� �÷��̾��� �ѱ� �������� �����Ѵٸ�, ray.origin �� ray.direction�� ������� �ѱ��� ��鼭��, �߻�ü�� �������� ȭ�� �߾��� �ƴ�
             *    �÷��̾��� �ѱ� �� ���� �� ���� ����� �����غ���. ���� �̷��ٸ� ����Ƽ ������ ���� �պ��ߵǱ� ������ ��������ε� ���� �����, ���� ȭ���� �ҽ���? �� ȭ�� �߾��� �ƴ� �ѱ� �����̶��, �÷��̾ ���������� ���� �� ���� ������ ���Ǽ��� ���� �������� ����
             * 
             */
            #endregion
            if (shooterState == ShooterState.Normal)
            {
                if (powerChargeSlider.value >= minShootableSliderValue)
                {
                    Shoot(ray.origin, ray.direction);
                    if (shootingChargeCoroutine != null)
                    {
                        StopCoroutine(shootingChargeCoroutine);
                        shootingChargeCoroutine = null;
                    }
                }
                else
                {
                    isEarlyShooted = true;
                    reservedOrigin = ray.origin;
                    reservedDirection = ray.direction;
                }
            }
            else if (shooterState == ShooterState.Reloading)
            {
                if (reloadReserveCoroutine != null)
                {
                    StopCoroutine(reloadReserveCoroutine);
                    reloadReserveCoroutine = null;
                }
            }
        }
    }

    private void Shoot(Vector3 origin, Vector3 direction)
    {
        PlayerArrow bullet = Instantiate(BulletPrefab, origin, Quaternion.LookRotation(direction));
        Destroy(bullet, 5f);
        bullet.GetDirectionAndPower(direction, (float)(powerChargeSlider.value / 23f));

        powerChargeSlider.value = 0;
        sliderCanvasGroup.alpha = 0;
        shooterState = ShooterState.Reloading;
        if (reloadingCoroutine != null)
        {
            StopCoroutine(reloadingCoroutine);
        }
        reloadingCoroutine = StartCoroutine(ReloadingCoroutine());
    }
    private IEnumerator ReloadingCoroutine()
    {
        yield return new WaitForSeconds(0.7f);
        shooterState = ShooterState.Normal;
        reloadingCoroutine = null;
    }

    public void GetShooingCharge()
    {
        if (!isGameover)
        {
            if (shooterState == ShooterState.Normal)
            {
                if (shootingChargeCoroutine != null)
                {
                    StopCoroutine(shootingChargeCoroutine);
                }
                shootingChargeCoroutine = StartCoroutine(ShootingCharge());
            }
            else if (shooterState == ShooterState.Reloading)
            {
                if (reloadReserveCoroutine != null)
                {
                    StopCoroutine(reloadReserveCoroutine);
                }
                reloadReserveCoroutine = StartCoroutine(ReloadReserveCoroutine());
            }
        }
    }
    private IEnumerator ReloadReserveCoroutine()
    {
        while (true)
        {
            if(shooterState == ShooterState.Normal)
            {
                break;
            }
            yield return null;
        }
        if(shootingChargeCoroutine != null)
        {
            StopCoroutine(shootingChargeCoroutine);
        }
        shootingChargeCoroutine = StartCoroutine(ShootingCharge());
        reloadReserveCoroutine = null;
    }
    private IEnumerator ShootingCharge()
    {
        isEarlyShooted = false;
        powerChargeSlider.value = 0;
        sliderCanvasGroup.alpha = 0.5f;
        while (true)
        {
            powerChargeSlider.value += Time.deltaTime * 100;

            if (isEarlyShooted && powerChargeSlider.value >= minShootableSliderValue)
            {
                break;
            }
            yield return null;
        }
        Shoot(reservedOrigin, reservedDirection);
        shootingChargeCoroutine = null;
    }

    public void OnRestartGame()
    {
        isGameover = false;
    }
    public void OnGameOver()
    {
        isGameover = true;
    }


}