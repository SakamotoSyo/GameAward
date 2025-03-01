using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

public class KiaiSkill : SkillBase
{
    [SerializeField] private PlayableDirector _anim;
    [SerializeField] private GameObject _playerObj;
    private PlayerController _playerStatus;
    private float _attackValue;
    private int _count;

    public KiaiSkill()
    {
        SkillName = "気合い";
        Damage = 0;
        RequiredPoint = 0;
        Weapon = (WeaponType)2;
        Type = (SkillType)0;
        FlavorText = "次の攻撃だけ威力が2倍に上昇";
    }
    
    public override bool IsUseCheck(ActorGenerator actor)
    {
        _playerStatus = actor.PlayerController;
        
        return true;
    }

    public async override UniTask UseSkill(PlayerController player, EnemyController enemy, ActorAttackType actorType)
    {
        Debug.Log("Use Skill");
        _playerStatus = player;
        _playerObj.SetActive(true);
        _playerStatus.gameObject.SetActive(false);
        _anim.Play();
        var dura = _anim.duration * 0.99f;
        await UniTask.WaitUntil(() => _anim.time >= dura,
            cancellationToken: this.GetCancellationTokenOnDestroy());
        SkillEffect();
        _anim.Stop();
        await UniTask.Delay(TimeSpan.FromSeconds(0.5));
        _playerStatus.gameObject.SetActive(true);
        Debug.Log("Anim End");
        _playerObj.SetActive(false);
    }

    protected override void SkillEffect()
    {
        // スキルの効果処理を実装する
        _count++;

        _attackValue = _playerStatus.PlayerStatus.EquipWeapon.GetPowerPram();
        FluctuationStatusClass fluctuation =
            new FluctuationStatusClass(_attackValue, 0, 0, 0, 0);
        _playerStatus.PlayerStatus.EquipWeapon.FluctuationStatus(fluctuation);
    }

    public override bool TurnEnd()
    {
        if (_count > 0)
        {
            _count--;
        }
        else
        {
            FluctuationStatusClass fluctuation =
                new FluctuationStatusClass(-_attackValue, 0, 0, 0, 0);
            _playerStatus.PlayerStatus.EquipWeapon.FluctuationStatus(fluctuation);
            _attackValue = 0;
        }

        return true;
    }

    public override void BattleFinish()
    {
        FluctuationStatusClass fluctuation =
            new FluctuationStatusClass(-_attackValue, 0, 0, 0, 0);
        _playerStatus.PlayerStatus.EquipWeapon.FluctuationStatus(fluctuation);
        _attackValue = 0;
    }
}