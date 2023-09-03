using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;


struct VisitedCell
{
    public int index;
    public GridCell cell;
    public bool wasVisited;

}
public class GameGrid : MonoBehaviour
{
    public static GameGrid current { get; private set; }
    public float startDelay = 2;
    public bool GameStarted;

    public Vector3 Size = Vector3.one * 10;
    private float cellSize = 1f;
    public List<GridCell> gameCells = new List<GridCell>();
    public Transform spawnerPoint;
    public Transform spawnerTopRowPoint;
    public Transform gridContainer;
    public BallSpawner ballspawner;
    public int maxFrameDelay = 20;
    int frameDelay = 300;

    [Header("Debug")]
    public bool drawGrid = true;
    public bool drawCells = true;
    public Color gridColor;
    public Color cellColor = Color.black;
    private bool needFloodFiil;

    void Start()
    {
        GameGrid.current = this;
        frameDelay = this.maxFrameDelay;
        this.ComputeGrid(true);
    }


    void FixedUpdate()
    {
        startDelay -= Time.fixedDeltaTime;
        if (startDelay < 0)
        {
            this.GameStarted = true;
            startDelay = 0;
        }
        if (frameDelay <= 0)
        {
            frameDelay--;
            return;
        }
        frameDelay = maxFrameDelay;

        this.DestroyOldCells();
        this.FloodCheck();


    }

    private void FloodCheck()
    {

        if (!this.needFloodFiil) return;

        var visitedFlood = new List<Vector3>();
        this.ClearGridState();
        this.FloodFill(this.GetTopRowCells(), visitedFlood);
    }

    private List<GridCell> GetConnectedCells(GridCell currentCell, List<int> visited = null, List<GridCell> connetedCells = null)
    {
        if (currentCell == null) return new List<GridCell>();
        if (visited == null) visited = new List<int>();
        if (connetedCells == null) connetedCells = new List<GridCell>();
        // TODO: add cells that are inside game area Rect
        for (int i = 0; i < gameCells.Count; i++)
        {
            GridCell otherCell = this.gameCells[i];
            if (otherCell == currentCell) continue;
            if (!visited.Contains(i) && currentCell.isRange(otherCell.gridPosition, 1.1f))
            {
                visited.Add(i);
                if (otherCell.ball && otherCell.ball.color == currentCell.ball.color)
                {
                    connetedCells.Add(otherCell);
                    this.GetConnectedCells(otherCell, visited, connetedCells);
                }
            }
        }
        return connetedCells;
    }

    public List<GridCell> GetTopRowCells()
    {
        return this.gameCells.Where(cell => cell.isTopRow).ToList<GridCell>();
    }

    public void FloodFill(List<GridCell> connected, List<Vector3> visited)
    {
        if (visited == null) visited = new List<Vector3>();
        foreach (var cell in connected)
        {
            if (visited.Contains(cell.gridPosition) || cell.ball == null) continue;
            cell.ball.toDestroy = false;
            visited.Add(cell.gridPosition);
            this.FloodFill(cell.connectedCells, visited);
        }
        this.needFloodFiil = false;
    }
    private void DestroyOldCells()
    {
        bool deleted = false;
        foreach (var cell in this.gameCells)
        {
            if (cell.ball == null) continue;
            if (cell.ball.toDestroy)
            {
                deleted = true;
                cell.Clear();
            }
        }
        this.needFloodFiil = deleted;
    }

    private void ComputeGrid(bool instantiateBall)
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


                if (instantiateBall)
                    this.CheckSpawnPointAndAddBall(cell);
            }
            increment += 0.1f;
        }

    }

    private void CheckSpawnPointAndAddBall(GridCell cell)
    {

        float minX = spawnerPoint.position.x - spawnerPoint.localScale.x / 2;
        float minY = spawnerPoint.position.y - spawnerPoint.localScale.y / 2;

        float tminX = spawnerTopRowPoint.position.x - spawnerTopRowPoint.localScale.x / 2;
        float tminY = spawnerTopRowPoint.position.y - spawnerTopRowPoint.localScale.y / 2;

        float gminX = gridContainer.position.x - gridContainer.localScale.x / 2;
        float gminY = gridContainer.position.y - gridContainer.localScale.y / 2;

        Rect rect = new Rect(minX, minY, spawnerPoint.localScale.x, spawnerPoint.localScale.y);
        Rect topRowRect = new Rect(tminX, tminY, spawnerTopRowPoint.localScale.x, spawnerTopRowPoint.localScale.y);
        Rect gridRect = new Rect(gminX, gminY, gridContainer.localScale.x, gridContainer.localScale.y);

        if (gridRect.Contains(cell.gridPosition)) this.gameCells.Add(cell);

        if (rect.Contains(cell.gridPosition))
        {

            GameObject instance = this.ballspawner.InstanciateBall(cell.gridPosition);
            cell.ball = instance.GetComponent<Ball>();
            cell.ball.transform.parent = this.transform;
            cell.ball.trigger.enabled = true;
            cell.isTopRow = topRowRect.Contains(cell.gridPosition);
            cell.Debug();
            cell.ball.Snap();

        }
    }

    public GridCell GetGridPosition(Vector3 worldPosition)
    {
        GridCell result = null;
        float smallDist = 5;
        for (int i = 0; i < this.gameCells.Count; i++)
        {
            GridCell c = this.gameCells[i];
            float distance = Vector2.Distance(c.gridPosition, worldPosition);
            if (distance < smallDist)
            {
                result = c;
                smallDist = distance;
            }
        }
        return result;
    }

    public void RemoveConnected(GridCell currentCell)
    {
        if (!GameStarted) return;
        var visited = new List<int>();
        var connectedcells = this.GetConnectedCells(currentCell, visited);
        if (connectedcells.Count >= 3)
        {
            foreach (var cell in connectedcells)
            {
                cell.Clear();
            }
        }
        this.needFloodFiil = true;
    }

    private void ClearGridState()
    {

        foreach (var cell in this.gameCells)
        {
            if (cell.ball)
            {
                cell.ball.toDestroy = true;
            }
        }
    }
    private void OnDrawGizmos()
    {
        if (drawGrid) DrawGrid();
        if (drawCells) DrawCells();
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
        for (int i = 0; i < gameCells.Count; i++)
        {
            GridCell c = this.gameCells[i];
            Gizmos.DrawWireSphere(c.gridPosition, 0.5f);
        }

    }


}
