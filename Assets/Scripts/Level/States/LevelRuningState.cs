using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime;

public class LevelRuningState : BaseState
{
    private StateMachine _stateMachine;
    private LevelRuningStateUI _levelUI;
    private Player _player;
    private PlayerInput _playerInput;
    private LevelWinTrigger _winTrigger;
    private LevelPauser _pauser;

    public LevelRuningState(StateMachine stateMachine, LevelRuningStateUI levelUI,
        Player player,PlayerInput input ,LevelWinTrigger levelWinTrigger, LevelPauser pauser)
    {
        _stateMachine = stateMachine;
        _levelUI = levelUI;
        _player = player;
        _playerInput = input;
        _winTrigger = levelWinTrigger;
        _pauser = pauser;
    }

    public override void Enter()
    {
        _playerInput.EnableInput();
        _player.EnableGravity();
        Coroutines.StartRoutine(_levelUI.Open());
        
        _winTrigger.OnPlayerWin += OnPlayerWin;
        _player.OnDie += OnPlayerLost;
        _pauser.OnLevelPaused += Pause;
    }

    public override void Exit()
    {
        _playerInput.DisableInput();
        Coroutines.StartRoutine(_levelUI.Close());
        _player.DisableGravity();
        
        _winTrigger.OnPlayerWin -= OnPlayerWin;
        _player.OnDie -= OnPlayerLost;
        _pauser.OnLevelPaused -= Pause;
    }

    private void OnPlayerWin()
    {
        _stateMachine.SwitchState<LevelWinState>();
    }

    private void OnPlayerLost()
    {
        _stateMachine.SwitchState<LevelLoseState>();
    }

    private void Pause()
    {
        _stateMachine.SwitchState<LevelPauseState>();
    }
}
