using UnityEngine;
using UnityEngine.UI;

public class EnemyView : MonoBehaviour
{
    [SerializeField, Tooltip("Helth��\������e�L�X�g")]
    private Text _healthText;

    /// <summary>
    /// �G�l�~�[�̎c��̗͂��e�L�X�g�ɕ\������
    /// </summary>
    public void HealthText(float health)
    {
        _healthText.text = health.ToString("00");
    }
}
