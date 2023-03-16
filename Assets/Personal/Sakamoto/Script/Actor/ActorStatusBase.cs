using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class ActorStatusBase :IStatusBase
{
    private ReactiveProperty<float> _maxHp = new();
    private ReactiveProperty<float> _currentHp = new();

    public void AddDamage(float damage) 
    {
        _currentHp.Value = damage;
    }

    /// <summary>
    /// �U���Ń_�E�����邩���肷��
    /// �h��͂Ƃ����������ꍇ����Ȋ֐�������Ɗy
    /// </summary>
    /// <param name="damage"></param>
    /// <returns></returns>
    public bool DownJudge(float damage)
    {
        return 0 < _currentHp.Value - damage;
    }

    public IReactiveProperty<float> GetMaxHpOb()
    {
        return _maxHp;
    }

    public IReactiveProperty<float> GetCurrentHpOb()
    {
        return _currentHp;
    }
}
