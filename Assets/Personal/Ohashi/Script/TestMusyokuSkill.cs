using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TestMusyokuSkill : MonoBehaviour
{
    [SerializeField, Tooltip("�t�F�[�h�p�̃p�l��")]
    private Image _fadePanel;

    [SerializeField]
    private EnemyController _enemyController;

    /// <summary>
    /// ���E�]���̃X�L��
    /// </summary>
    public void Skill()
    {
        _fadePanel.enabled = true;
        var sequence = DOTween.Sequence();
        //�t�F�[�h�A�E�g
        sequence.Append(_fadePanel.DOFade(1f, 1f)
            .OnComplete(() => _fadePanel.color = new Color(0, 0, 0, 0)));
        //�v���C���[�𓮂���
        sequence.Append(transform.DOMoveX(5.5f, 0.5f));
        //�҂�
        sequence.AppendInterval(1f);
        //�G�̃_���[�W�̃��\�b�h���Ă�
        sequence.AppendCallback(() => _enemyController.AddDamage(10000));
        //�҂�
        sequence.AppendInterval(1f);
        //���Ƃ̈ʒu�ɖ߂�
        sequence.Append(transform.DOMoveX(-5.5f, 0.5f));
        //�t�F�[�h�̃p�l�����A�N�e�B�u�ɂ���
        sequence.AppendCallback(() => _fadePanel.enabled = false);
    }
}