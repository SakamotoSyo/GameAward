using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class EnemyStatus
{
    private WeaponData[] _weaponDates;

    public WeaponData[] WeaponDates => _weaponDates;

    private EquipEnemyWeapon _epicEnemyWeapon = new();

    public EquipEnemyWeapon EquipEnemyWeapon => _epicEnemyWeapon;

    /// <summary>
    /// ����̔z��̍X�V
    /// </summary>
    public void SetWeaponDate(WeaponData[] weaponDatas)
    {
        _weaponDates = weaponDatas;
    }

    /// <summary>
    /// �ǂ̕���ɕύX���邩���w�肷��
    /// </summary>
    public void EquipChangeWeapon(int weaponNum)
    {
        _epicEnemyWeapon.ChangeWeapon(_weaponDates[weaponNum]);
    }
}
