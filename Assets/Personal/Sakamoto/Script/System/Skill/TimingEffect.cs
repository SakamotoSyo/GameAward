using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingEffect :MonoBehaviour, ISkill
{
    [Tooltip("�o�����̐�")]
    [SerializeField] private int _arrowNum;
    [Tooltip("SKill�Ɏg��Obj")]
    [SerializeField] private GameObject _skillObj;
    [Tooltip("Notes�̔w�i�𐶐�����ꏊ")]
    [SerializeField] private GameObject _notesBackInsObj;    
    [Tooltip("Notes�𐶐�����ꏊ")]
    [SerializeField] private GameObject _notesInsObj;
    [Tooltip("����Prefab")]
    [SerializeField] private GameObject _arrowPrefab;
    [Tooltip("1���҂����肾�Ƃ��Ăǂ̒��x�͈̔͂𐬌��Ƃ��邩")]
    private float _successRange = 0.1f;

    private async void Start() 
    {

    }

    public string SkillEffectAnimName()
    {
        return "TimingSkill";
    }

    public void SkillEnd()
    {
        _skillObj.SetActive(false);
        foreach (Transform c in _notesBackInsObj.transform) 
        {
            Destroy(c.gameObject);
        }
    }

    public float SkillResult()
    {
        return 1;
    }

    public async UniTask StartSkill()
    {
        _skillObj.SetActive(true);
        for (int i = 0; i < _arrowNum; i++) 
        {
            GameObject Obj = Instantiate(_arrowPrefab);
            GameObject obj2 = Instantiate(_arrowPrefab);
            Obj.transform.SetParent(_notesBackInsObj.transform);
            obj2.transform.SetParent(_notesInsObj.transform);
            var timingCs = Obj.GetComponent<TimingArrowScript>();
            timingCs.SetSuccessRange(_successRange);
            await timingCs.StartEffect();
        }
    }
}
