using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class IntialPreparationScript : MonoBehaviour
{
    public List<WeaponData> WeaponDatas => _weaponData;
    private List<WeaponData> _weaponData = new();

    //private void Start()
    //{
    //    SaveManager.Initialize();
    //    for (int i = 0; i < SaveManager._weaponFileList.Count; i++)
    //    {
    //        SaveManager.ResetSaveData(SaveManager._weaponFileList[i]);
    //    }

    //}

    /// <summary>
    /// �����z��ɂ����邱�Ƃ��o���邩�m�F���ē����
    /// </summary>
    /// <param name="weaponType"></param>
    /// <returns></returns>
    public bool SetWeaponTypeConfirmation(WeaponData weaponType)
    {
        WeaponData[] array = new WeaponData[2];
        if (_weaponData.Count != 0)
        {
            array = _weaponData.Where(x => x.WeaponType == weaponType.WeaponType).ToArray();
        }
        else 
        {
            _weaponData.Add(weaponType);
            return true;
        }
        
        if (array.Length == 0 && _weaponData.Count < 2)
        {
            _weaponData.Add(weaponType);
            return true;
        }
        else 
        {
           return false;
        }
    }
}
