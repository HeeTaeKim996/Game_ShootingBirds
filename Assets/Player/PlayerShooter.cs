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
            #region 공부정리
            /* 예전에 내가 사용한 멍슈팅에서의 Ray 는, Ray ray = new Ray( (Vector3)rayStartPosition, (Vector3)rayDirection)); 으로 Ray를 쐈었는데,
             * 위에서는 다른 방법으로 Ray 가 실행되는데, 위처럼 Camera.main.ScreePointToRay( (Vector2)touchPosition); 을 실행하면, ray.origin = (이클래스를담은 오브젝트가 아닌) 카메라의 위치, ray.direction 은 ray.origin을 기준에서, touchPosition을 방향으로 나아가는 Vector3 가 된다.
             * 이는 카메라가 Orthographic 이 아닌 Perspective일 때, 카메라의 원근 투영을 기반으로 계산된 것. 여기서 소실점? 은 화면의 중앙이므로, ray.origin은 화면의 중앙이다.
             * 
             * □ 만약 투사체를 발사하는 곳이 TPS게임처럼 화면의 중앙이 아닌 플레이어의 총구 방향이라면, Ray ray 를 기반만으로는 발사체의 시작점과, Vector3 Direction을 구할 수 없다. 따라서 이 때에는 Ray ray 와 더붙어, 
             * RaycastHit hit; 
             * if(Physics.RayCast(ray, out hit, Mathf.Infinity))
             * { out 출력된 hit 을 기반으로, 발사체의 시작점과, Direction을 계산 
             * }
             * else
             * { 기존 Ray ray 의 rayDirection을 기반으로 Direction으로 사용해서, 발사체 시작점에서 발사. (이렇게 하면, 플레이어가 지정한 포인터로 발사되는게 아니라, ray.origin과 총구발사점의 차이 만큼, 오차가 있는 곳으로 발사체가 나감)
             * }
             * 이렇게 처리할 수 있을 것같음. 이렇게 하면 Physics.RayCast 에서 ray가 접한 곳의 위치정보를 기반으로 만들어진 곳에는 정확히 발사체가 그 지점으로 나갈 수 있지만, 문제는 그 지점이 아닌 다른 곳에 적이 있다면,
             * 만약 ray.origin기준 발사시작점이 우측에 있다면, 적이 ray 위치정보보다 가깝다면, 발사체는 플레이어가 보는 기준 적의 우측으로 향하고, 적이 ray 위치정보보다 멀리 있다면, 발사체는 플레이어가 보는 기준 적의 왼쪽을 향한다.
             * 따라서, 삼각함수 모형처럼 작동하는 Physics.RasCast(ray, out hit,...) 와 발사체의 시작점이 ray.origin이 아닌 모델은, Physics.RayCast가 주는 위치정보와 실제 플레이어가 목표로 쏘는 적의 위치정보가 동일할 때에는 정확하게 그 방향으로 발사체를 발사할 수 있지만,
             * 플레이어가 목표로 하는 대상과, Physics.RayCast가 주는 대상의 위치정보가 다를 시에, 발사체는 엉뚱한 곳으로 나아갈 것.
             * 
             * 따라서 위 계산을 기반으로 TPS 슈팅 게임을 만든다면, 위 조건을 충족하는 상황( 주로 발사체가 중력의 영향을 받지 않는 상황) 이라면, 크게 문제가 되지 않을 테지만,
             * 만약 발사체가 중력의 영향을 받는 슈팅 장르라면, RayCast가 주는 위치정보와 플레이어가 목표로 하는 대상의 위치 정보가 일치하지 않는 경우가 흔할 것이기 때문에, 플레이어가 의도한 방향으로 발사체가 나가지 않을 가능성이 크다.
             * 따라서, 이번 게임에서는, Physics.Raycast를 사용하지 않고, Ray ray의 ray.origin과 ray.direction을 기반으로만 발사체를 발사해야 한다.
             * 
             * ※ 추가로, 카메라의 소실점? 을 화면 중앙이 아닌, 플레이어의 발사체의 총구 Vector2 로 커스터마이징 한다면, ray.origin도 화면 중앙이 아닌 플레이어의 총구 방향으로 변경한다면, ray.origin 과 ray.direction을 기반으로 총구를 쏘면서도, 발사체의 시작점이 화면 중앙이 아닌
             *    플레이어의 총구 로 만들 수 있지 않을까도 생각해봤음. 물론 이렇다면 유니티 엔진을 직접 손봐야되기 때문에 기술적으로도 많이 힘들고, 또한 화면의 소실점? 이 화면 중앙이 아니 총구 방향이라면, 플레이어가 어지러움을 느낄 것 같기 때문에 현실성은 없는 내용으로 보임
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