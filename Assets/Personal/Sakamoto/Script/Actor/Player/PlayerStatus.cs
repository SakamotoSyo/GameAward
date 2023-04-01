using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using VContainer.Unity;

public class PlayerStatus : ActorStatusBase
{
    public List<ISkillBase> PlayerSkillList => _skillList;
    public WeaponData[] WeponDatas => _weaponDatas;
    public WeaponData EquipWepon => _equipWeapon;

    [Tooltip("���̃��x���܂ł̌o���l")]
    private int _maxExperienceAmount;
    [Tooltip("���݂̌o���l")]
    private int _currentExperienceAmount;
    [Tooltip("�������Ă��镐��")]
    private WeaponData _equipWeapon;
    private int _level;
    private int _skillPoint;
    private WeaponData[] _weaponDatas = new WeaponData[4];
    private List<ISkillBase> _skillList = new();

    private PlayerStatus() 
    {
        Init();
    }

    public void ChangeWeponArray(WeaponData[] weaponDatas) 
    {
        _weaponDatas = weaponDatas;
    }

    /// <summary>
    /// �o���l�̎擾���鏈��
    /// </summary>
    public void ExperienceAcquisition(int experience) 
    {
        //���x���A�b�v�܂ł̌o���l
        var SurplusExperience = _maxExperienceAmount - _currentExperienceAmount;

        if (SurplusExperience <= experience)
        {
            LevelUp();
            //���x���A�b�v���ė]�����o���l��ǉ��ŗ^����
            _currentExperienceAmount = experience - SurplusExperience;
        }
        else 
        {
            _currentExperienceAmount += experience;
        }
    }

    /// <summary>
    /// ���x���A�b�v�̏���
    /// </summary>
    private void LevelUp() 
    {
        _level++;
        _maxExperienceAmount = DataBaseScript.GetMaxExperienceAmount(_level);
    }

    public void SaveData() 
    {

    }
}

public class PlayerSaveData 
{
    public int Level;
    public int MaxExperienceAmount;
    public int CurrentExperienceAmount;
    public int SkillPoint;
    public WeaponData EquipWepon;
    public List<ISkillBase> PlayerSkillList = new();
}
