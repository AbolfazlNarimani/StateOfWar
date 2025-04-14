using System;
using Unit;
using Unity.VisualScripting;
using UnityEngine;

namespace Ui.ActionBusyUI
{
    public class ActionBusyUI : MonoBehaviour
    {

        private void Start()
        {
            UnitActionSystem.Instance.OnBusyChanged += OnBusyStateChanged;
            HideBusy();
        }

        private void OnBusyStateChanged(object sender, bool e)
        {
            if (e)
            {
                ShowBusy();
            }
            else
            {
                HideBusy();
            }
        }

        private void ShowBusy()
        {
            gameObject.SetActive(true);
        }

        private void HideBusy()
        {
            gameObject.SetActive(false);
        }
    }
}