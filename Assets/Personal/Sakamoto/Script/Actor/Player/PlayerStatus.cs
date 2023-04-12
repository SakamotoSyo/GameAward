using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayerStatus
{
    public ISkillBase[] PlayerSkillList => _skillList;
    public WeaponData[] WeponDatas => _weaponDatas;
    public PlayerEquipWeapon EquipWepon => _equipWeapon;
    private WeaponData[] _weaponDatas = new WeaponData[4];
    [Tooltip("�������Ă��镐��")]
    private PlayerEquipWeapon _equipWeapon = new();
    [Tooltip("�K�E�Z")]
    private ISkillBase _ninjaThrowingKnives;
    private ISkillBase[] _skillList = new ISkillBase[2];

    private PlayerStatus() 
    {
        //�����I��ō폜����
        for (int i = 0; i < _weaponDatas.Length; i++) 
        {
            _weaponDatas[i] = new(); 
            _weaponDatas[i].MaxDurable = 50;
            _weaponDatas[i].CurrentDurable = 50;
            _weaponDatas[i].OffensivePower = 50;
        }
        _equipWeapon.ChangeWeapon(_weaponDatas[0]);
    }

    public void ChangeWeponArray(WeaponData[] weaponDatas) 
    {
        _weaponDatas = weaponDatas;
    }

    /// <summary>
    /// �p�����[�^�̍X�V�����ĕ�������ւ���
    /// </summary>
    /// <param name="weaponData"></param>
    public void EwuipWeponChange(WeaponData weaponData) 
    {
        _weaponDatas[_equipWeapon.WeaponNum].UpdateParam(_equipWeapon);
        _equipWeapon.ChangeWeapon(weaponData);
    }

    public void SaveData() 
    {

    }
}

public class PlayerSaveData 
{
    public WeaponData EquipWepon;
    public List<ISkillBase> PlayerSkillList = new();
}
