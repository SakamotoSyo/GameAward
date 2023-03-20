using UnityEngine;

public class SkillDelay : MonoBehaviour
{
    [Tooltip("Time.timeScale���ǂꂭ�炢�����邩")]
    [Range(0.1f, 1f)]
    [SerializeField] private float _delayScale = 0.1f;
    [SerializeField] private float _delayTime = 1f;
    [SerializeField] private SlowAnim _slow = default;

    //private Rigidbody2D _rb2d = default;
    private float _delaying = 0f;
    private bool _isDelay = false;

    private void Start()
    {
        //RigidBody.Interpolate��Interpolate�ɂ��邱�ƂŁA
        //�X���[���̕������Z�����炩�ɍs��
        //_rb2d = GetComponent<Rigidbody2D>();
        //_rb2d.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    private void Update()
    {
        //�ȉ��e�X�g
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!_slow.IsDelay)
                _slow.DelayAnimation();
        }

        if (_isDelay)
        {
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

    /// <summary> ���ɖ߂� </summary>
    private void DelayReset()
    {
        Time.timeScale = 1f;
        Debug.Log("delay reset");
    }
}
