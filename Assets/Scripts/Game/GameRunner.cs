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

        public event Action<GameContext> OnGameContextCreated;
        public GameContext Context => _context;

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
            _context = GameStateSetup.CreateBasicGame(set, _roomFactory);
            OnGameContextCreated?.Invoke(_context);
        }

        public void StartGame()
        {
            _context.ChangeState(_context.GameStartState);
        }
    }
}