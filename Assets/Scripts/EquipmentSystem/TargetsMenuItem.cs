using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EquipmentSystem
{
    public class TargetsMenuItem : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _bindingText;

        public void Initialize(Sprite sprite, string binding)
        {
            _icon.sprite = sprite;
            _bindingText.text = binding;
        }
    }
}