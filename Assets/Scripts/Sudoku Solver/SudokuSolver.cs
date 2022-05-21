using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Sudoku_Solver
{
    public class SudokuSolver : MonoBehaviour
    {
        private const int MinValue = 1;
        private const int MaxValue = 9;


        private static int solutionDepth;


        public static event UnityAction<string> onSolutionEnd;
        private void SolutionEnd(string message) => onSolutionEnd?.Invoke(message);
    
        public UnityEvent onCleared;
        private void Cleared() => onCleared?.Invoke();

        public void Solve()
        {
            solutionDepth = 0;

            var area = SudokuArea.SudokuBoxes;

            var firstEmptyBox = SudokuArea.SudokuBoxes[0, 0];

            if (firstEmptyBox.Value != SudokuBox.NullValue)
            {
                firstEmptyBox = NextBox(area, firstEmptyBox);
            }

            if (!IsValidArea(area))
            {
                SolutionEnd("Not a Valid Sudoku!");
                return;
            }

            if (firstEmptyBox == null)
            {
                SolutionEnd("Sudoku is already Solved!");
                return;
            }

            if (!Solve(area, firstEmptyBox))
            {
                SolutionEnd("Impossible!");
                return;
            }
        
            SolutionEnd($"Solution Depth: {solutionDepth}");
        }
    
        public void Clear()
        {
            var area = SudokuArea.SudokuBoxes;
        
            for (int x = 0; x < SudokuArea.ColumnCount; x++)
            {
                for (int y = 0; y < SudokuArea.RowCount; y++)
                {
                    area[x, y].Clear();
                }
            }
        
            Cleared();
        }

        private static bool IsValidArea(SudokuBox[,] area)
        {
            var isValid = true;

            for (int x = 0; x < SudokuArea.ColumnCount; x++)
            {
                for (int y = 0; y < SudokuArea.RowCount; y++)
                {
                    var box = area[x, y];
                
                    if (box.Value == SudokuBox.NullValue) continue;

                    var validNumbers = SolutionsForBox(area, box);

                    if (validNumbers.Contains(box.Value)) continue;
                
                    isValid = false;
                    break;
                }
            }

            return isValid;
        }

        private static bool Solve(SudokuBox[,] area, SudokuBox box)
        {
            solutionDepth++;
        
            var solutions = SolutionsForBox(area, box);

            if (solutions.Count == 0) return false;
        

            SudokuBox next = NextBox(area, box);

            if (next == null)
            {
                box.SetValue(solutions[0]);
                return true;
            }

            foreach (var solution in solutions)
            {
                box.SetValue(solution);

                if (Solve(area, next))
                {
                    return true;
                }
            }

            box.Clear();
            return false;
        }

        private static SudokuBox NextBox(SudokuBox[,] area, SudokuBox current)
        {
            int nextX = current.x + 1;
            int nextY = current.y;

            if (nextX > 8)
            {
                nextX = 0;
                nextY++;
            }

            if (nextY > 8) return null;

            var next = area[nextX, nextY];

            if (next.Value == SudokuBox.NullValue) return next;

            return NextBox(area, next);
        }
    
        private static List<int> SolutionsForBox(SudokuBox[,] area, SudokuBox box)
        {
            var rows = CheckRow(area, box);
            var columns = CheckColumn(area, box);
            var subBoxes = CheckSubBox(area, box);

            var solutions = new List<int>();

            foreach (var value in rows)
            {
                if (!columns.Contains(value)) continue;
            
                if (!subBoxes.Contains(value)) continue;
            
                solutions.Add(value);
            }

            return solutions;
        }


        private static List<int> CheckRow(SudokuBox[,] area, SudokuBox box)
        {
            var result = new List<int>();

            for (int i = MinValue; i <= MaxValue; i++)
            {
                bool isValid = true;
            
                for (int x = 0; x < SudokuArea.ColumnCount; x++)
                {
                    if (x == box.x) continue;
                
                    if (area[x, box.y].Value == SudokuBox.NullValue) continue;

                    if (i == area[x, box.y].Value)
                    {
                        isValid = false;
                        break;
                    }
                }

                if (!isValid) continue;
            
                result.Add(i);
            }

            return result;
        }

        private static List<int> CheckColumn(SudokuBox[,] area, SudokuBox box)
        {
            var result = new List<int>();

            for (int i = MinValue; i <= MaxValue; i++)
            {
                bool isValid = true;
            
                for (int y = 0; y < SudokuArea.RowCount; y++)
                {
                    if (y == box.y) continue;
                
                    if (area[box.x, y].Value == SudokuBox.NullValue) continue;

                    if (i == area[box.x, y].Value)
                    {
                        isValid = false;
                        break;
                    }
                }

                if (!isValid) continue;
            
                result.Add(i);
            }

            return result;
        }
    
        private static List<int> CheckSubBox(SudokuBox[,] area, SudokuBox box)
        {
            var result = new List<int>();

            int boxStartX;

            if (box.x < 3) boxStartX = 0;

            else if (box.x < 6) boxStartX = 3;

            else boxStartX = 6;
        
            int boxStartY;

            if (box.y < 3) boxStartY = 0;

            else if (box.y < 6) boxStartY = 3;

            else boxStartY = 6;

            for (int i = MinValue; i <= MaxValue; i++)
            {
                bool isValid = true;
            
                for (int x = boxStartX; x < boxStartX + 3; x++)
                {
                    if (!isValid) break;
                
                    for (int y = boxStartY; y < boxStartY + 3; y++)
                    {
                        if (x == box.x && y == box.y) continue;
                    
                        if (area[x, y].Value == SudokuBox.NullValue) continue;

                        if (i == area[x, y].Value)
                        {
                            isValid = false;
                            break;
                        }
                    }
                }
            
                if (!isValid) continue;
            
                result.Add(i);
            }

            return result;
        }
    }
}
