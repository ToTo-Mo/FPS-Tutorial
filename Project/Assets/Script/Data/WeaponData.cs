using UnityEngine;
using System;

public enum WeaponType {
    Hand,
    Melee,
    Projectile,
    Bow,
}

[Serializable]
public class WeaponData {

    public WeaponType type;
    public string name;

    [Tooltip("Weapon distance")]    
    public float effectiveDistance; // 유효 (사)거리
    public int damage;

    [Tooltip("Weapon action speed, like RPM")]    
    public float rateOfAction; // 분당 공격 횟수
    public float delayOfActionAfter;

    public float delayOfActionBefore;

    public Animator animator;


    public void SetRateOfAction(float value){
        rateOfAction = value;
        animator.SetFloat("AttackSpeed", rateOfAction / 60);
    }
}