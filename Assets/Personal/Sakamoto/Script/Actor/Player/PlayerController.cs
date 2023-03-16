using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private IPlayerStatus _playerStatus;
    private IStatusBase _statusBase;
    private PlayerAnimation _playerAnimation = new();

    private void Start()
    {
        _statusBase = _playerStatus.GetStatusBase();
    }

    /// <summary>
    /// ダメージを受ける流れ
    /// </summary>
    public void AddDamage(float damage) 
    {
        if (_statusBase.DownJudge(damage))
        {
            _playerStatus.GetStatusBase().AddDamage(damage);
        }
        else 
        {
            //死んだときの処理を追加
        }
        
    }

    public void SetPlayerStatus(IPlayerStatus playerStatus) 
    {
        _playerStatus = playerStatus;
    }
}
