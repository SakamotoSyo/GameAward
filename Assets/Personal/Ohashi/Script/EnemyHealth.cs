using UnityEngine;
using DG.Tweening;
using UniRx;

[System.Serializable]
public class EnemyHealth
{
    [SerializeField, Tooltip("�}�b�N�X����Health")]
    private float _maxHealth = 5f;

    public float MaxHealth => _maxHealth;

    private ReactiveProperty<float> _health = new();

    public IReadOnlyReactiveProperty<float> Health => _health;

    private SpriteRenderer _renderer;

    private Animator _animator;

    private GameObject _enemy;

    /// <summary>
    /// ������
    /// </summary>
    public void Init(SpriteRenderer renderer,
        Animator animator,
        GameObject enemy)
    {
        _health.Value = _maxHealth;
        _renderer = renderer;
        _animator = animator;
        _enemy = enemy;
    }

    /// <summary>
    /// �_���[�W���󂯂����̃A�j���[�V����
    /// </summary>
    public void DamageAnimation()
    {
        //_animator.Play("Damage");
        _renderer.DOColor(Color.red, 0.3f)
            .OnComplete(() => _renderer.DOColor(Color.white, 0.3f));
    }

    /// <summary>
    /// �_���[�W���󂯁A�̗͂�0�ɂȂ�����G���A�N�e�B�u�ɂ���
    /// </summary>
    public void Damage(int damage)
    {
        _health.Value -= damage;

        if (_health.Value <= 0)
        {
            Debug.Log("Enemy�̗̑͂�0�ɂȂ���");
            _enemy.SetActive(false);
        }
    }
}
