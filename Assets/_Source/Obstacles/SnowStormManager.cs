using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowStormManager : MonoBehaviour
{
    public bool IsStorm { get; private set; }
    public Vector2 Velocity { get; private set; }

    [SerializeField] private GameObject _stormPrefab;
    [SerializeField] private GameObject _dangersSign;
    [SerializeField] private Transform _spawnPosition;
    [SerializeField] private Vector2 _spawnOffset;

    private float _timeForNextStorm;
    private float _durationOfNextStorm;
    private bool _stormIsShowed;
    private bool _isStormCoroutine;
    private float _stormVolume;
    private SpriteRenderer _stormRender;
    private AudioSource _stormSound;
    private Level level;

    void Start()
    {
        if (!GameManager.instance.currentLevel.snow_storm) gameObject.SetActive(false);

        level = GameManager.instance.currentLevel;
        _isStormCoroutine = false;
        _stormIsShowed = false;
        IsStorm = false;
        _timeForNextStorm = Random.Range(level.stormMinDelay, level.stormMaxDelay) -10;
        _durationOfNextStorm = Random.Range(level.stormMinDuration, level.stormMaxDuration);
    }

    void Update()
    {
        _timeForNextStorm -= Time.deltaTime;
        if (!_isStormCoroutine && !IsStorm && _timeForNextStorm <= 0)
        {
            _durationOfNextStorm = Random.Range(level.stormMinDuration, level.stormMaxDuration);
            _timeForNextStorm = Random.Range(level.stormMinDelay, level.stormMaxDelay);
            StartCoroutine(Storm());
        }
    }

    private void FixedUpdate()
    {
        if (IsStorm && !_stormIsShowed)
        {
            float alpha = Mathf.Lerp(_stormRender.color.a, 1, level.appearSpeed);
            _stormRender.color = new Color(1, 1, 1, alpha);

            _stormSound.volume = Mathf.Lerp(_stormSound.volume, _stormVolume, level.appearSpeed);

            if (_stormRender.color.a > 0.7f)
            {
                _stormIsShowed = true;
            }
        }
        else if (!IsStorm && _stormIsShowed)
        {
            float alpha = Mathf.Lerp(_stormRender.color.a, 0, level.appearSpeed);
            _stormRender.color = new Color(1, 1, 1, alpha);

            _stormSound.volume = Mathf.Lerp(_stormSound.volume, 0, level.appearSpeed);

            if (_stormRender.color.a < 0.01f)
            {
                _stormIsShowed = false;
                Destroy(_stormRender.gameObject);
            }
        }
    }
    
    private IEnumerator Storm()
    {
        GameManager.instance.isSnowStorm = true;
        _isStormCoroutine = true;
        _dangersSign.SetActive(true);

        yield return new WaitForSeconds(level.signTime);

        Velocity = new Vector2(Random.value < 0.5f ? -level.stormSpeed : level.stormSpeed, 0);
        _stormRender = Instantiate(_stormPrefab, (Vector2)_spawnPosition.position + (Velocity.x > 0 ? -_spawnOffset : _spawnOffset), Quaternion.identity).GetComponent<SpriteRenderer>();
        _stormRender.transform.SetParent(_spawnPosition);
        _stormRender.color = new Color(1, 1, 1, 0);
        _stormRender.GetComponent<SnowStorm>().Velocity = Velocity;
        _stormSound = _stormRender.GetComponent<AudioSource>();
        _stormSound.volume = GameManager.AudoPlayer.SoundVolume * _stormSound.volume;
        _stormVolume = _stormSound.volume;
        _stormSound.volume = 0;

        IsStorm = true;

        yield return new WaitForSeconds(_durationOfNextStorm);

        IsStorm = false;
        _isStormCoroutine = false;
        _dangersSign.SetActive(false);
        GameManager.instance.isSnowStorm = false;
    }
}
