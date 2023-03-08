using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TimingBar : MonoBehaviour
{
    [Tooltip("�^�C�~���O�o�[�Œ��̒���")]
    private float _timingBarWidth;
    [Tooltip("�^�C�~���O�o�[�̍ő�l")]
    private float _maxTime;
    [SerializeField] private RectTransform _timingTransform;

    private void Start()
    {
        _timingBarWidth = _timingTransform.sizeDelta.x;
        _maxTime = 100;
        DOTween.To(() => 0,
            x => _timingTransform.SetWidth(GetWidth(x)),
            _maxTime, 1f).SetLoops(-1);
    }

    public void TestButtonEvent() 
    {
        Debug.Log(GetWidth(_timingTransform.sizeDelta.x));
    }

    protected float GetWidth(float value)
    {
        float width = Mathf.InverseLerp(0, _maxTime, value);
        return Mathf.Lerp(0, _timingBarWidth, width);
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