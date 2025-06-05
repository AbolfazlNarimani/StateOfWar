using System;
using GamePlay.ActionSystem.BaseShootAction.ShootAction;
using UnityEngine;

namespace Ui.Camera
{
    public class ScreenShakeActions : MonoBehaviour
    {
        private void Start()
        {
            ShootAction.OnAnyShoot += ShootActionOnAnyShoot;
        }

        private void ShootActionOnAnyShoot(object sender, EventArgs e)
        {
            ScreenShake.Instance.Shake();
        }
    }
}
