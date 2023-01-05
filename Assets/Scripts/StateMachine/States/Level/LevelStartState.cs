﻿using System.Collections;
public class LevelStartState : BaseState
{
    private StateMachine _stateMachine;
    private Player _player;
    private LevelStartMenuUI _menuUI;
    private LevelPauser _pauser;

    public LevelStartState(StateMachine stateMachine, Player player , LevelStartMenuUI startMenu,
        LevelPauser levelPauser)
    {
        _stateMachine = stateMachine;
        _player = player;
        _menuUI = startMenu;
        _pauser = levelPauser;
    }

    public override void Enter()
    {
        Coroutines.StartRoutine(_menuUI.Open());
        _player.DisableGravity();
        _player.ResetToStartState();

        _menuUI.OnLevelStart += StartGame;
        _pauser.OnLevelPaused += Pause;
    }

    public override void Exit()
    {
        Coroutines.StartRoutine(_menuUI.Close());
        _menuUI.OnLevelStart -= StartGame;
        _pauser.OnLevelPaused -= Pause;
    }

    private void StartGame()
    {
        _stateMachine.SwitchState<LevelRuningState>();
    }

    private void Pause()
    {
        _stateMachine.SwitchState<LevelPauseState>();
    }
}
