using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class GameGrid : MonoBehaviour
{
    public static GameGrid current { get; private set; }
    public bool drawGrid = true;
    public bool drawCells = true;
    public Color gridColor;
    public Color cellColor = Color.black;
    public Vector3 Size = Vector3.one * 10;
    private float cellSize = 1f;
    public Dictionary<string, GridCell> cells = new Dictionary<string, GridCell>();
    public Transform spawnerPoint;
    public BallSpawnner ballspawner;

    void Start()
    {
        GameGrid.current = this;
        this.computeGrid();
    }

    void Update()
    {

    }

    private void OnDrawGizmos()
    {
        if (drawGrid) DrawGrid();
        if (drawCells) DrawCells();

        float minX = spawnerPoint.position.x - spawnerPoint.localScale.x / 2;
        float minY = spawnerPoint.position.y + spawnerPoint.localScale.y / 2;
        float maxX = spawnerPoint.position.x + spawnerPoint.localScale.x / 2;
        float maxY = spawnerPoint.position.y - spawnerPoint.localScale.y / 2;
        Gizmos.DrawLine(new Vector3(minX, minY, 0), new Vector3(maxX, maxY, 0));
    }

    private void computeGrid()
    {
        float increment = 0f;
        float halfx = Size.x / 2;
        float halfy = Size.y;

        for (int y = 0; y <= Size.y; y++)
        {
            for (int x = 0; x <= Size.x; x++)
            {
                Vector3 newPosition = Vector3.zero;

                if (MathF.Abs(y) % 2 != 0)
                {
                    newPosition = this.transform.position + new Vector3(x, 1 * -(y - halfy - 0.5f - increment)) - Vector3.right * halfx;
                }
                else
                {
                    newPosition = this.transform.position + new Vector3(x + 0.5f, 1 * -(y - halfy - 0.5f - increment), 0) - Vector3.right * halfx;

                }
                GridCell cell = new GridCell() { gridPosition = newPosition };
                this.ChecSpawnPointkAndAddBall(cell);
                this.cells[x + ":" + y] = cell;
            }
            increment += 0.1f;
        }
    }

    private void ChecSpawnPointkAndAddBall(GridCell cell)
    {
        float minX = spawnerPoint.position.x - spawnerPoint.localScale.x / 2;
        float minY = spawnerPoint.position.y - spawnerPoint.localScale. y / 2;

        Rect rect = new Rect(minX, minY, spawnerPoint.localScale.x, spawnerPoint.localScale.y);
        if (rect.Contains(cell.gridPosition)){
            
            GameObject instance = this.ballspawner.InstanciateBall(cell.gridPosition);
            cell.ball = instance.GetComponent<Ball>();
            cell.ball.Snap();
            
        }
    }

    public GridCell getGridPosition(Vector3 worldPosition)
    {
        GridCell result = null;
        float smallDist = 5;
        var keys = this.cells.Keys.ToArray();
        for (int i = 0; i < keys.Length; i++)
        {
            string key = keys[i];
            GridCell c = this.cells[key];
            float distance = Vector2.Distance(c.gridPosition, worldPosition);
            if (distance < smallDist)
            {
                result = c;
                smallDist = distance;
            }
        }
        return result;
    }

    private void DrawGrid()
    {
        float halfx = Size.x / 2;
        float halfy = Size.y;
        int minX = (int)(this.transform.position.x - halfx);
        int maxX = (int)(this.transform.position.x + halfx);

        int minY = (int)(this.transform.position.y + halfy);
        int maxY = (int)(this.transform.position.y - halfy);
        Gizmos.color = gridColor;
        float increment = 0f;
        for (float y = 0; y <= Size.y; y += cellSize)
        {
            Gizmos.color = gridColor;
            Vector3 fromPosition = transform.position - new Vector3(minX, y - halfy, 0);
            Vector3 toPosition = transform.position - new Vector3(maxX, y - halfy, 0);
            // Gizmos.DrawLine(fromPosition, toPosition);
            for (float x = 0; x <= Size.x; x += cellSize)
            {
                if (MathF.Abs(y) % 2 != 0)
                {
                    Gizmos.DrawWireSphere(transform.position + new Vector3(x, 1 * -(y - halfy - 0.5f - increment)) - Vector3.right * halfx, 0.5f); ;
                }
                else
                {
                    Gizmos.DrawWireSphere(transform.position + new Vector3(x + 0.5f, 1 * -(y - halfy - 0.5f - increment), 0) - Vector3.right * halfx, 0.5f);

                }
            }
            increment += 0.1f;

        }
    }

    private void DrawCells()
    {
        Gizmos.color = this.cellColor;
        var keys = this.cells.Keys.ToArray();
        for (int i = 0; i < keys.Length; i++)
        {
            string key = keys[i];
            GridCell c = this.cells[key];

            Gizmos.DrawWireSphere(c.gridPosition, 0.5f);
        }

    }

    public void checkConnection(GridCell currentCell)
    {
        var visited = new List<string>();
        var connectedcells = this.getConnectedCells(currentCell, visited);
        if (connectedcells.Count >= 3)
        {
            foreach (var cell in connectedcells)
            {
                cell.Clear();
            }
        }
    }


    private List<GridCell> getConnectedCells(GridCell currentCell, List<string> visited = null, List<GridCell> connetedCells = null)
    {
        if (currentCell == null) return new List<GridCell>();
        var keys = this.cells.Keys.ToArray();
        if (visited == null) visited = new List<string>();
        if (connetedCells == null) connetedCells = new List<GridCell>();

        for (int i = 0; i < keys.Length; i++)
        {
            string key = keys[i];
            GridCell otherCell = this.cells[key];
            if (otherCell == currentCell) continue;
            if (!visited.Contains(key) && currentCell.isRange(otherCell.gridPosition, 1.1f))
            {
                visited.Add(key);
                if (otherCell.ball && otherCell.ball.color == currentCell.ball.color)
                {
                    connetedCells.Add(otherCell);
                    this.getConnectedCells(otherCell, visited, connetedCells);
                }
            }
        }
        return connetedCells;
    }

}
