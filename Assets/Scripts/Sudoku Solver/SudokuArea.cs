using UnityEngine;

namespace Sudoku_Solver
{
    public class SudokuArea : MonoBehaviour
    {
        [SerializeField] private SudokuBox boxPrefab;

        public const int RowCount = 9;
        public const int ColumnCount = 9;
        public const int BoxCount = RowCount * ColumnCount;

        private static SudokuBox[,] sudokuBoxes;
        public static SudokuBox[,] SudokuBoxes => sudokuBoxes;


        private void Start()
        {
            Create();
        }

        private void Create()
        {
            sudokuBoxes = new SudokuBox[RowCount, ColumnCount];

            for (int x = 0; x < ColumnCount; x++)
            {
                for (int y = 0; y < RowCount; y++)
                {
                    var newBox = Instantiate(boxPrefab, this.transform);
                    newBox.name = $"({x},{y})";
                    newBox.SetPosition(x, y);
                    sudokuBoxes[x, y] = newBox;
                }
            }
        }
    }
}
