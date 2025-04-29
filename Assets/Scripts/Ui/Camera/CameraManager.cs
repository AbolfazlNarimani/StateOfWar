using System;
using GamePlay.ActionSystem.BaseAction;
using GamePlay.ActionSystem.BaseShootAction.ShootAction;
using GamePlay.Unit.BaseUnit;
using Unity.Cinemachine;
using UnityEngine;

namespace Ui.Camera
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private GameObject actionCameraGameObject;

        private void Awake()
        {
            HideActionCamera();
        }

        private void Start()
        {
            BaseAction.OnAnyActionStarted += BaseActionOnAnyActionStarted;
            BaseAction.OnAnyActionCompleted += BaseActionOnAnyActionCompleted;
            
        }

        private void BaseActionOnAnyActionCompleted(object sender, EventArgs e)
        {
            switch (sender)
            {
                case ShootAction shootAction:
                    HideActionCamera();
                    Debug.Log("camera disable");
                    break;
            }
        }

        private void BaseActionOnAnyActionStarted(object sender, EventArgs e)
        {
            switch (sender)
            {
                case ShootAction shootAction:
                    
                    BaseUnit shooterUnit = shootAction.GetUnit();
                    BaseUnit targetUnit = shootAction.GetTargetUnit();
                    float shoulderOffsetAmount = .5f;
                    Vector3 shootDir = (targetUnit.GetWorldPosition() - shooterUnit.GetWorldPosition()).normalized;
                    Vector3 cameraCharacterHeight = Vector3.up * 1.7F;
                    Vector3 shoulderOffset = Quaternion.Euler(0f, 90f, 0f) * shootDir * shoulderOffsetAmount;
                    Vector3 actionCameraPosition = shooterUnit.GetWorldPosition() + cameraCharacterHeight + shoulderOffset + (shootDir * -1);
                    actionCameraGameObject.transform.position = actionCameraPosition;
                    actionCameraGameObject.transform.LookAt(targetUnit.GetWorldPosition() + cameraCharacterHeight);
                    ShowActionCamera();
                    Debug.Log("camera enable");
                    break;
            }
        }

        private void ShowActionCamera()
        {
            actionCameraGameObject.SetActive(true);
        }

        private void HideActionCamera()
        {
            actionCameraGameObject.SetActive(false);
        }
    }
}
