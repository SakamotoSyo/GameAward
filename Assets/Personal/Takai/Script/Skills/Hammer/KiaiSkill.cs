using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

public class KiaiSkill : SkillBase
{
    private PlayableDirector _anim;
    private PlayerController _playerStatus;

    public KiaiSkill()
    {
        SkillName = "気合い";
        Damage = 0;
        Weapon = (WeaponType)2;
        Type = (SkillType)0;
        FlavorText = "次の攻撃だけ威力が2倍に上昇";
    }

    public async override UniTask UseSkill(PlayerController player, EnemyController enemy, ActorAttackType actorType)
    {
        Debug.Log("Use Skill");
        _playerStatus = player;
        _anim = GetComponent<PlayableDirector>();
        SkillEffect();
        await UniTask.WaitUntil(() => _anim.state == PlayState.Paused,
            cancellationToken: this.GetCancellationTokenOnDestroy());
        Debug.Log("Anim End");
    }

    protected override void SkillEffect()
    {
        // スキルの効果処理を実装する
        _playerStatus.AddDamage(_playerStatus.PlayerStatus.EquipWeapon.OffensivePower.Value * 2);
    }

    public override bool TurnEnd()
    {
        return false;
    }

    public override void BattleFinish()
    {
        
    }
}