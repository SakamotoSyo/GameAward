using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class EquipEnemyWeapon
{

    /// <summary>����̍U����</summary>
    private float _offensivePower;

    /// <summary>����̍U���͂̃v���p�e�B</summary>
    public float OffensivePower { get => _offensivePower; set => _offensivePower = value; }

    /// <summary>����̃N���e�B�J����</summary>
    private float _criticalRate;

    /// <summary>����̃N���e�B�J�����̃v���p�e�B</summary>
    public float CriticalRate { get => _criticalRate; set => _criticalRate = value; }

    /// <summary>����̍ő�ϋv�l</summary>
    private float _maxDurable;

    /// <summary>����̌��݂̑ϋv�l</summary>
    private ReactiveProperty<float> _currentDurable = new();

    /// <summary>����̌��݂̑ϋv�l�̃v���p�e�B</summary>
    public IReactiveProperty<float> CurrentDurable => _currentDurable;

    /// <summary>����̏d��</summary>
    private float _weaponWeight;

    /// <summary>����̏d���̃v���p�e�B</summary>
    public float WeaponWeight { get => _weaponWeight; set => _weaponWeight = value; }

    /// <summary>����̎��</summary>
    private WeaponType _weaponType;

    private int _breakCount = 0;

    public int WeaponBreakCount => _breakCount;

    /// <summary>
    /// ����ւ̃_���[�W
    /// </summary>
    public void AddDamage(float damage)
    {
        _currentDurable.Value -= damage;

    }

    public bool IsWeaponBreak()
    {
        if (_currentDurable.Value <= 0)
        {
            _breakCount++;
            return true;
        }
        return false;
    }

    /// <summary>
    /// �������Ă��镐�����シ��
    /// </summary>
    public void ChangeWeapon(WeaponData weaponData)
    {
        _offensivePower = weaponData.OffensivePower;
        _criticalRate = weaponData.CriticalRate;
        _maxDurable = weaponData.MaxDurable;
        _weaponWeight = weaponData.WeaponWeight;
        _currentDurable.Value = weaponData.CurrentDurable;
        _weaponType = weaponData.WeaponType;
    }

}
