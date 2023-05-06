using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class SkillDataManagement : MonoBehaviour
{
    [SerializeField] private ActorGenerator _actorGenerator;

    [SerializeField] private PlayerStatus _pStatus;
    [SerializeField] private EnemyStatus _eStatus;
    [SerializeField] private WeaponStatus _wStatus;

    private List<SkillBase> _skills = new List<SkillBase>();
    public IReadOnlyList<SkillBase> PlayerSkillList => _skills;

    private void Awake()
    {
        SkillBase[] skillPrefabs = Resources.LoadAll<SkillBase>("Skills");

        foreach (var skill in skillPrefabs)
        {
            var sObj = Instantiate(skill, transform);
            _skills.Add(sObj);
        }
    }

    public SkillBase OnSkillCall(WeaponType weapon, SkillType type)
    {
        List<SkillBase> skills = new List<SkillBase>();

        foreach (var s in _skills)
        {
            if (s.Weapon == weapon && s.Type == type)
            {
                skills.Add(s);
                Debug.Log($"{s}を検索");
            }
        }

        int n = Random.Range(0, skills.Count);

        return skills[n];
    }

    public void OnSkillUse<T>(ActorAttackType actorType) where T : SkillBase
    {
        SkillBase skill = _skills.Find(skill => skill.GetType() == typeof(T));
        if (skill != null)
        {
            _pStatus = _actorGenerator.PlayerController.PlayerStatus;
            _eStatus = _actorGenerator.EnemyController.EnemyStatus;

            skill.UseSkill(_pStatus, _eStatus, _wStatus,actorType);
        }
        else
        {
            Debug.Log("スキルが見つかりません");
        }
    }

    public void TurnCall()
    {
        Debug.Log("TurnCall呼び出し");
        foreach (var skill in _skills)
        {
            if (skill.gameObject.activeSelf)
            {
                skill.TurnEnd();
            }
        }
    }
}

public enum ActorAttackType
{
    Player,
    Enemy,
}