using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine.Playables;

public class ShinsokuranbuSkill : SkillBase
{
    [SerializeField] private PlayableDirector _anim;
    [SerializeField] private GameObject _playerObj;
    private PlayerController _playerStatus;
    private EnemyController _enemyStatus;
    private ActorAttackType _actor;

    public ShinsokuranbuSkill()
    {
        SkillName = "神速乱舞";
        Damage = 150;
        RequiredPoint = 5;
        Weapon = (WeaponType)1;
        Type = (SkillType)1;
        FlavorText = "重さが30以下のとき発動可能　※使用後元のステータスに戻る";
    }

    public override bool IsUseCheck(PlayerController player)
    {
        float weight = player.PlayerStatus.EquipWeapon.WeaponWeight.Value;
        return (weight >= 100) ? true : false;
    }

    public async override UniTask UseSkill(PlayerController player, EnemyController enemy, ActorAttackType actorType)
    {
        Debug.Log("Use Skill");
        _playerStatus = player;
        _enemyStatus = enemy;
        _playerObj.SetActive(true);
        _playerStatus.gameObject.SetActive(false);
        _anim.Play();
        var dura = _anim.duration * 0.99f;
        await UniTask.WaitUntil(() => _anim.time >= dura,
            cancellationToken: this.GetCancellationTokenOnDestroy());
        _anim.Stop();
        await UniTask.Delay(TimeSpan.FromSeconds(0.5));
        SkillEffect();
        _playerStatus.gameObject.SetActive(true);
        Debug.Log("Anim End");
        _playerObj.SetActive(false);
    }

    protected override void SkillEffect()
    {
        float weight = 0;

        switch (_actor)
        {
            case ActorAttackType.Enemy:
            {
                weight = _playerStatus.PlayerStatus.EquipWeapon.WeaponWeight.Value;
                _enemyStatus.AddDamage(_playerStatus.PlayerStatus.EquipWeapon.OffensivePower.Value + Damage);
            }
                break;
            case ActorAttackType.Player:
            {
                weight = _enemyStatus.EnemyStatus.EquipWeapon.WeaponWeight;
                _playerStatus.AddDamage(_enemyStatus.EnemyStatus.EquipWeapon.CurrentOffensivePower + Damage);
            }
                break;
        }
    }

    public override bool TurnEnd()
    {
        //ステータスが元に戻る処理

        return false;
    }

    public override void BattleFinish()
    {
    }
}