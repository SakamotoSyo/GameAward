using UnityEngine;
using UniRx;

public class EnemyController : MonoBehaviour, IAddDamage
{
    [SerializeField, Tooltip("�ő�")]
    private float _maxHealth = 5f;

    private ReactiveProperty<float> _health = new();


    private void Start()
    {
        _health.Value = _maxHealth;
        HealthObserver();
    }

    /// <summary>
    /// health�̒l���Ď����A0�ȉ��ɂȂ�������Subscribe����
    /// </summary>
    private void HealthObserver()
    {
        _health
            .Where(health => health <= 0)
            .Subscribe(_ =>
            {
                Debug.Log("Enemy�̗̑͂�0�ɂȂ���");
                gameObject.SetActive(false);
            })
            .AddTo(this);
    }

    public void AddDamage(float damage)
    {
        _health.Value -= damage;
    }
}
