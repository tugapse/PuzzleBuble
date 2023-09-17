using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Database/Grid")]
public class LevelManager : ScriptableObject
{
    private GameGrid _currentGrid;
    private bool _gameStarted = false;
    public GameGrid CurrentGrid { get { if (_currentGrid == null) _currentGrid = GameObject.FindFirstObjectByType<GameGrid>(); return this._currentGrid; } }

    public bool GameStarted { get { return _gameStarted; } }

    public UnityAction<Collider2D[]> OnBallCollision;
    public UnityAction<GridCell[]> OnBallExplode;
    public UnityAction<GridCell> OnRemoveConnected;
    public UnityAction OnWarningState;
    public UnityAction OnNormalState;
    public UnityAction<Level> onLevelChanged;
    public UnityAction<Level> onLevelStarted;
    public UnityAction<Level> onLevelEnded;
    public UnityAction<float> OnLevelCount;



    [SerializeField] Level[] levels;
    public LevelManager()
    {
        this._gameStarted = false;
    }

    public void BallCollision()
    {
        this.OnBallCollision?.Invoke(null);
    }

    public void BallExplode(List<GridCell> balls)
    {
        this.OnBallExplode?.Invoke(balls.ToArray());
    }

    public void RemoveConnected(GridCell cell)
    {
        this.OnRemoveConnected?.Invoke(cell);
    }

    public void SetWarningState()
    {
        this.OnWarningState?.Invoke();

    }

    public void SetNormalState()
    {
        this.OnNormalState?.Invoke();
    }

    public void LoadLevel(int levelIndex)
    {
        this.onLevelChanged?.Invoke(this.levels[levelIndex]);
    }

    public void StartLevel(int levelIndex)
    {
        this.onLevelStarted?.Invoke(this.levels[levelIndex]);
    }

    public void SetLevelCountDown(float seconds)
    {
        this.OnLevelCount?.Invoke(seconds);
    }


    public void StartGame()
    {
        this._gameStarted = true;
    }

    public void StopGame()
    {
        this._gameStarted = false;
    }
}
