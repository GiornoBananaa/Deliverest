using UnityEngine;

namespace CameraSystem
{
    public class CameraTarget : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        private float _lastY;
        
        private void Start()
        {
            _lastY = transform.position.y;
        }
        
        private void Update()
        {
            Follow();
            LimitY();
        }
        
        private void Follow()
        {
            transform.position = _target.position;
        }
        
        private void LimitY()
        {
            var position = transform.position;

            if (position.y < _lastY)
            {
                transform.position = new Vector3(position.x, _lastY, position.z);
            }

            _lastY =  transform.position.y;
        }
    }
}
