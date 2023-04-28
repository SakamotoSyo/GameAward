using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

public class KiriageSkill : SkillBase
{
    public override string SkillName { get; protected set; }
    public override int Damage { get; protected set; }
    public override WeaponType Weapon { get; protected set; }
    public override SkillType Type { get; protected set; }
    public override string FlavorText { get; protected set; }

    private PlayerStatus _playerStatus;
    private PlayableDirector _anim;
    private const float AddDamageValue = 0.05f;
    private const int Turn = 3;
    private int _count = 0;
    private float _attackValue = 0;

    public KiriageSkill()
    {
        SkillName = "斬り上げ";
        Damage = 70;
        Weapon = (WeaponType)0;
        Type = (SkillType)0;
    }

    public async override UniTask UseSkill(PlayerStatus player, EnemyStatus enemy, WeaponStatus weapon)
    {
        Debug.Log("Use Skill");
        _playerStatus = player;
        _anim = GetComponent<PlayableDirector>();
        SkillEffect();
        await UniTask.WaitUntil(() => _anim.state == PlayState.Paused);
        Debug.Log("Anim End"); 
    }

    protected override void SkillEffect()
    {
        float dmg = _playerStatus.EquipWeapon.OffensivePower.Value;
        // スキルの効果処理を実装する
        if (_count <= Turn)
        {
            _count++;
            _attackValue += (dmg * (AddDamageValue * _count)) + Damage;
            _playerStatus.EquipWeapon.OffensivePower.Value += (dmg * (AddDamageValue * _count));
        }
    }

    public override void TurnEnd()
    {
        _playerStatus.EquipWeapon.OffensivePower.Value -= _attackValue;
    }


    public override void BattleFinish()
    {
        _playerStatus.EquipWeapon.OffensivePower.Value -= _attackValue;
        _count = 0;
        _attackValue = 0;
    }
}