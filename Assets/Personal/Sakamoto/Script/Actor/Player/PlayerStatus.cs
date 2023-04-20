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
    [Tooltip("装備している武器")]
    private PlayerEquipWeapon _equipWeapon = new();
    [Tooltip("必殺技")]
    private SkillBase _ninjaThrowingKnives;
    private SkillBase[] _skillList = new SkillBase[2];

    private PlayerStatus() 
    {
        //TODO:試験的後で削除する
        for (int i = 0; i < _weaponDatas.Length; i++) 
        {
            _weaponDatas[i] = new(1000, 1000,50,1000, WeaponData.AttributeType.None, WeaponType.GreatSword); 
        }
        _equipWeapon.ChangeWeapon(_weaponDatas[0]);
    }

    public void ChangeWeponArray(WeaponData[] weaponDatas) 
    {
        _weaponDatas = weaponDatas;
    }

    /// <summary>
    /// パラメータの更新をして武器を入れ替える
    /// </summary>
    /// <param name="weaponData"></param>
    public void EquipWeponChange(WeaponData weaponData) 
    {
        _weaponDatas[_equipWeapon.WeaponNum].UpdateParam(_equipWeapon);
        _equipWeapon.ChangeWeapon(weaponData);
    }

    /// <summary>
    /// 使える武器を持っていた場合ランダムに入れ替える
    /// 入れ替えれなかった場合falseを返す
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

    public void SaveData() 
    {

    }
}

public class PlayerSaveData 
{
    public WeaponData EquipWeapon;
    public WeaponData[] WeaponArray;
    public SkillBase[] PlayerSkillList = new SkillBase[2];
    public SkillBase NinjaThrowingKnives;
    public int PlayerRankPoint;
}
