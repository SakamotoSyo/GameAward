using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class EnemyStatus
{
    private WeaponData[] _weaponDatas;

    public WeaponData[] WeaponDates => _weaponDatas;

    private EquipEnemyWeapon _epicWeapon = new();

    public EquipEnemyWeapon EquipWeapon => _epicWeapon;

    private SkillBase[] _skills = new SkillBase[2];

    private SkillBase _specialSkill;

    /// <summary>
    /// ����̔z��̍X�V
    /// </summary>
    public void SetWeaponDates(EnemyData enemyData)
    {
        _weaponDatas = enemyData.WeaponDates;
        _epicWeapon.ChangeWeapon(_weaponDatas[0]);
    }

    public bool IsWeaponsAllBrek()
    {
        if(_epicWeapon.WeaponBreakCount >= _weaponDatas.Length)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// �ǂ̕���ɕύX���邩���w�肷��
    /// </summary>
    public void EquipChangeWeapon()
    {
        foreach(WeaponData weapon in _weaponDatas)
        {
            if(weapon.CurrentDurable > 0)
            {
                _epicWeapon.ChangeWeapon(weapon);
                return;
            }
        }
    }
}
