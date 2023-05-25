using UnityEngine;
using Cysharp.Threading.Tasks;

public class EnemyAttack
{
    private EquipEnemyWeapon _equipWepon;

    private SkillDataManagement _skillDataManagement;

    private int[] _skillArray = new int[3] {0, 0, 1};

    public void Init(EquipEnemyWeapon equipWepon)
    {
        _equipWepon = equipWepon;
        _skillDataManagement = GameObject.Find("SkillDataBase").GetComponent<SkillDataManagement>();
    }

    /// <summary>
    /// �Z�������_���őI��
    /// </summary>
    public async UniTask SelectAttack()
    {
        int index = Random.Range(0, _skillArray.Length);

        if (_equipWepon.CurrentDurable.Value <= _equipWepon.CurrentDurable.Value / 2)
        {
            SpecialAttack();
        }

        if (_skillArray[index] == 0)
        {
            NormalAttack();
        }
        else
        {
            SkillAttack();
        }
    }

    /// <summary>
    /// �U��
    /// </summary>
    public async UniTask NormalAttack()
    {
        Debug.Log("normal");
        if(_equipWepon.WeaponType == WeaponType.GreatSword)
        {
            await _skillDataManagement.OnSkillUse(ActorAttackType.Enemy, "���ߎa��");
        }
        else if (_equipWepon.WeaponType == WeaponType.DualBlades)
        {
            await _skillDataManagement.OnSkillUse(ActorAttackType.Enemy, "�A���a��");
        }
        else if (_equipWepon.WeaponType == WeaponType.Hammer)
        {
            await _skillDataManagement.OnSkillUse(ActorAttackType.Enemy, "�S�͑ł�");

        }
        else if (_equipWepon.WeaponType == WeaponType.Spear)
        {
            await _skillDataManagement.OnSkillUse(ActorAttackType.Enemy, "��M");
        }

    }

    /// <summary>
    /// �X�L���U��
    /// </summary>
    private async UniTask SkillAttack()
    {
        int r = Random.Range(0, 2);
        if(r == 0)
        {
            Debug.Log("skill1");
            await _skillDataManagement.OnSkillUse(ActorAttackType.Enemy, _equipWepon.WeaponSkill.WeaponSkillArray[0]);
        }
        else
        {
            Debug.Log("skill2");
            await _skillDataManagement.OnSkillUse(ActorAttackType.Enemy, _equipWepon.WeaponSkill.WeaponSkillArray[1]);
        }
    }

    /// <summary>
    /// �K�E�Z
    /// </summary>
    private async UniTask SpecialAttack()
    {
        Debug.Log("special");
        await _skillDataManagement.OnSkillUse(ActorAttackType.Enemy, _equipWepon.WeaponSkill.SpecialAttack);
    }
}