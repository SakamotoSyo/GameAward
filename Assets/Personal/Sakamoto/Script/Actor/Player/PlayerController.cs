using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerStatus PlayerStatus => _playerStatus;

    private PlayerStatus _playerStatus;
    private PlayerAnimation _playerAnimation = new();

    private void Start()
    {
        
    }

    private void Update()
    {
       
    }

    /// <summary>
    /// �_���[�W���󂯂闬��
    /// </summary>
    public void AddDamage(float damage) 
    {
        if (_playerStatus.EquipWeapon.DownJudge(damage))
        {
            //�A�j���[�V�������������炱���Ń_���[�W���󂯂鏈�����Ă�
            _playerStatus.EquipWeapon.AddDamage(damage);
        }
        else 
        {
            //���񂾂Ƃ��̏�����ǉ�
        }
        
    }

    public void SetPlayerStatus(PlayerStatus playerStatus) 
    {
        _playerStatus = playerStatus;
    }
}
