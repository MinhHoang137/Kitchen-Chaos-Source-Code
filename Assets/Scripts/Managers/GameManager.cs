using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public event EventHandler OnStateChange;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;
    private enum State
    {
        WaitingToStart,
        CountDownToStart,
        GamePlaying,
        GameOver
    }
    private State state;
	private float waitingToStartTimer = 1;
	private float countDownToStartTimer = 3;
    private bool isGamePaused = false;
	private void Awake()
	{
        Instance = this;
		state = State.WaitingToStart;
	}
	private void Start()
	{
		GameInput.Instance.OnPause += GameInput_OnPause;
	}

	private void GameInput_OnPause(object sender, EventArgs e)
	{
		TogglePauseGame();
	}

	private void Update()
	{
        switch (state) {
            case State.WaitingToStart:
                waitingToStartTimer -= Time.deltaTime;
                if (waitingToStartTimer <= 0) {
                    state = State.CountDownToStart;
                    OnStateChange?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.CountDownToStart:
                countDownToStartTimer -= Time.deltaTime;
                if (countDownToStartTimer <= 0)
                {
                    state = State.GamePlaying;
					OnStateChange?.Invoke(this, EventArgs.Empty);
				}
                break;
            case State.GamePlaying: 
                if (DeliveryManager.Instance.FailToDeliverAllow <= 0) { 
                    state = State.GameOver;
					OnStateChange?.Invoke(this, EventArgs.Empty);
				}
                break;
            case State.GameOver:
                break;
        }
	}
    public bool IsGamePlaying()
    {
        return state == State.GamePlaying;
    }
    public float GetCountdownToStartTimer()
    {
        return countDownToStartTimer; 
    }
    public bool IsCountdownToStart()
    {
        return state == State.CountDownToStart;
    }
    public bool IsGameOver() { return state == State.GameOver; }
    public void TogglePauseGame()
    {
        isGamePaused = !isGamePaused;
        if (isGamePaused)
        {
            OnGamePaused?.Invoke(this, EventArgs.Empty);
            Time.timeScale = 0;
        }
        else
        {
			OnGameUnpaused?.Invoke(this, EventArgs.Empty);
			Time.timeScale = 1;
        }
    }
}
