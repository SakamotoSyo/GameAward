using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

public class KiaiSkill : SkillBase
{
    private PlayableDirector _anim;
    private PlayerController _playerStatus;
    private EnemyController _enemyStatus;

    public KiaiSkill()
    {
        SkillName = "気合い";
        Damage = 0;
        Weapon = (WeaponType)2;
        Type = (SkillType)0;
        FlavorText = "次の攻撃だけ威力が2倍に上昇";
    }
    
    private void Start()
    {
        _anim = GetComponent<PlayableDirector>();
    }

    
    public override bool IsUseCheck(PlayerController player)
    {
        return true;
    }

    public async override UniTask UseSkill(PlayerController player, EnemyController enemy, ActorAttackType actorType)
    {
        Debug.Log("Use Skill");
        _playerStatus = player;
        _enemyStatus = enemy;
        _anim = GetComponent<PlayableDirector>();
        _anim.Play();
        SkillEffect();
        await UniTask.WaitUntil(() => _anim.state == PlayState.Paused,
            cancellationToken: this.GetCancellationTokenOnDestroy());
        Debug.Log("Anim End");
    }

    protected override void SkillEffect()
    {
        // スキルの効果処理を実装する
        _enemyStatus.AddDamage(_playerStatus.PlayerStatus.EquipWeapon.OffensivePower.Value * 2);
    }

    public override bool TurnEnd()
    {
        return false;
    }

    public override void BattleFinish()
    {
        
    }
}