using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class TimingArrowScript : MonoBehaviour
{
    [SerializeField] private GameObject _imageObj;
    [Tooltip("矢印の方向")]
    [SerializeField] private string _inputName;
    private Tween _tween;
    private float _successRange;
    private float _nowNotes;
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _tween != null)
        {
            _tween.Kill();
            _tween = null;
            var enemyObj = GameObject.FindGameObjectsWithTag("Enemy");
            if (enemyObj[0].TryGetComponent<IAddDamage>(out IAddDamage enemy)
                && (0.8 - _successRange < _nowNotes && _nowNotes < 0.8 + _successRange))
            {
                enemy.AddDamage(10);
                Debug.Log("成功");
            }
            else 
            {

            }
        }
    }

    public async UniTask StartEffect() 
    {

       _tween = DOTween.To(() => new Vector3(1.5f, 1.5f, 1.5f),
                  x => 
                  {
                      transform.localScale = x;
                      _nowNotes = x.x;
                  },
                  new Vector3(0.7f, 0.7f, 0.7f), 2f)
                  .OnComplete(() => 
                  {
                      _tween = null;
                      //MissってText表示したい
                  } );
        await _tween;
    }

    public void SetSuccessRange(float range) 
    {
        _successRange = range;
    }
}
