using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Enemy_HurtBox : MonoBehaviour
{
    private Enemy enemy;
    public float damageRatio;
    public Transform uniScaleTransform;
    // 클래스 PlayerArrow에 기록한 SetParent 와 Non-Uni-Scaling 문제 때문에 nullTransform 을 public으로 설정했는데, 지금은 콜라이더에 Mesh를 넣고 참조 용도로 했지만, 굳이 Mesh를 넣을 필요가 있나 싶다. 차라리 Collider의 Mesh를 삭제하고, uni-Transform으로 하는게 낫지 않을까 싶음
    // 현재는 위 기능 외에도, ForTest로 enemy에 정보를 보내 디버그로 uniScaleTransform.gameObject.name 을 디버그해서 연결한 본의 이름도 디버그하기 때문에, 지금은 현상태로 작업하고 있음

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
            Debug.Log("Enemy 를 참조하지 못함");
        }
    }

    
}
