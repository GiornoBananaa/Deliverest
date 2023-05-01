using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private float _lossHeight;
    [SerializeField] private float _jumpForce;
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
        {
            //_rigidbody2D.AddForce(_jumpForce, ForceMode2D.Impulse);
        }

        if (_body.transform.localPosition.y < _lossHeight && !_isFallingForLoss)
            StartCoroutine(FallLoss());
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
