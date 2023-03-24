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

    /// <summary>
    /// �_���[�W���󂯂闬��
    /// </summary>
    public void AddDamage(float damage) 
    {
        if (_playerStatus.DownJudge(damage))
        {
            _playerStatus.AddDamage(damage);
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
