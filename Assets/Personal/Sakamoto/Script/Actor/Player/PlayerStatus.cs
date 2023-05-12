using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;

public class PlayerStatus
{
    public WeaponData[] WeaponDatas => _weaponDatas;
    public PlayerEquipWeapon EquipWeapon => _equipWeapon;
    public StateAnomaly CurrentAnomaly => _currentAnomaly;

    private WeaponData[] _weaponDatas = new WeaponData[4];
    [Tooltip("�������Ă��镐��")]
    private PlayerEquipWeapon _equipWeapon = new();
    private int _playerRankPoint = 0;
    private StateAnomaly _currentAnomaly = StateAnomaly.None;

    private WeaponSaveData _weaponSaveData = default;
    private PlayerStatus()
    {
        //TODO:�����I��ō폜����
        for (int i = 0; i < _weaponDatas.Length; i++)
        {
            _weaponDatas[i] = new(1000, 1000, 50, 1000, WeaponData.AttributeType.None, WeaponType.DualBlades);
        }
        _equipWeapon.ChangeWeapon(_weaponDatas[0], 0);

        _weaponSaveData = new WeaponSaveData();

        //for(int i = 0; i < _weaponDatas.Length; i++)
        //{
        //    if (_weaponDatas[i].WeaponType == WeaponType.GreatSword)
        //    {
        //        WeaponSaveData.GSData = SaveManager.Load(SaveManager.GREATSWORDFILEPATH);
        //    }
        //    if (_weaponDatas[i].WeaponType == WeaponType.DualBlades)
        //    {
        //        WeaponSaveData.DBData = SaveManager.Load(SaveManager.DUALBLADES);
        //    }
        //    if (_weaponDatas[i].WeaponType == WeaponType.Hammer)
        //    {
        //        WeaponSaveData.HData = SaveManager.Load(SaveManager.HAMMERFILEPATH);
        //    }
        //    if (_weaponDatas[i].WeaponType == WeaponType.Spear)
        //    {
        //        WeaponSaveData.SData = SaveManager.Load(SaveManager.SPEARFILEPATH);
        //    }
        //}

    }

    public void ChangeWeponArray(WeaponData[] weaponDatas)
    {
        _weaponDatas = weaponDatas;
    }

    public void AddRankPoint()
    {
        _playerRankPoint += 100;
    }

    /// <summary>
    /// �p�����[�^�̍X�V�����ĕ�������ւ���
    /// </summary>
    /// <param name="weaponData"></param>
    public void EquipWeponChange(WeaponData weaponData, int arrayNum)
    {
        _weaponDatas[_equipWeapon.WeaponNum].UpdateParam(_equipWeapon);
        _equipWeapon.ChangeWeapon(weaponData, arrayNum);
    }

    /// <summary>
    /// �g���镐��������Ă����ꍇ�����_���ɓ���ւ���
    /// ����ւ���Ȃ������ꍇfalse��Ԃ�
    /// </summary>
    public bool RandomEquipWeponChange()
    {
        _weaponDatas[_equipWeapon.WeaponNum].UpdateParam(_equipWeapon);
        for (int i = 0; i < _weaponDatas.Length; i++)
        {
            if (0 < _weaponDatas[i].CurrentDurable)
            {
                _equipWeapon.ChangeWeapon(WeaponDatas[i], i);
                return true;
            }
        }
        return false;
    }

    public float ConventionalAttack()
    {
        return _equipWeapon.OffensivePower.Value;
    }

    public bool ChackAnomaly()
    {
        if (StateAnomaly.Stun == _currentAnomaly)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// ��Ԉُ�̕t�^
    /// </summary>
    /// <param name="anomaly"></param>
    public void SetStateAnomaly(StateAnomaly anomaly)
    {
        _currentAnomaly = anomaly;
    }

    public void SaveStatus(PlayerSaveData saveData)
    {
        saveData.WeaponArray = _weaponDatas;
        saveData.PlayerRankPoint = _playerRankPoint;
    }

    public void LoadStatus(PlayerSaveData player)
    {
        _weaponDatas = player.WeaponArray;
        _equipWeapon.ChangeWeapon(player.WeaponArray[0], 0);
        _playerRankPoint = player.PlayerRankPoint;
    }
}

public class PlayerSaveData
{
    public WeaponData[] WeaponArray;
    public SkillBase[] PlayerSkillArray = new SkillBase[2];
    public SkillBase SpecialAttack;
    public int PlayerRankPoint;
}

public enum StateAnomaly
{
    None,
    Stun
}