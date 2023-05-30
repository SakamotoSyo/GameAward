using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private Text _currentHpText;
    [SerializeField] private Text _maxMpText;
    [SerializeField] private Image _currentHpImage;
    [SerializeField] private Sprite[] _weaponArrayImage = new Sprite[4];
    [SerializeField] private Image _weaponImage;
    private float _maxHp;
    private float _currentHp;

    public void SetCurrentHp(float num) 
    {
        _currentHpText.text = num.ToString();
        _currentHp = num;
        AdjustmentHpBar();
        Debug.Log($"���݂�Hpsasa��{num}");
    }

    public void SetMaxHp(float num) 
    {
        _maxMpText.text = num.ToString();
        _maxHp = num;
        //AdjustmentHpBar();
    }

    private void AdjustmentHpBar() 
    {
        _currentHpImage.DOFillAmount(_currentHp / _maxHp, 0.5f);
    }

    public void ChangeWeaponIcon(WeaponType weaponType) 
    {
        if (weaponType == WeaponType.DualBlades)
        {
           _weaponImage.sprite = _weaponArrayImage[0];
        }
        else if (weaponType == WeaponType.Hammer)
        {
            _weaponImage.sprite = _weaponArrayImage[1];
        }
        else if (weaponType == WeaponType.Spear)
        {
            _weaponImage.sprite = _weaponArrayImage[2];
        }
        else if (weaponType == WeaponType.GreatSword) 
        {
            _weaponImage.sprite = _weaponArrayImage[3];
        }
    }
}