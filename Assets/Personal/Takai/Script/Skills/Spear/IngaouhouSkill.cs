using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;
using System;

public class IngaouhouSkill : SkillBase
{
    [SerializeField] private PlayableDirector _anim;
    [SerializeField] private GameObject _playerObj;
    private PlayerController _playerStatus;
    private EnemyController _enemyStatus;

    public IngaouhouSkill()
    {
        SkillName = "因果応報";
        Damage = 70;
        Weapon = (WeaponType)3;
        Type = (SkillType)0;
        FlavorText = "発動したターンに攻撃を受けるとダメージを30%軽減し、反撃する。";
    }

    public override bool IsUseCheck(PlayerController player)
    {
        return true;
    }

    public async override UniTask UseSkill(PlayerController player, EnemyController enemy, ActorAttackType actorType)
    {
<<<<<<< HEAD
        Debug.Log("Use Skill");
=======
>>>>>>> 18306373660527c4c8546ab126057dd241e19a81
        _playerStatus = player;
        _enemyStatus = enemy;
        _playerObj.SetActive(true);
        _playerStatus.gameObject.SetActive(false);
        _anim.Play();
        SkillEffect();
        await UniTask.WaitUntil(() => _anim.state == PlayState.Paused,
            cancellationToken: this.GetCancellationTokenOnDestroy());
        _anim.Stop();
        await UniTask.Delay(TimeSpan.FromSeconds(0.5));
        _playerStatus.gameObject.SetActive(true);
        _playerObj.SetActive(false);
    }

    protected override void SkillEffect()
    {
    }

    public override async UniTask InEffectSkill(ActorAttackType attackType)
    {
        if (attackType == ActorAttackType.Player)
        {
            _playerObj.SetActive(true);
            _anim.Play();
            _enemyStatus.AddDamage(_playerStatus.PlayerStatus.EquipWeapon.GetPowerPram() * (Damage * 0.01f));
<<<<<<< HEAD
            await UniTask.WaitUntil(() => _anim.time >= _anim.duration - 0.1,
                cancellationToken: this.GetCancellationTokenOnDestroy());
=======
            await UniTask.WaitUntil(() => _anim.time >= _anim.duration - 0.1, cancellationToken: this.GetCancellationTokenOnDestroy());
            _playerObj.SetActive(false);
>>>>>>> 18306373660527c4c8546ab126057dd241e19a81
        }
        else
        {
            _anim.Play();
            // _playerStatus.AddDamage(_enemyStatus.EnemyStatus.EquipWeapon.GetPowerPram() * (Damage * 0.01f));
        }
    }

    public override bool TurnEnd()
    {
        return false;
    }

    public override void BattleFinish()
    {
    }
}