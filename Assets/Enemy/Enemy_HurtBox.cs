using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Enemy_HurtBox : MonoBehaviour
{
    private Enemy enemy;
    public float damageRatio;
    public Transform uniScaleTransform;
    // Ŭ���� PlayerArrow�� ����� SetParent �� Non-Uni-Scaling ���� ������ nullTransform �� public���� �����ߴµ�, ������ �ݶ��̴��� Mesh�� �ְ� ���� �뵵�� ������, ���� Mesh�� ���� �ʿ䰡 �ֳ� �ʹ�. ���� Collider�� Mesh�� �����ϰ�, uni-Transform���� �ϴ°� ���� ������ ����
    // ����� �� ��� �ܿ���, ForTest�� enemy�� ������ ���� ����׷� uniScaleTransform.gameObject.name �� ������ؼ� ������ ���� �̸��� ������ϱ� ������, ������ �����·� �۾��ϰ� ����

    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    private void Start()
    {
        //ForTest
        //enemy.ForTestMethod_CheckHurtColliders(damageRatio, gameObject.layer, uniScaleTransform);
    }

    public void GetDamage(float damage)
    {
        if(enemy != null)
        {
            enemy.GetDamage(damage * damageRatio);
            Debug.Log($"{uniScaleTransform.gameObject.name} - {damage * damageRatio}");
        }
        else
        {
            Debug.Log("Enemy �� �������� ����");
        }
    }

    
}
