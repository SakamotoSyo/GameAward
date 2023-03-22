using UnityEngine;

public class SkillDelay : MonoBehaviour
{
    [Tooltip("Time.timeScale���ǂꂭ�炢�����邩")]
    [Range(0.1f, 1f)]
    [SerializeField] private float _delayScale = 0.1f;
    [Tooltip("Delay�̎��s����")]
    [SerializeField] private float _delayTime = 1f;
    [Tooltip("Animation�̍Đ���x�点��(�e�X�g)")]
    [SerializeField] private SlowAnim _slow = default;

    private float _delaying = 0f;
    private bool _isDelay = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Animation��P�̂�Delay������e�X�g
            if (_slow != null)
            {
                if (!_slow.IsDelay)
                    _slow.DelayAnimation();
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
            Delay();

        if (_isDelay)
        {
            //��莞�Ԍo������Delay����
            _delaying += Time.unscaledDeltaTime;
            if (_delaying >= _delayTime)
            {
                DelayReset();
                _isDelay = false;
            }
        }
    }

    /// <summary> �X���[���[�V���� </summary>
    public void Delay()
    {
        _delaying = 0f;
        //FixedUpdate()��Time.timeScale�̉e�����󂯂�
        //Update()��Time.timeScale�̉e�����󂯂Ȃ�
        Time.timeScale = _delayScale;
        _isDelay = true;
        Debug.Log("delay");
    }

    /// <summary> Delay�����ɖ߂� </summary>
    private void DelayReset()
    {
        Time.timeScale = 1f;
        Debug.Log("delay reset");
    }
}
