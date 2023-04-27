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

    private bool _isStan = false;

    public bool IsStan { get => _isStan; set => _isStan = value; }

    private bool _isBoss = false;

    public bool IsBoss { get => _isBoss; set => _isBoss = value; }

    /// <summary>
    /// 武器の配列の更新
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
    /// どの武器に変更するかを指定する
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
