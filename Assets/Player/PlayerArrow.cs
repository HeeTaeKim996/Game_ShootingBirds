using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class PlayerArrow : MonoBehaviour
{
    private Rigidbody arrowRigidbody;
    private LayerMask hurtBoxLayer;
    private LayerMask terrainLayer;
    private Collider collider1;
    private float damage = 10f;
    private Coroutine rotationCoroutine;

    Quaternion currentRotation;
    Quaternion oneFrameLateRotation;

    private bool isAttackable;

    private void Awake()
    {
        arrowRigidbody = GetComponent<Rigidbody>();
        hurtBoxLayer = LayerMask.GetMask("HurtBox");
        terrainLayer = LayerMask.GetMask("Terrain");
        collider1 = GetComponent<Collider>();
    }
    private void Start()
    {
        collider1.material = StaticValues.nonFrictionMaterial;
        Destroy(gameObject, 7f);
    }
    private void OnEnable()
    {
        if (rotationCoroutine == null)
        {
            rotationCoroutine = StartCoroutine(UpdateRotation());
        }
        arrowRigidbody.isKinematic = false;
        collider1.enabled = true;
        isAttackable = true;
    }
    private void Update()
    {
        oneFrameLateRotation = currentRotation;
        currentRotation = transform.rotation;
    }

    public void GetDirectionAndPower(Vector3 direction, float power)
    {
        arrowRigidbody.AddForce(direction * power, ForceMode.Impulse);
    }

    private IEnumerator UpdateRotation()
    {
        yield return new WaitForFixedUpdate();
        while (true)
        {
            arrowRigidbody.rotation = Quaternion.LookRotation(arrowRigidbody.velocity.normalized);
            yield return new WaitForFixedUpdate();
        }
    }


    private void OnCollisionEnter(Collision collision)
    {

        if (((1 << collision.gameObject.layer) & terrainLayer) != 0)
        {
            isAttackable = false;
            SetDormantState();
            transform.rotation = oneFrameLateRotation;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (isAttackable)
        {
            if (((1 << other.gameObject.layer) & hurtBoxLayer) != 0)
            {
                isAttackable = false;


                Enemy_HurtBox hurtBox = other.gameObject.GetComponent<Enemy_HurtBox>();

                hurtBox.GetDamage(damage);

                SetDormantState();

                transform.SetParent(hurtBox.uniScaleTransform, true);

                #region 공부정리
                /*
                ■ SetParent에서 Non-Uni-Scaling 문제 
                - transform.SetParent(A,true)에서 A 또는 A의 부모들 옵젝중 하나의 Scale의 x,y,z값이 서로 다른 경우, SetParent의 (2) 가 true라 해도, 자식으로 설정되는 오브젝트의 로테이션이 외곡된다.
                  자식으로 설정될 때, 포지션, 로테이션, 스케일이 재정렬되는데, 이 때 부모 대상 오브젝트의 스케일이 Uni-Scale을 기준으로 매서드가 만들어진 듯 하다. 
                  지피티의 도움으로 시도1, 시도2 도 해봤지만, 해결이 안됐음. 
                  물론 FBX 모델링으로 파일을 다운받아서 만들기 때문에, 대부분의 오브젝트는 Uni Scaling이기 때문에 이런 경우가 흔하지 않지만, 허트박스 등 콜라이더가 논 유니 스케일링일 수 있다.
                  

                밑은 시도1, 시도2로 해결은 못했지만 참고용으로 남겨둠
                
                □ 시도1
                Matrix4x4 parentMatrix = Matrix4x4.TRS(Vector3.zero, hurtBox.transform.rotation, hurtBox.transform.lossyScale);
                Matrix4x4 inverseParentMatrix = parentMatrix.inverse;
                Matrix4x4 childMatrix = Matrix4x4.TRS(oneFrameLatePosition, oneFrameLateRotation, transform.lossyScale);
                Matrix4x4 localMatrix = inverseParentMatrix * childMatrix;
                transform.localRotation = localMatrix.rotation;
                transform.localPosition = other.transform.InverseTransformPoint(oneFrameLatePosition);

                □ 시도2
                Vector3 forward = hurtBox.transform.InverseTransformDirection(oneFrameLateRotation * Vector3.forward);
                Vector3 up = hurtBox.transform.InverseTransformDirection(oneFrameLateRotation * Vector3.up);
                transform.localRotation = Quaternion.LookRotation(forward, up);
                transform.localPosition = hurtBox.transform.InverseTransformPoint(oneFrameLatePosition);
                */
                #endregion
            }
        }
    }

    private void SetDormantState()
    {
        if (rotationCoroutine != null)
        {
            StopCoroutine(rotationCoroutine);
            rotationCoroutine = null;
        }
        collider1.enabled = false;
        arrowRigidbody.isKinematic = true;
    }

}

