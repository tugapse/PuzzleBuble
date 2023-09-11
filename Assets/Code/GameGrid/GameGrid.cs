using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;


public class GameGrid : MonoBehaviour
{


    public float gameStartDelay = 2;

    public bool gameStarted;

    public Vector3 Size = Vector3.one * 10;
    private float cellSize = 1f;
    public List<GridCell> gameCells = new List<GridCell>();
    public Transform spawnerPoint;
    public Transform spawnerTopRowPoint;
    public Transform gridContainer;
    public BallSpawner ballspawner;
    public ParticleSystem explotionParticles;
    public GridData gridData;


    int frameDelay = 300;


    [Header("Debug")]
    public bool drawGrid = true;
    public bool drawCells = true;
    public Color gridColor;
    public Color cellColor = Color.black;
    private bool needFloodFiil;
    private bool needClean;

    void Start()
    {
        this.ComputeGrid(true);
    }


    void FixedUpdate()
    {

        if (!CanUpdate()) return;
        this.FloodCheck();
        this.DestroyAfterFlood();


    }

    bool CanUpdate()
    {
        gameStartDelay -= Time.fixedDeltaTime;
        if (gameStartDelay < 0)
        {
            this.gameStarted = true;
            gameStartDelay = 0;
        }
        return gameStarted;
    }

    private void FloodCheck()
    {

        if (!this.needFloodFiil) return;
        var visitedFlood = new List<Vector3>();
        this.ClearGridState();
        this.FloodFill(this.GetTopRowCells(), visitedFlood);
    }

    private void ClearGridState()
    {

        foreach (var cell in this.gameCells)
        {
            if (cell.isFull)
            {
                cell.isDirty = true;
            }
        }
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
                if (otherCell.isFull && otherCell.ball.color == currentCell.ball.color)
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

    void FloodFill(List<GridCell> connected, List<Vector3> visited)
    {
        if (visited == null) visited = new List<Vector3>();
        foreach (var cell in connected)
        {
            if (visited.Contains(cell.gridPosition) || cell.isEmpty || !cell.isDirty) continue;
            cell.isDirty = false;
            visited.Add(cell.gridPosition);
            this.FloodFill(cell.connectedCells, visited);
        }
        this.needFloodFiil = false;
        this.needClean = true;
    }
    private void DestroyAfterFlood()
    {
        if (!needClean) return;
        foreach (var cell in this.gameCells)
        {
            if (cell.isEmpty) continue;
            if (cell.isDirty) cell.Fall(4);
        }
        this.needClean = false;
    }

    private void ComputeGrid(bool instantiateBall)
    {
        float increment = 0f;

        for (int y = 0; y <= Size.y; y++)
        {
            for (int x = 0; x <= Size.x; x++)
            {
                GridCell cell = new GridCell() { gridPosition = this.ComputeGridPosition(increment, x, y), gameGrid = this };

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


        float gridMinX = gridContainer.position.x - gridContainer.localScale.x / 2;
        float gridMinY = gridContainer.position.y - gridContainer.localScale.y / 2;

        Rect spanwRect = new Rect(minX, minY, spawnerPoint.localScale.x, spawnerPoint.localScale.y);
        Rect gridRect = new Rect(gridMinX, gridMinY, gridContainer.localScale.x, gridContainer.localScale.y);

        if (gridRect.Contains(cell.gridPosition)) this.gameCells.Add(cell);
        if (spanwRect.Contains(cell.gridPosition)) InstantiateBall(cell);

    }

    private void InstantiateBall(GridCell cell)
    {
        float tminX = spawnerTopRowPoint.position.x - spawnerTopRowPoint.localScale.x / 2;
        float tminY = spawnerTopRowPoint.position.y - spawnerTopRowPoint.localScale.y / 2;
        Rect topRowRect = new Rect(tminX, tminY, spawnerTopRowPoint.localScale.x, spawnerTopRowPoint.localScale.y);

        GameObject instance = this.ballspawner.InstanciateBall(cell.gridPosition);
        cell.ball = instance.GetComponent<Ball>();
        cell.ball.gameGrid = this;
        cell.ball.transform.parent = this.transform;
        cell.ball.trigger.enabled = true;
        cell.isTopRow = topRowRect.Contains(cell.gridPosition);
        cell.ball.Snap();
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
        if (!gameStarted) return;
        var visited = new List<int>();
        var connectedcells = this.GetConnectedCells(currentCell, visited);
        if (connectedcells.Count >= 3)
        {
            EmmitExplosionParticles(currentCell);
            this.gridData.BallExplode(connectedcells);
            foreach (var cell in connectedcells)
            {
                // cell.Fall(currentCell.gridPosition + Vector3.down * 2, 4);
                cell.Clear();
            }
        }
        this.needFloodFiil = true;
    }

    private void EmmitExplosionParticles(GridCell currentCell)
    {
        ParticleSystem.MainModule settings = this.explotionParticles.main;

        settings.startColor = new ParticleSystem.MinMaxGradient(currentCell.ball.ExplosionColor);
        this.explotionParticles.transform.position = currentCell.gridPosition;
        this.explotionParticles.Emit(20);
    }

    private void OnDrawGizmos()
    {
        if (drawGrid) DrawGrid();
        if (drawCells) DrawCells();
    }

    private void DrawGrid()
    {
        float radius = 0.5f;
        Gizmos.color = gridColor;
        float increment = 0f;
        for (float y = 0; y <= Size.y; y += cellSize)
        {
            Gizmos.color = gridColor;

            for (float x = 0; x <= Size.x; x += cellSize)
            {

                Gizmos.DrawWireSphere(this.ComputeGridPosition(increment, x, y), radius);
            }
            increment += 0.1f;

        }
    }

    private Vector3 ComputeGridPosition(float increment, float x, float y)
    {
        bool isOdd = MathF.Abs(y) % 2 != 0;
        float xOffset = isOdd ? 0.5f : 0;
        float radius = 0.5f;
        return this.transform.position + new Vector3(x + xOffset, 1 * (y - radius - increment), 0);
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
