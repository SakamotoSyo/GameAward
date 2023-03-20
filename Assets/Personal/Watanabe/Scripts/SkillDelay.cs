using UnityEngine;

public class SkillDelay : MonoBehaviour
{
    [Tooltip("Time.timeScale���ǂꂭ�炢�����邩")]
    [Range(0.1f, 1f)]
    [SerializeField] private float _delayScale = 0.1f;

    /// <summary> �X���[���[�V���� </summary>
    public void Delay()
    {
        //FixedUpdate()��Time.timeScale�̉e�����󂯂�
        //Update()��Time.timeScale�̉e�����󂯂Ȃ�
        Time.timeScale = _delayScale;
        Debug.LogFormat("delay");
    }

    /// <summary> ���ɖ߂� </summary>
    public void DelayReset()
    {
        Time.timeScale = 1f;
        Debug.LogFormat("delay reset");
    }
}
