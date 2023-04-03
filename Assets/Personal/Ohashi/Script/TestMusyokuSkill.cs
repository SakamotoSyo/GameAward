using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TestMusyokuSkill : MonoBehaviour
{
    [SerializeField, Tooltip("�t�F�[�h�p�̃p�l��")]
    private Image _fadePanel;

    [SerializeField]
    private EnemyController _enemyController;

    [SerializeField, Tooltip("�v���C���[����������p�̃X�v���C�g")]
    private SpriteRenderer _blackPlayer;

    [SerializeField, Tooltip("�G����������p�̃X�v���C�g")]
    private SpriteRenderer _blackEnemy;

    [SerializeField, Tooltip("�G�̂ӂ��������p�̃X�v���C�g")]
    private SpriteRenderer _blueEnemy;

    [SerializeField]
    private SpriteRenderer _backGround;

    [SerializeField]
    private ParticleSystem _explosionEffect;

    [SerializeField]
    private ParticleSystem[] _swordEffects;

    [SerializeField, Tooltip("�v���C���[�̍ŏI�n�_")]
    private float _moveX = 8f;

    [SerializeField, Tooltip("�U����C���^�[�o��")]
    private float _appendInterval = 0.2f;

    private int _count = 1;

    /// <summary>
    /// ���E�]���̃X�L��
    /// </summary>
    public void Skill()
    {
        _count = 0;
        _fadePanel.enabled = true;
        var sequence = DOTween.Sequence();
        //�t�F�[�h�A�E�g
        sequence.Append(_fadePanel.DOFade(1f, 1f))
            .Join(transform.DORotate(new Vector3(0, 0, -11), 0.5f));
        //�҂�
        sequence.AppendInterval(0.3f);
        //�S�̂̐F���������ăt�F�[�h����߂�
        sequence.AppendCallback(() =>
        {
            _backGround.color = new Color(0, 0, 0, 0.9f);
            _blackPlayer.enabled = true;
            _blackEnemy.enabled = true;
            _blueEnemy.enabled = true;
            _fadePanel.color = Color.clear;
        });

        sequence.Append(_blackPlayer.DOFade(1, 0))
            .Join(_blackEnemy.DOFade(1, 0))
            .Join(_blueEnemy.DOFade(1, 0));
        //�v���C���[�𓮂���
        sequence.Append(transform.DOMoveX(_moveX, 0.3f));
        //�҂�
        sequence.AppendInterval(0.5f);

        sequence.Append(_blackPlayer.DOFade(0, 1f))
            .Join(_blackEnemy.DOFade(0, 1f))
            .Join(_blueEnemy.DOFade(0, 1f))
            .Join(_backGround.DOFade(0, 1f));
        //�S�̂̐F�����ɖ߂�
        sequence.AppendCallback(() =>
        {
            _blackPlayer.enabled = false;
            _blackEnemy.enabled = false;
            _blueEnemy.enabled = false;
        });
        //�҂�
        //sequence.AppendInterval(_appendInterval);
        //�G�̃_���[�W�̃��\�b�h���Ă�
        sequence.AppendCallback(() =>
        {
            _explosionEffect.Play();
            _enemyController.AddDamage(10000);
        });
        //�҂�
        sequence.AppendInterval(0.5f);
        sequence.Append(transform.DORotate(Vector3.zero, 0.5f));
        //���Ƃ̈ʒu�ɖ߂�
        sequence.Append(transform.DOMoveX(-5.5f, 0.5f));
        //�t�F�[�h�̃p�l�����A�N�e�B�u�ɂ���
        sequence.AppendCallback(() => _fadePanel.enabled = false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�ڐG������a��Effect���o��
        if (collision.gameObject.TryGetComponent<IAddDamage>(out IAddDamage addDamage) && _count == 0)
        {
            _count++;
            foreach (var swordEffect in _swordEffects)
            {
                swordEffect.Play();
            }
        }
    }
}
