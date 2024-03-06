using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace Obstacles
{
    public class ObstacleOutliner : MonoBehaviour
    {
        [SerializeField] private Material _outlineMaterial;
        [SerializeField] private LayerMask _outlineObjectLayer;
        private GameObject _outlinedObject;
        private Renderer _outlinedRender;
        private Material _outlinedObjectDefaultMaterial;
        
        private void Update()
        {
            CheckForObstacle();
        }

        public void CheckForObstacle()
        {
            RaycastHit2D rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition),100,_outlineObjectLayer);
            if (rayHit.collider!=null)
            {
                if(_outlinedObject == rayHit.collider.gameObject) return;
                _outlinedObject = rayHit.collider.gameObject;
                _outlinedRender = rayHit.collider.gameObject.GetComponent<Renderer>();
                if(_outlinedRender == null)
                    _outlinedRender = rayHit.collider.gameObject.GetComponentInParent<Renderer>();
                _outlinedObjectDefaultMaterial = _outlinedRender.material;
                Debug.Log(_outlinedObjectDefaultMaterial);
                _outlinedRender.material = _outlineMaterial;
            }
            else if (_outlinedRender != null)
            {
                _outlinedRender.material = _outlinedObjectDefaultMaterial;
                _outlinedRender = null;
                _outlinedObject = null;
            }
            if(rayHit.collider != null)Debug.Log(rayHit.collider.gameObject.name);
        }
    }
}
