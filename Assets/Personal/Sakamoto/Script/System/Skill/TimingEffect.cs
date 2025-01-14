using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingEffect :MonoBehaviour, ISkill
{
    [Tooltip("出す矢印の数")]
    [SerializeField] private int _arrowNum;
    [Tooltip("SKillに使うObj")]
    [SerializeField] private GameObject _skillObj;
    [Tooltip("Notesの背景を生成する場所")]
    [SerializeField] private GameObject _notesBackInsObj;    
    [Tooltip("Notesを生成する場所")]
    [SerializeField] private GameObject _notesInsObj;
    [Tooltip("矢印のPrefab")]
    [SerializeField] private GameObject _arrowPrefab;
    [SerializeField] private WeaponStatus _weaponStatus;
    [SerializeField] private ParticleSystem _skillParticle = null;
    [Tooltip("1がぴったりだとしてどの程度の範囲を成功とするか")]
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
        foreach (Transform c in _notesInsObj.transform) 
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
            timingCs.Init(_successRange, _weaponStatus);
            _skillParticle.Play();
            await timingCs.StartEffect();
            _skillParticle.Stop();
        }
    }
}
