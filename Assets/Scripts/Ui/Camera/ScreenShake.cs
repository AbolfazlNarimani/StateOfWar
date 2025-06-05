using System;
using Unity.Cinemachine;
using UnityEngine;

namespace Ui.Camera
{
    public class ScreenShake : MonoBehaviour
    {
        private CinemachineImpulseSource _cinemachineImpulseSource;
        public static ScreenShake Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            _cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
        }

        public void Shake(float intensity = 1f)
        {
            _cinemachineImpulseSource.GenerateImpulse(intensity);
        }
    }
}