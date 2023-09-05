using System;
using Unity.Plastic.Newtonsoft.Json.Serialization;
using UnityEngine;

namespace Core.PlayerSystems
{
    public class PlayerPawn
    {
        private readonly IHealthSystem _healthSystem;
        
        public PlayerInventory Inventory { get; }

        public IHealthSystem HealthSystem => _healthSystem;

        public Vector2Int Position { get; private set; }
        public event Action<Vector2Int> OnPlayerMove;

        public PlayerPawn(IHealthSystem healthSystem)
        {
            _healthSystem = healthSystem;
            Inventory = new PlayerInventory();
        }

        public void Move(Vector2Int roomPosition)
        {
            Position = roomPosition;
            OnPlayerMove.Invoke(Position);
        }
    }
}