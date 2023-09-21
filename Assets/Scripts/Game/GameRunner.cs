using System;
using Core.Data.Scriptable;
using Core.GameStates;
using Core.RoomsSystem;
using Scripts.DependancyInjector;
using UnityEngine;

namespace Game
{
    public class GameRunner : MonoBehaviour
    {
        public static GameRunner Instance { get; private set; }

        [Inject] private IRoomFactory _roomFactory;

        [SerializeField] private CardSet set;

        private GameContext _context;
        private GameCommander _commander;

        public event Action<GameContext, GameCommander> OnGameInitFinished;
        public GameContext Context => _context;
        public GameCommander Commander => _commander;

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
                MaxCardsInHand = 3,
                MaxPlayerHealth = 30
            };
            
            _context = new GameContext();
            _commander = GameStateSetup.SetupStates(gameSetup, _context);
            OnGameInitFinished?.Invoke(_context, _commander);
        }

        public void StartGame()
        {
            _commander.ChangeState(_commander.GameStartState);
        }
    }
}