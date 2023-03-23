using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class ActorStatusBase
{
    public IReactiveProperty<float> MaxHp => _maxHp;
    public IReactiveProperty<float> CurrentHp => _currentHp;

    private ReactiveProperty<float> _maxHp = new();
    private ReactiveProperty<float> _currentHp = new();

    public void Init() 
    {
        _maxHp.Value = 50;
        _currentHp.Value = 50;
    }

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
}
