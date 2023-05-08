using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private float _lossHeight;
    [SerializeField] private float _lossY;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _jumpReloadTime;
    [SerializeField] private float _fallingForLossTime;
    [SerializeField] private ArmController _leftArmController;
    [SerializeField] private ArmController _rightArmController;
    [SerializeField] private GameObject _body;
    [SerializeField] private GameObject _restartButton;

    private Rigidbody2D _rigidbody2D;
    private AudioSource _audioSource;
    private bool _isFallingForLoss;
    private float _timeForNextJump;

    public float NormilizedJumpReloadTime { get => _timeForNextJump / _jumpReloadTime; }
    public bool IsOnOneHand { get => (_leftArmController.IsHooked && !_rightArmController.IsHooked) || (!_leftArmController.IsHooked && _rightArmController.IsHooked); }
    public bool IsOnTwoHands { get => (_leftArmController.IsHooked && _rightArmController.IsHooked); }

    void Start()
    {
        _timeForNextJump = _jumpReloadTime;
        _isFallingForLoss = false;
        _rigidbody2D = _body.GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        _timeForNextJump += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        if ((_body.transform.position.y <= _lossHeight || _body.transform.localPosition.x <= -_lossY || _body.transform.localPosition.x >= _lossY) && !_isFallingForLoss)
            StartCoroutine(FallLoss());
    }

    private void Jump()
    {
        if(!_leftArmController.IsHooked || !_rightArmController.IsHooked || _timeForNextJump < _jumpReloadTime)
            return;

        _leftArmController.Fall(true);
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - (Vector2)_body.transform.position).normalized;

        _rigidbody2D.AddForce(direction * _jumpForce, ForceMode2D.Impulse);

        _timeForNextJump = 0;
    }

    private IEnumerator FallLoss()
    {
        _restartButton.SetActive(true);
        _isFallingForLoss = true;
        _audioSource.Play();

        float fallingTime = 0;
        while (fallingTime< _fallingForLossTime)
        {
            fallingTime += Time.deltaTime;
            _audioSource.volume = Mathf.Lerp(_audioSource.volume, 0, 0.005f);
            yield return new WaitForEndOfFrame();
        }
        /*
        while (_audioSource.isPlaying)
        {
            yield return new WaitForFixedUpdate();
        }*/
        _restartButton.SetActive(false);
        GameManager.instance.LoseGame(false);
    }
}
