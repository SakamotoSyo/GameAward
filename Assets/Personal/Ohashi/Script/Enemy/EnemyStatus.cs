using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class EnemyStatus
{
    private WeaponData[] _weaponDates;

    public WeaponData[] WeaponDates => _weaponDates;

    private EquipEnemyWeapon _epicWeapon = new();

    public EquipEnemyWeapon EquipWeapon => _epicWeapon;

    /// <summary>
    /// ����̔z��̍X�V
    /// </summary>
    public void SetWeaponDates(EnemyData enemyDate)
    {
        _weaponDates = enemyDate.WeaponDates;
    }

    /// <summary>
    /// �ǂ̕���ɕύX���邩���w�肷��
    /// </summary>
    public void EquipChangeWeapon(int weaponNum)
    {
        _epicWeapon.ChangeWeapon(_weaponDates[weaponNum]);
    }
}
