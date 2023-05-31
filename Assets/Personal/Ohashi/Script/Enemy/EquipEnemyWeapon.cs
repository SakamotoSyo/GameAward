using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using Cysharp.Threading.Tasks;
using Random = UnityEngine.Random;

public class EquipEnemyWeapon
{

    /// <summary>����̍U����</summary>
    private float _offensivePower;

    /// <summary>����̍U���͂̃v���p�e�B</summary>
    public float OffensivePower => _offensivePower;

    /// <summary>���݂̕���̍U����</summary>
    private float _currentOffensivePower;

    /// <summary>���݂̕���̍U���͂̃v���p�e�B</summary>
    public float CurrentOffensivePower { get => _currentOffensivePower; set => _currentOffensivePower = value; }

    /// <summary>����̃N���e�B�J����</summary>
    private float _criticalRate;

    /// <summary>����̃N���e�B�J�����̃v���p�e�B</summary>
    public float CriticalRate => _criticalRate;

    /// <summary>���݂̕���̃N���e�B�J����</summary>
    private float _currentCriticalRate;

    /// <summary>���݂̕���̃N���e�B�J�����̃v���p�e�B</summary>
    public float CurrentCriticalRate { get => _currentCriticalRate; set => _currentCriticalRate = value; }

    /// <summary>����̍ő�ϋv�l</summary>
    private ReactiveProperty<float> _maxDurable = new();

    public IReactiveProperty<float> MaxDurable => _maxDurable;

    /// <summary>���݂̕���̑ϋv�l</summary>
    private ReactiveProperty<float> _currentDurable = new();

    /// <summary>���݂̕���̑ϋv�l�̃v���p�e�B</summary>
    public IReactiveProperty<float> CurrentDurable => _currentDurable;

    /// <summary>����̏d��</summary>
    private float _weaponWeight;

    /// <summary>����̏d���̃v���p�e�B</summary>
    public float WeaponWeight => _weaponWeight;

    /// <summary>���݂̕���̏d��</summary>
    private float _currentWeaponWeight;

    /// <summary>���݂̕���̏d���̃v���p�e�B</summary>
    public float CurrentWeaponWeight { get => _currentWeaponWeight; set => _currentWeaponWeight = value; }

    /// <summary>����̎��</summary>
    private ReactiveProperty<WeaponType> _weaponType;

    public WeaponType WeaponType => _weaponType.Value;
    public IObservable<WeaponType> WeaponTypeOb => _weaponType;

    private WeaponSkill _weaponSkill;

    public WeaponSkill WeaponSkill => _weaponSkill;

    private int _breakCount = 0;

    public int WeaponBreakCount => _breakCount;

    /// <summary>
    /// ����ւ̃_���[�W
    /// </summary>
    public bool AddDamage(int damage, float criticalRate)
    {
        int critical = Random.Range(0, 100);
        if(critical >= criticalRate)
        {
            Debug.Log("�N���e�B�J��");
            damage = (int)((float)damage * 1.3f);
            _currentDurable.Value -= damage;
            return true;
        }
        _currentDurable.Value -= damage;
        return false;
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
        _currentOffensivePower = weaponData.OffensivePower;
        _currentCriticalRate = weaponData.CriticalRate;
        _maxDurable.Value = weaponData.MaxDurable;
        _currentWeaponWeight = weaponData.WeaponWeight;
        _currentDurable.Value = weaponData.CurrentDurable;
        _weaponType.Value = weaponData.WeaponType;
        _weaponSkill = weaponData.WeaponSkill;

        _offensivePower = weaponData.OffensivePower;
        _criticalRate = weaponData.CriticalRate;
        _weaponWeight = weaponData.WeaponWeight;
    }

    public void FluctuationStatus(FluctuationStatusClass fluctuation)
    {
        _currentOffensivePower += fluctuation.OffensivePower;
        _currentWeaponWeight += fluctuation.WeaponWeight;
        _currentCriticalRate += fluctuation.CriticalRate;
        _maxDurable.Value += fluctuation.MaxDurable;
        _currentDurable.Value += fluctuation.CurrentDurable;
    }

}
