using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponController : WeaponController
{
    // 상태 변수
    private bool isSwing = false;
    private RaycastHit hittedObject;

    private void Start() {
        
    }

    private void Update() {
        TryAttack();
    }

    private void TryAttack(){
        if(Input.GetButton("Fire1")){
            if(!isAttack){
                // 1. coroutine
                StartCoroutine(AttackCoroutine());
            }
        }
    }

    protected override IEnumerator AttackCoroutine() {
        isAttack = true;
        weapon.GetData().animator.SetFloat("AttackSpeed", weapon.GetData().rateOfAction / 60);

        float delayOfActionBefore = weapon.GetData().delayOfActionBefore;
        float delayOfActionAfter = ((MeleeWeaponData)weapon.GetData()).delayOfActionAfter;
        float actionPerSeconds = (60 / weapon.GetData().rateOfAction);

        // 공격 전 딜레이 또는 준비시간
        yield return new WaitForSeconds(delayOfActionBefore);
        weapon.GetData().animator.SetTrigger("Attack");
        isSwing = true;

        // 공격 활성화
        StartCoroutine(HitCoroutine());

        // 공격 후 딜레이
        yield return new WaitForSeconds(delayOfActionAfter);
        isSwing = false;

        // 공격속도
        yield return new WaitForSeconds(actionPerSeconds - (delayOfActionBefore + delayOfActionAfter));
        isAttack = false;
    }

    protected override IEnumerator HitCoroutine(){
        while(isSwing){
            if(CheckObject()){
                isSwing = false;
                Debug.Log(hittedObject);
            }

            yield return null;
        }
    }

    protected override bool CheckObject()
    {
        if (Physics.Raycast(transform.position,
            transform.forward,
            out hittedObject,
            weapon.GetData().effectiveDistance))
        {
            return true;
        }

        return false;
    }
}
