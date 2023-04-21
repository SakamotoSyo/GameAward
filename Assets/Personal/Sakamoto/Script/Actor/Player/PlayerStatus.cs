using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;

public class PlayerStatus
{
    public SkillBase NinjaThrowingKnives => _ninjaThrowingKnives;
    public SkillBase[] PlayerSkillList => _skillList;
    public WeaponData[] WeaponDatas => _weaponDatas;
    public PlayerEquipWeapon EquipWeapon => _equipWeapon;
    private WeaponData[] _weaponDatas = new WeaponData[4];
    [Tooltip("�������Ă��镐��")]
    private PlayerEquipWeapon _equipWeapon = new();
    [Tooltip("�K�E�Z")]
    private SkillBase _ninjaThrowingKnives;
    private SkillBase[] _skillList = new SkillBase[2];
    private int _playerRankPoint = 0;

    private PlayerStatus()
    {
        //TODO:�����I��ō폜����
        for (int i = 0; i < _weaponDatas.Length; i++)
        {
            _weaponDatas[i] = new(1000, 1000, 50, 1000, WeaponData.AttributeType.None, WeaponType.GreatSword);
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
    public void EquipWeponChange(WeaponData weaponData)
    {
        _weaponDatas[_equipWeapon.WeaponNum].UpdateParam(_equipWeapon);
        _equipWeapon.ChangeWeapon(weaponData);
    }

    /// <summary>
    /// �g���镐��������Ă����ꍇ�����_���ɓ���ւ���
    /// ����ւ���Ȃ������ꍇfalse��Ԃ�
    /// </summary>
    public bool RandomEquipWeponChange()
    {
        _weaponDatas[_equipWeapon.WeaponNum].UpdateParam(_equipWeapon);
        var weaponArray = _weaponDatas.Where(x => 0 < x.CurrentDurable).ToArray();
        if (weaponArray.Length == 0)
        {
            Debug.Log("GameOver");
            return false;
        }
        else
        {
            _equipWeapon.ChangeWeapon(weaponArray[0]);
        }
        return true;
    }

    public float ConventionalAttack()
    {
        return _equipWeapon.OffensivePower.Value;
    }

    public void SaveStatus()
    {
        var playerData = new PlayerSaveData();
        playerData.WeaponArray = _weaponDatas;
        playerData.PlayerSkillList = PlayerSkillList;
        playerData.NinjaThrowingKnives = NinjaThrowingKnives;
        playerData.PlayerRankPoint = _playerRankPoint;
        GameManager.SetPlayerData(playerData);
    }

    public void LoadStatus(PlayerSaveData player)
    {
        _weaponDatas = player.WeaponArray;
        _equipWeapon.ChangeWeapon(player.WeaponArray[0]);
        _skillList = player.PlayerSkillList;
        _ninjaThrowingKnives = player.NinjaThrowingKnives;
        _playerRankPoint = player.PlayerRankPoint;
    }
}

public class PlayerSaveData
{
    public WeaponData[] WeaponArray;
    public SkillBase[] PlayerSkillList = new SkillBase[2];
    public SkillBase NinjaThrowingKnives;
    public int PlayerRankPoint;
}
