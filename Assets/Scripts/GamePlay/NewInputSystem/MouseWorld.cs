using UnityEngine;

namespace NewInputSystem
{
    public class MouseWorld : MonoBehaviour
    {
        private static MouseWorld _instance;
        
        [SerializeField] private LayerMask mousePlaneLayerMask;

        private void Awake()
        {
            _instance = this;
        }
    
        public static Vector3 GetMouseWorldPosition()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out RaycastHit hit, maxDistance: float.MaxValue ,layerMask:  _instance.mousePlaneLayerMask);
        
            return hit.point;
        }
    }
}