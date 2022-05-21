using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sudoku_Solver
{
    public class SudokuBox : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private Image image;

        [Header("Sprites")]
        [SerializeField] private Sprite selected;
        [SerializeField] private Sprite notSelected;
    
        [Header("Text Colors")]
        [SerializeField] private Color inputColor;
        [SerializeField] private Color solvedColor;
    
        public const int NullValue = -1;

        private int value = NullValue;
        public int Value => value;

        public int x { get; private set; }
        public int y { get; private set; }


        private void OnEnable()
        {
            inputField.onSelect.AddListener(OnSelected);
            inputField.onDeselect.AddListener(OnDeselected);
        }

        private void OnDisable()
        {
            inputField.onSelect.RemoveAllListeners();
            inputField.onDeselect.RemoveAllListeners();
        }

        public void OnInputChanged()
        {
            if (inputField.text is "0" or "")
            {
                inputField.text = "";
                value = NullValue;
                return;
            }
        
            value = int.Parse(inputField.text);

            inputField.textComponent.color = inputColor;
        }

        public void SetPosition(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public void SetValue(int newValue)
        {
            this.value = newValue;
            inputField.text = newValue.ToString();

            inputField.textComponent.color = solvedColor;
        }

        public void Clear()
        {
            SetValue(0);
        }

        private void OnSelected(string content)
        {
            image.sprite = selected;
        }

        private void OnDeselected(string content)
        {
            image.sprite = notSelected;
        }
    }
}
