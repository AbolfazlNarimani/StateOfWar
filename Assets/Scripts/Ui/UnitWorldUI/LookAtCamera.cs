using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ui.UnitWorldUI
{
    public class LookAtCamera : MonoBehaviour
    {
       private Transform cameraTransform;
       [SerializeField] private bool invert;

       private void Awake()
       {
           cameraTransform = Camera.main.transform;
       }

       private void LateUpdate()
       {
           if (invert)
           {
               Vector3 direction = (cameraTransform.position - transform.position).normalized;
               transform.LookAt(cameraTransform.position, direction * -1);
           }
           else
           {
               transform.LookAt(cameraTransform);
           }
       }
    }
}
