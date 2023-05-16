using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using SakamotoTest;
using UnityEngine.UI;
using System;
using Cysharp.Threading.Tasks;

public class BattleStateController : MonoBehaviour
{
    public GameObject ComanndObj => _commandObj;
    public GameObject GameOverTextObj => _gameOverTextObj;
    public PlayerController PlayerController => _playerController;
    public EnemyController EnemyController => _enemyController;
    public ResultUIScript ResultUIScript => _resultUIScript;
    public SkillDataManagement SkillManagement => _skillManagement;

    [SerializeField] private GameObject _commandObj;
    [SerializeField] private ActorGenerator _generator;
    [SerializeField] private Text _stateText;
    [SerializeField] private ResultUIScript _resultUIScript;
    [SerializeField] private SkillDataManagement _skillManagement;
    [SerializeField] private GameObject _gameOverTextObj;
    private StateMachine<BattleStateController> _stateMachine;
    private List<ActionSequentialData> _actionSequentialList = new();
    private PlayerController _playerController;
    private EnemyController _enemyController;


    void Start()
    {
        _playerController = _generator.PlayerController;
        _enemyController = _generator.EnemyController;

        _stateMachine = new StateMachine<BattleStateController>(this);
        _stateMachine.AddAnyTranstion<GameStartState>((int)BattleEvent.BattleStart);
        _stateMachine.AddAnyTranstion<SBattleEndState>((int)BattleEvent.BattleEnd);
        _stateMachine.AddAnyTranstion<GameOverState>((int)BattleEvent.GameOver);
        _stateMachine.AddTransition<GameStartState, SelectNextActorTransitionState>
                                  ((int)BattleEvent.StartToNextActorState);
        _stateMachine.AddTransition<SelectNextActorTransitionState, SPlayerAttackState>
                                  ((int)BattleEvent.SelectStateToPlayerTrun);
        _stateMachine.AddTransition<SPlayerAttackState, SPlayerAttackState>
                                  ((int)BattleEvent.ReturnPlayer);
        _stateMachine.AddTransition<SelectNextActorTransitionState, SEnemyAttackState>
                                  ((int)BattleEvent.SelectStateToEnemyTrun);
        _stateMachine.AddTransition<SPlayerAttackState, SelectNextActorTransitionState>
                                  ((int)BattleEvent.PlayerTurnToSelectState);
        _stateMachine.AddTransition<SEnemyAttackState, SelectNextActorTransitionState>
                                  ((int)(BattleEvent.EnemyToSelectState));

        _stateMachine.Start<GameStartState>();
        _generator.PlayerController.GameOverAction += GameOver;
    }

    void Update()
    {
        _stateText.text = _stateMachine.CurrentState.ToString();
        _stateMachine.Update();
    }

    /// <summary>
    /// �s�����̌���
    /// </summary>
    public void ActionSequentialDetermining()
    {
        _actionSequentialList.Clear();
        var playerActionSequential = new ActionSequentialData();

        playerActionSequential.PlayerController = _playerController;
        playerActionSequential.WeaponWeight = _playerController.PlayerStatus.EquipWeapon.WeaponWeight.Value;
        _actionSequentialList.Add(playerActionSequential);

        var enemyActionSequential = new ActionSequentialData();

        enemyActionSequential.EnemyController = _enemyController;
        enemyActionSequential.WeaponWeight = _enemyController.EnemyStatus.EquipWeapon.WeaponWeight;
        _actionSequentialList.Add(enemyActionSequential);
        //�G�������̂ł������p
        //for (int i = 0; i < _enemyController.Count; i++)
        _actionSequentialList.OrderBy(x => -x.WeaponWeight);
    }

    /// <summary>
    /// List�̒����玟�ɍs������Actor�����肳����
    /// </summary>
    public void NextActorStateTransition()
    {
        var token = this.GetCancellationTokenOnDestroy();
        UniTask.WaitUntil(() => _stateMachine.CurrentState == _stateMachine.GetOrAddState<SelectNextActorTransitionState>(), cancellationToken: token);

        for (int i = 0; i < _actionSequentialList.Count; i++)
        {
            if (_actionSequentialList[i].PlayerController && !_actionSequentialList[i].alreadyActedOn)
            {
                _stateMachine.Dispatch((int)BattleEvent.SelectStateToPlayerTrun);
                break;
            }
            else if (_actionSequentialList[i].EnemyController && !_actionSequentialList[i].alreadyActedOn)
            {
                _stateMachine.Dispatch((int)BattleEvent.SelectStateToEnemyTrun);
                break;
            }
            else if (_actionSequentialList[i].PlayerController && _actionSequentialList[i].EnemyController)
            {
                Debug.LogError("�z��O��Data���܂܂�Ă��܂�");
            }
            else if(_actionSequentialList.Count -1 == i)
            {
                ActionSequentialDetermining();
                Debug.Log("�S�čs���ς݂Ȃ̂ōs���������߂Ȃ���");
               NextActorStateTransition();
            }
        }
    }

    /// <summary>
    /// Actor�̍s���̏I���ɌĂԊ֐�
    /// </summary>
    public async void ActorStateEnd() 
    {
        ClearCheck();

        for (int i = 0; i < _actionSequentialList.Count; i++) 
        {
            if (!_actionSequentialList[i].alreadyActedOn) 
            {
                _actionSequentialList[i].alreadyActedOn = true;
                break;
            }
        }

        if (_stateMachine.CurrentState == _stateMachine.GetOrAddState<SPlayerAttackState>())
        {
            var result = await _skillManagement.InEffectCheck("���N", ActorAttackType.Player);
            if (result)
            {
                _stateMachine.Dispatch((int)BattleEvent.ReturnPlayer);
            }
            else 
            {
                _stateMachine.Dispatch((int)BattleEvent.PlayerTurnToSelectState);
            }
            
        }
        else if (_stateMachine.CurrentState == _stateMachine.GetOrAddState<SEnemyAttackState>()) 
        {
            _stateMachine.Dispatch((int)BattleEvent.EnemyToSelectState);
        }
    }

    public void ClearCheck() 
    {
        if (_enemyController.EnemyStatus.IsWeaponsAllBrek()) 
        {
            _stateMachine.Dispatch((int)BattleEvent.BattleEnd);
        }
    }

    public void GameOver() 
    {
        _stateMachine.Dispatch((int)BattleEvent.GameOver);
    }

    public enum BattleEvent
    {
        BattleStart,
        StartToNextActorState,
        PlayerTurnToSelectState,
        EnemyToSelectState,
        ReturnPlayer,
        SelectStateToPlayerTrun,
        SelectStateToEnemyTrun,
        BattleEnd,
        GameOver,
    }

    public class ActionSequentialData
    {
        public PlayerController PlayerController;
        public EnemyController EnemyController;
        public float WeaponWeight;
        [Tooltip("�s���ς݂��ǂ���")]
        public bool alreadyActedOn = false;
    }
}
