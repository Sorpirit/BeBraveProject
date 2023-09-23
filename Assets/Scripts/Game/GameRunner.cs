using System;
using Core.CardSystem.Data;
using Core.Data.Scriptable;
using Core.GameStates;
using Core.RoomsSystem;
using Scripts.DependancyInjector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    public class GameRunner : MonoBehaviour
    {
        public static GameRunner Instance { get; private set; }

        [Inject] private IRoomFactory _roomFactory;

        [SerializeField] private bool autoPlay = true;
        [SerializeField] private bool deterministic;
        [SerializeField] private CardSetSO set;

        private GameContext _context;
        private GameCommander _commander;

        public event Action<GameContext, GameCommander> OnGameInitFinished;
        public GameContext Context => _context;
        public GameCommander Commander => _commander;

        private bool _gameStarted;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(this);
            }
        }

        private void Start()
        {
            var gameSetup = new GameSetup
            {
                CardSet = set,
                RoomFactory = _roomFactory,
                MaxCardsInHand = 5,
                MaxPlayerHealth = 30
            };
            
            if (deterministic)
            {
                Random.InitState(20031260);
            }
            Debug.Log("Game seed: " + Random.state);
            _context = new GameContext();
            _commander = GameStateSetup.SetupStates(gameSetup, _context);
            OnGameInitFinished?.Invoke(_context, _commander);
        }

        private void Update()
        {
            if(!_gameStarted && autoPlay)
                StartGame();
        }

        public void StartGame()
        {
            _gameStarted = true;
            _commander.ChangeState(_commander.GameStartState);
        }
    }
}