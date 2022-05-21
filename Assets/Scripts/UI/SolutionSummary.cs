using Sudoku_Solver;
using TMPro;
using UnityEngine;

namespace UI
{
    public class SolutionSummary : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textField;


        private void OnEnable()
        {
            SudokuSolver.onSolutionEnd += OnSummary;
        }

        private void OnDisable()
        {
            SudokuSolver.onSolutionEnd -= OnSummary;
        }

        private void OnSummary(string message)
        {
            textField.text = message;
        }

        public void Clear()
        {
            textField.text = "";
        }
    }
}
