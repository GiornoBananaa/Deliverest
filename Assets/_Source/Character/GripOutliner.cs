using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class GripOutliner : MonoBehaviour
    {
        private class OutlinedObjectData
        {
            public GameObject GameObject;
            public Renderer Render;
            public Material DefaultMaterial;
            public int RayCastsCount;
        }
        
        [SerializeField] private Material _outlineMaterial;
        [SerializeField] private LayerMask _outlineObjectLayer;
        [SerializeField] private Transform[] _rayCastPoints;
        
        private Dictionary<GameObject,OutlinedObjectData> _outlinedObjectsData;
        private Dictionary<int,OutlinedObjectData> _raycasterOutlinedObjects;
        private Camera _camera;

        private void Start()
        {
            _outlinedObjectsData = new Dictionary<GameObject, OutlinedObjectData>();
            _raycasterOutlinedObjects = new Dictionary<int, OutlinedObjectData>();
            for (int i = 0; i < _rayCastPoints.Length+1; i++)
            {
                _raycasterOutlinedObjects.Add(i, null);
            }
            _camera = Camera.main;
        }

        private void Update()
        {
            CheckForObstacle();
        }
        
        private void CheckForObstacle()
        {
            RaycastHit2D rayHit = Physics2D.GetRayIntersection(_camera.ScreenPointToRay(Input.mousePosition),100,_outlineObjectLayer);
            SwitchObjectOutline(rayHit,0);
            for (int i = 0; i < _rayCastPoints.Length; i++)
            {
                RaycastHit2D pointRayHit = Physics2D.Raycast(_rayCastPoints[i].position,Vector2.zero,0,_outlineObjectLayer);
                SwitchObjectOutline(pointRayHit,i+1);
            }
        }

        private void SwitchObjectOutline(RaycastHit2D rayHit, int raycasterIndex)
        {
            if (_raycasterOutlinedObjects[raycasterIndex]!=null)
            {
                if(rayHit.collider != null && _raycasterOutlinedObjects[raycasterIndex].GameObject == rayHit.collider.gameObject)
                    return;
                DisableOutline(_raycasterOutlinedObjects[raycasterIndex].GameObject);
                _raycasterOutlinedObjects[raycasterIndex] = null;
            }
            if (rayHit.collider != null)
            {
                _raycasterOutlinedObjects[raycasterIndex] = EnableOutline(rayHit.collider.gameObject);
            }
        }

        private OutlinedObjectData EnableOutline(GameObject outlinedObject)
        {
            if(_outlinedObjectsData.TryGetValue(outlinedObject, out OutlinedObjectData value))
            {
                value.RayCastsCount += 1;
                return value;
            }
            
            OutlinedObjectData outlinedObjectData = new OutlinedObjectData();
            outlinedObjectData.RayCastsCount += 1;
            outlinedObjectData.GameObject = outlinedObject;
            outlinedObjectData.Render = outlinedObject.GetComponent<Renderer>();
            if(outlinedObjectData.Render == null)
                outlinedObjectData.Render = outlinedObject.GetComponentInParent<Renderer>();
            outlinedObjectData.DefaultMaterial = outlinedObjectData.Render.material;
            outlinedObjectData.Render.material = _outlineMaterial;
            _outlinedObjectsData.Add(outlinedObject,outlinedObjectData);
            return outlinedObjectData;
        }
        
        private void DisableOutline(GameObject outlinedObject)
        {
            OutlinedObjectData outlinedObjectData = _outlinedObjectsData[outlinedObject];
            
            outlinedObjectData.RayCastsCount -= 1;
            
            if (outlinedObjectData.RayCastsCount > 0)
            {
                return;
            }
            outlinedObjectData.Render.material = outlinedObjectData.DefaultMaterial;
            _outlinedObjectsData.Remove(outlinedObject);
        }
    }
}
