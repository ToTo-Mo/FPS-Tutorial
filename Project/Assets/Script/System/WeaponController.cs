using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponController : MonoBehaviour
{
    [SerializeField]
    protected Weapon weapon;

    // 상태 변수
    protected bool isAttack = false;

    public void TryAttack(){
        if(Input.GetButton("Fire1")){
            if(!isAttack){
                // 1. coroutine
            }
        }
    }

    protected abstract IEnumerator AttackCoroutine();
    protected abstract IEnumerator HitCoroutine();

    protected abstract bool CheckObject();
}
