using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowStormManager : MonoBehaviour
{
    public bool IsStorm { get; private set; }
    public Vector2 Velocity { get; private set; }

    [SerializeField] private float _stormMinDelay, _stormMaxDelay;
    [SerializeField] private float _stormMinDuration, _stormMaxDuration;
    [SerializeField] private float _appearSpeed;
    [SerializeField] private float _stormSpeed;
    [SerializeField] private float _signTime;
    [SerializeField] private GameObject _stormPrefab;
    [SerializeField] private Transform _spawnPosition;
    [SerializeField] private Vector2 _spawnOffset;

    private float _timeForNextStorm;
    private float _durationOfNextStorm;
    private bool _stormIsShowed;
    private bool _isStormCoroutine;
    private SpriteRenderer _stormRender;

    void Start()
    {
        _isStormCoroutine = false;
        _stormIsShowed = false;
        IsStorm = false;
        _timeForNextStorm = Random.Range(_stormMinDelay, _stormMaxDelay)-10;
        _durationOfNextStorm = Random.Range(_stormMinDuration, _stormMaxDuration);
    }

    void Update()
    {
        _timeForNextStorm -= Time.deltaTime;
        if (!_isStormCoroutine && !IsStorm && _timeForNextStorm <= 0)
        {
            _durationOfNextStorm = Random.Range(_stormMinDuration, _stormMaxDuration);
            _timeForNextStorm = Random.Range(_stormMinDelay, _stormMaxDelay);
            StartCoroutine(Storm());
        }
    }

    private void FixedUpdate()
    {
        if (IsStorm && !_stormIsShowed)
        {
            float alpha = Mathf.Lerp(_stormRender.color.a, 1, _appearSpeed);
            _stormRender.color = new Color(1, 1, 1, alpha);
            if (_stormRender.color.a > 0.7f)
            {
                _stormIsShowed = true;
            }
        }
        else if (!IsStorm && _stormIsShowed)
        {
            float alpha = Mathf.Lerp(_stormRender.color.a, 0, _appearSpeed);
            _stormRender.color = new Color(1, 1, 1, alpha);

            if (_stormRender.color.a < 0.01f)
            {
                _stormIsShowed = false;
                Destroy(_stormRender);
            }
        }
    }

    private IEnumerator Storm()
    {
        _isStormCoroutine = true;
        Velocity = new Vector2(Random.value < 0.5f ? -_stormSpeed : _stormSpeed, 0);
        _stormRender = Instantiate(_stormPrefab, (Vector2)_spawnPosition.position + (Velocity.x > 0 ? -_spawnOffset : _spawnOffset), Quaternion.identity).GetComponent<SpriteRenderer>();
        _stormRender.color = new Color(1, 1, 1, 0);
        _stormRender.GetComponent<SnowStorm>().Velocity = Velocity;

        IsStorm = true;

        yield return new WaitForSeconds(_durationOfNextStorm);

        IsStorm = false;
        _isStormCoroutine = false;
    }
}