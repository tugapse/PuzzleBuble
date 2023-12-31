using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


public class GameGrid : MonoBehaviour
{

    [SerializeField] int currentLevelIndex;
    [SerializeField] Vector2Int Size = Vector2Int.one;


    [Header("Spawners")]
    [SerializeField] Transform spawnerPoint;
    [SerializeField] Transform spawnerTopRowPoint;
    [SerializeField] Transform gridContainer;
    [SerializeField] BallSpawner ballspawner;
    [SerializeField] Transform gameViewBounds;

    [Header("Managers")]
    [SerializeField] LevelManager levelManager;
    [SerializeField] PlayerManager playerManager;
    [SerializeField] ParticleSystemManager particleSystemManager;


    [SerializeField] List<GridCell> gameCells = new List<GridCell>();
    float gameStartDelay = 2;
    float cellSize = 1f;
    bool needFloodFiil;
    bool needClean;
    Vector2 scrollView = Vector2.zero;
    float scrollViewSpeed = 0.09f;
    Level currentLevel;



    public GridCell[] VisibleCells { get { return this.gameCells.Where(cell => this.IsVisibleCell(cell)).ToArray(); } }
    public GridCell[] ActiveCells { get { return this.gameCells.Where(cell => cell.isFull).ToArray(); } }

    private void Awake()
    {

        this.levelManager.StopGame();
        this.levelManager.OnRemoveConnected += this.RemoveConnected;
        this.levelManager.OnGameOver += this.OnGameOver;
    }

    private void OnGameOver()
    {
        this.levelManager.StopGame();
    }

    void Start()
    {

        this.currentLevel = this.levelManager.LoadLevel(this.currentLevelIndex);
        this.gameStartDelay = currentLevel.StartLevelDelay;
        this.CreateGridCells(true);
    }
    private void CreateGridCells(bool instantiateBall)
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

    void FixedUpdate()
    {
        if (!CanUpdate()) return;
        var VisibleCellsCount = this.VisibleCells.Length;
        Debug.Log("Active Cells : " + this.ActiveCells.Length + " Visible cells : " + VisibleCellsCount);
        this.FloodCheck();
        this.DestroyAfterFlood();

        if (VisibleCellsCount < this.currentLevel.minViewBallsBeforeScroll)
        {
            this.scrollView -= Vector2.down * scrollViewSpeed;
            this.levelManager.ScrollView(this.scrollView);
        }
    }

