using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockManager : MonoBehaviour
{
    public List<RockBehaviour> rocks = new List<RockBehaviour>();
    public RockBehaviour[,] rockMatrix = new RockBehaviour[5, 6];
    public RockBehaviour[,] baseRockMatrix = new RockBehaviour[5, 6];

    private void Start()
    {
        rockMatrix[0, 0] = rocks.Count > 12 ? rocks[12] : null;
        rockMatrix[1, 0] = null;
        rockMatrix[2, 0] = null;
        rockMatrix[3, 0] = null;
        rockMatrix[4, 0] = rocks.Count > 13 ? rocks[13] : null;

        rockMatrix[0, 1] = rocks.Count > 0 ? rocks[0] : null;
        rockMatrix[1, 1] = rocks.Count > 1 ? rocks[1] : null;
        rockMatrix[2, 1] = null;
        rockMatrix[3, 1] = rocks.Count > 2 ? rocks[2] : null;
        rockMatrix[4, 1] = rocks.Count > 3 ? rocks[3] : null;

        rockMatrix[0, 2] = null;
        rockMatrix[1, 2] = rocks.Count > 4 ? rocks[4] : null;
        rockMatrix[2, 2] = rocks.Count > 5 ? rocks[5] : null;
        rockMatrix[3, 2] = rocks.Count > 6 ? rocks[6] : null;
        rockMatrix[4, 2] = null;

        rockMatrix[0, 3] = rocks.Count > 7 ? rocks[7] : null;
        rockMatrix[1, 3] = null;
        rockMatrix[2, 3] = rocks.Count > 8 ? rocks[8] : null;
        rockMatrix[3, 3] = null;
        rockMatrix[4, 3] = rocks.Count > 9 ? rocks[9] : null;

        rockMatrix[0, 4] = null;
        rockMatrix[1, 4] = rocks.Count > 10 ? rocks[10] : null;
        rockMatrix[2, 4] = null;
        rockMatrix[3, 4] = rocks.Count > 11 ? rocks[11] : null;
        rockMatrix[4, 4] = null;

        rockMatrix[0, 5] = null;
        rockMatrix[1, 5] = null;
        rockMatrix[2, 5] = null;
        rockMatrix[3, 5] = null;
        rockMatrix[4, 5] = null;

        for (int x = 0; x < rockMatrix.GetLength(0); x++)
        {
            for (int y = 0; y < rockMatrix.GetLength(1); y++)
            {
                baseRockMatrix[x, y] = rockMatrix[x, y];
            }
        }
        //PrintMatrix();
    }

    public void ResetRockMatrix()
    {
        foreach (RockBehaviour rock in rocks)
        {
            if (rock != null)
            {
                rock.transform.position = rock.basePosition;
            }
        }

        for (int x = 0; x < rockMatrix.GetLength(0); x++)
        {
            for (int y = 0; y < rockMatrix.GetLength(1); y++)
            {
                rockMatrix[x, y] = baseRockMatrix[x, y];
            }
        }
    }

    public bool TryToMoveRock(RockBehaviour rock, Vector2Int direction)
    {
        // Get current grid position
        int x = -1;
        int y = -1;

        for (int i = 0; i < rockMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < rockMatrix.GetLength(1); j++)
            {
                if (rockMatrix[i, j] == rock)
                {
                    x = i;
                    y = j;
                    break;
                }
            }
        }

        // Calculate new position
        int newX = x + direction.x;
        int newY = y + direction.y;

        // Check if new position is within bounds and empty
        if (newX >= 0 && newX < rockMatrix.GetLength(0) &&
            newY >= 0 && newY < rockMatrix.GetLength(1))
        {
            if (rockMatrix[newX, newY] == null)
            {
                // Update rockMatrix
                rockMatrix[x, y] = null;
                rockMatrix[newX, newY] = rock;
                //PrintMatrix();
                return true;
            }
        }
        //PrintMatrix();
        return false;
    }

    private void PrintMatrix()
    {
        for (int i = rockMatrix.GetLength(1) - 1; i >= 0; i--)
        {
            string row = "";
            for (int j = 0; j < rockMatrix.GetLength(0); j++)
            {
                row += rockMatrix[j, i] != null ? "1 " : "0 ";
            }
            Debug.Log($"Row {i}: {row}");
        }
    }
}