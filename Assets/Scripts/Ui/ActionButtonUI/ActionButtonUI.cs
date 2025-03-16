using NewInputSystem.ActionSystem.BaseAction;
using TMPro;
using Unit;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.ActionButtonUI
{
    public class ActionButtonUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textMesh;
        [SerializeField] private Button button;
        [SerializeField] private Image image;
        [SerializeField] private GameObject selectedGameObject;
        
        private BaseAction _baseAction;

        public void SetBaseAction(BaseAction baseAction)
        {
            this._baseAction = baseAction;
            textMesh.text = baseAction.GetActionName().ToUpper();
            image.sprite = baseAction.GetActionIcon();
            button.onClick.AddListener(() =>
            {
                //lambda expression (Anonymous function)
                UnitActionSystem.Instance.SetSelectedAction(baseAction);
            });
        }

        public void UpdateSelectedVisual()
        {
            BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();
            selectedGameObject.SetActive(selectedAction == _baseAction);
        }
    }
}