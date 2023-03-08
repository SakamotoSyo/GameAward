using UnityEngine;
using UniRx;

public class EnemyPresenter : MonoBehaviour
{
    [SerializeField, Tooltip("���f��")]
    private EnemyController _enemyModel;

    [SerializeField, Tooltip("�G�l�~�[��view")]
    private EnemyView _enemyView;

    private void Start()
    {
        EnemyHealthObserver();
    }

    /// <summary>
    /// Halth�̒l���Ď����A�ύX���ꂽ�Ƃ�Subscribe����
    /// </summary>
    private void EnemyHealthObserver()
    {
        _enemyModel.Health
            .Subscribe(health => _enemyView.HealthText(health))
            .AddTo(this);
    }

}
