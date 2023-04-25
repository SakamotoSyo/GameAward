using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

public class SeishintouitsuSkill : SkillBase
{
    public override string SkillName { get; protected set; }
    public override int Damage { get; protected set; }
    public override WeaponType Weapon { get; protected set; }
    public override SkillType Type { get; protected set; }
    public override string FlavorText { get; protected set; }
    private PlayableDirector _anim;
    private PlayerStatus _status;

    public SeishintouitsuSkill()
    {
        SkillName = "精神統一";
        Damage = 0;
        Weapon = (WeaponType)3;
        Type = (SkillType)0;
    }

    public async override UniTask UseSkill(PlayerStatus status)
    {
        Debug.Log("Use Skill");
        _status = status;
        _anim = GetComponent<PlayableDirector>();
        SkillEffect();
        await UniTask.WaitUntil(() => _anim.state == PlayState.Paused);
        Debug.Log("Anim End");
    }

    protected override void SkillEffect()
    {
        // スキルの効果処理を実装する
    }
    
    public override void TurnEnd()
    {
            
    }

    public override void BattleFinish()
    {
    }
}