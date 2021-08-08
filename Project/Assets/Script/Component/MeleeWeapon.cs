using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    [SerializeField]
    private MeleeWeaponData meleeWeaponData;

    public override WeaponData GetData(){
        return meleeWeaponData;
    }
}