    bool CanUpdate()
    {
        gameStartDelay -= Time.fixedDeltaTime;

        if (gameStartDelay < 0)
        {
            this.levelManager.StartGame();
            gameStartDelay = 0;
            StartCoroutine(this.StartLevel());
        }
        else if (!this.levelManager.LevelRunning)
        {
            this.levelManager.SetLevelCountDown(gameStartDelay);
        }
        return this.levelManager.LevelRunning;
    }
    private IEnumerator StartLevel()
    {
        yield return new WaitForSeconds(1);
        this.levelManager.StartLevel(this.currentLevelIndex);
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
            if (otherCell == currentCell || IsVisibleCell(otherCell) == false) continue;

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

    bool IsVisibleCell(GridCell cell)
    {
        if (cell.isEmpty) return false;
        Rect viewRect = new Rect(
            gameViewBounds.position.x - gameViewBounds.localScale.x / 2,
            gameViewBounds.position.y - gameViewBounds.localScale.y / 2,
            gameViewBounds.localScale.x,
            gameViewBounds.localScale.y
        );
        return viewRect.Contains(cell.gridPosition);
    }

    List<GridCell> GetTopRowCells()
    {
        return this.gameCells.Where(cell => cell.isTopRow).ToList<GridCell>();
    }

    void FloodFill(List<GridCell> connected, List<Vector3> visited)
    {
        if (visited == null) visited = new List<Vector3>();
        foreach (var cell in connected.Where(cell => cell != null))
        {
            if (visited.Contains(cell.gridPosition) || cell.isEmpty || !cell.isDirty) continue;
            cell.isDirty = false;
            visited.Add(cell.gridPosition);
            this.FloodFill(cell.connectedCells, visited);
        }
        this.needFloodFiil = false;
        this.needClean = true;
    }
    void DestroyAfterFlood()
    {
        if (!needClean) return;
        foreach (var cell in this.gameCells)
        {
            if (cell.isEmpty) continue;
            if (cell.isDirty) cell.Fall(4);
        }
        this.needClean = false;
    }



    void CheckSpawnPointAndAddBall(GridCell cell)
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

    void InstantiateBall(GridCell cell)
    {
        float tminX = spawnerTopRowPoint.position.x - spawnerTopRowPoint.localScale.x / 2;
        float tminY = spawnerTopRowPoint.position.y - spawnerTopRowPoint.localScale.y / 2;
        Rect topRowRect = new Rect(tminX, tminY, spawnerTopRowPoint.localScale.x, spawnerTopRowPoint.localScale.y);

        GameObject instance = this.ballspawner.InstanciateBall(cell.gridPosition);
        cell.ball = instance.GetComponent<Ball>();
        cell.ball.levelManager = this.levelManager;
        cell.ball.transform.parent = this.transform;
        cell.ball.trigger.enabled = true;
        cell.isTopRow = topRowRect.Contains(cell.gridPosition);
        foreach (Collider2D col in cell.ball.GetComponents<Collider2D>())
        {
            col.enabled = col.isTrigger;
        }
        cell.ball.Snap();
        foreach (Collider2D col in cell.ball.GetComponents<Collider2D>())
        {
            cell.ball.preventPop = true;
            col.enabled = true;
        }
    }

    public GridCell GetGridPosition(Vector3 worldPosition)
    {
        GridCell result = null;
        float smallDist = 500;
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
        if (!this.levelManager.LevelRunning) return;
        var visited = new List<int>();
        var connectedcells = this.GetConnectedCells(currentCell, visited);
        if (connectedcells.Count >= 3) StartCoroutine(this.ExplodeBalls(currentCell, connectedcells));


    }
    IEnumerator ExplodeBalls(GridCell currentCell, List<GridCell> connectedcells)
    {
        connectedcells.Sort((a, b) => Mathf.Abs(Vector3.Distance(currentCell.gridPosition, a.gridPosition)) > Mathf.Abs(Vector3.Distance(currentCell.gridPosition, b.gridPosition)) ? 1 : -1);
        this.levelManager.ConnectedBallsExplode(connectedcells);
        foreach (var cell in connectedcells)
        {
            EmmitExplosionParticles(cell);
            this.levelManager.BallExplode(cell);
            cell.Fall();
            yield return new WaitForSeconds(0.05f);

        }
        this.needFloodFiil = true;
    }
    void EmmitExplosionParticles(GridCell currentCell)
    {
        this.particleSystemManager.BallExplosion(currentCell.gridPosition, currentCell.ball.explosionColor, 15);
    }

    Vector3 ComputeGridPosition(float increment, float x, float y)
    {
        bool isOdd = MathF.Abs(y) % 2 != 0;
        float xOffset = isOdd ? 0.5f : 0;
        float radius = 0.5f;
        return this.transform.position + new Vector3(x + xOffset, 1 * (y - radius - increment), 0);
    }

    public void ResetCells()
    {
        levelManager.StopGame();
        if (gameCells != null)
        {

            foreach (GridCell cell in gameCells)
            {
                cell.Clear(true);
            }
            gameCells.Clear();
        }
        this.CreateGridCells(true);


    }

}


#if UNITY_EDITOR

[CustomEditor(typeof(GameGrid))]
public class GameGridEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GameGrid gridObject = (GameGrid)target;
        DrawDefaultInspector();

        if (GUILayout.Button("Generate Balls"))
        {
            gridObject.ResetCells();
        }



    }

}
#endif