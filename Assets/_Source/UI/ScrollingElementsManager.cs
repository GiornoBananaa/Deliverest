using UnityEngine;
using UnityEngine.SceneManagement;

public class ScrollingElementsManager : MonoBehaviour
{
    [SerializeField] private Transform _panel;
    [SerializeField] private float _leftBorderX;
    [SerializeField] private float _rightBorderX;

    private RectTransform _rectTransform;
    private float _offset;
    private float _leftborderRatio;
    private float _rightborderRatio;
    private float _lastScreenWidth;

    private void Start()
    {
        _offset = 550;
        _leftborderRatio = 1920f / _leftBorderX;
        _rightborderRatio = 1920f / _rightBorderX;
        _leftBorderX = Screen.width / _leftborderRatio;
        _rightBorderX = Screen.width / _rightborderRatio;
        _lastScreenWidth = Screen.width;

        _rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (_lastScreenWidth != Screen.width)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        if (_rectTransform.position.x > _rightBorderX)
            MoveAuthor(true);

        else if(_rectTransform.position.x < _leftBorderX)
            MoveAuthor(false);
    }
    
    public void MoveAuthor(bool isRight)
    {
        if (isRight)
        {
            _rectTransform.localPosition = _panel.GetChild(0).localPosition + new Vector3(-_offset, 0,0);
            _rectTransform.SetAsFirstSibling();
        }
        else
        {
            _rectTransform.localPosition = _panel.GetChild(_panel.childCount-1).localPosition + new Vector3(_offset, 0, 0);
            _rectTransform.SetAsLastSibling();
        }
    }
}
