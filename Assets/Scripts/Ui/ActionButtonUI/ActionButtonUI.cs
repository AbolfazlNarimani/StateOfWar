using NewInputSystem.ActionSystem.BaseAction;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.ActionButtonUI
{
    public class ActionButtonUI : MonoBehaviour
    {
   
        [SerializeField] private TextMeshProUGUI textMesh;
        [SerializeField] private Button button;

        public void SetBaseAction(BaseAction baseAction)
        {
            textMesh.text = baseAction.GetActionName().ToUpper();
        }
    }
}
