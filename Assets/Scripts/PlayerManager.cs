using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private float _lossHeight;
    [SerializeField] private float _jumpForce;
    [SerializeField] private ArmController _leftArmController;
    [SerializeField] private ArmController _rightArmController;
    [SerializeField] private GameObject _body;

    private Rigidbody2D _rigidbody2D;
    private AudioSource _audioSource;
    private bool _isFallingForLoss;

    void Start()
    {
        _isFallingForLoss = false;
        _rigidbody2D = _body.GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        if (_body.transform.localPosition.y < _lossHeight && !_isFallingForLoss)
            StartCoroutine(FallLoss());
    }

    private void Jump()
    {
        if(!_leftArmController.IsHooked || !_rightArmController.IsHooked)
            return;

        _leftArmController.Fall(true);
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - (Vector2)_body.transform.position).normalized;

        _rigidbody2D.AddForce(direction * _jumpForce, ForceMode2D.Impulse);
    }

    private IEnumerator FallLoss()
    {
        _isFallingForLoss = true;
        _audioSource.Play();
        while (_audioSource.isPlaying)
        {
            yield return new WaitForFixedUpdate();
        }

        GameManager.instance.LoseGame(false);
    }
}
