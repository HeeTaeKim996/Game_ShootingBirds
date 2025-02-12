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

                #region ��������
                /*
                �� SetParent���� Non-Uni-Scaling ���� 
                - transform.SetParent(A,true)���� A �Ǵ� A�� �θ�� ������ �ϳ��� Scale�� x,y,z���� ���� �ٸ� ���, SetParent�� (2) �� true�� �ص�, �ڽ����� �����Ǵ� ������Ʈ�� �����̼��� �ܰ�ȴ�.
                  �ڽ����� ������ ��, ������, �����̼�, �������� �����ĵǴµ�, �� �� �θ� ��� ������Ʈ�� �������� Uni-Scale�� �������� �ż��尡 ������� �� �ϴ�. 
                  ����Ƽ�� �������� �õ�1, �õ�2 �� �غ�����, �ذ��� �ȵ���. 
                  ���� FBX �𵨸����� ������ �ٿ�޾Ƽ� ����� ������, ��κ��� ������Ʈ�� Uni Scaling�̱� ������ �̷� ��찡 ������ ������, ��Ʈ�ڽ� �� �ݶ��̴��� �� ���� �����ϸ��� �� �ִ�.
                  

                ���� �õ�1, �õ�2�� �ذ��� �������� ��������� ���ܵ�
                
                �� �õ�1
                Matrix4x4 parentMatrix = Matrix4x4.TRS(Vector3.zero, hurtBox.transform.rotation, hurtBox.transform.lossyScale);
                Matrix4x4 inverseParentMatrix = parentMatrix.inverse;
                Matrix4x4 childMatrix = Matrix4x4.TRS(oneFrameLatePosition, oneFrameLateRotation, transform.lossyScale);
                Matrix4x4 localMatrix = inverseParentMatrix * childMatrix;
                transform.localRotation = localMatrix.rotation;
                transform.localPosition = other.transform.InverseTransformPoint(oneFrameLatePosition);

                �� �õ�2
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

