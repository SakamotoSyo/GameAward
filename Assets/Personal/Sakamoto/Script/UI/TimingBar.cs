using System;
using UnityEngine;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using Cysharp.Threading.Tasks;

public class TimingBar : MonoBehaviour, ISkill
{
    [SerializeField] private RectTransform _timingTransform;
    [Tooltip("�^�C�~���O�o�[�̐ݒ肷��ő�l")]
    [SerializeField] private GameObject _timeObj;
    [Tooltip("���������Ƃ��̔{��")]
    [SerializeField] private float _successRate = 1.1f;
    [Tooltip("UI�̃^�C�~���O�o�[�̒���")]
    [SerializeField] private float _maxTime;
    private float _timingBarWidth;
    [Tooltip("�X�L���������������ǂ���")]
    private bool _isSuccess = false;
    [Tooltip("�X�L�����I������ǂ���")]
    private bool _isSkillFinished = false;
    private float _nowTiming;
    private IDisposable _skillDispose;

    private void Start()
    {
        _timingBarWidth = _timingTransform.sizeDelta.x;
        _maxTime = 100;
    }

    public async UniTask StartSkill()
    {
        _timeObj.SetActive(true);
        DOTween.To(() => 0,
        x =>
        {
            _nowTiming = x; 
            _timingTransform.SetWidth(GetWidth(x));
        },
        _maxTime, 1f).SetLoops(-1);

        _skillDispose = this.UpdateAsObservable()
             .Subscribe(_ => TestButtonEvent()).AddTo(this);
        //�X�L�����I���܂őҋ@����
        await UniTask.WaitUntil(() => _isSkillFinished == true);
    }

    /// <summary>
    /// �X�L�����g�������ʂɂ���Ĕ{����Ԃ�
    /// </summary>
    /// <returns>�U���͂ɂ�����{��</returns>
    public float SkillResult()
    {
        return _isSuccess ? _successRate : 1;
    }

    public void SkillEnd()
    {
        _isSuccess = false;
        _isSkillFinished = false;
        _timeObj.SetActive(false);
        Debug.Log("�w�ǂ��I�����܂�");
        _skillDispose.Dispose();
    }

    /// <summary>
    /// �X�L���������������ǂ������肷��
    /// </summary>
    public void TestButtonEvent()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (90 <= _nowTiming)
            {
                _isSuccess = true;
                Debug.Log($"����{_nowTiming}de{GetWidth(90)}");
            }
            else
            {
                Debug.Log($"���s{_nowTiming}��{_maxTime}");
            }

            _isSkillFinished = true;
        }

    }

    protected float GetWidth(float value)
    {
        float width = Mathf.InverseLerp(0, _maxTime, value);
        return Mathf.Lerp(0, _timingBarWidth, width);
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}

public static class UIExtensions
{
    /// <summary>
    /// ���݂̒l��Rect�ɃZ�b�g����
    /// </summary>
    /// <param name="width"></param>
    public static void SetWidth(this RectTransform rect, float width)
    {
        Vector2 s = rect.sizeDelta;
        s.x = width;
        rect.sizeDelta = s;
    }
}