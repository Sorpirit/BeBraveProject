using Core.CardSystem.Data.Cards;
using Core.Data;
using Core.Data.Rooms;
using Library.Collections;
using NUnit.Framework;
using UnityEngine;

namespace Tests.CoreTest.Data
{
    public class DungeonTest
    {
        [Test]
        public void InitRoomTest()
        {
            Dungeon dungeon = new Dungeon();
            Vector2Int roomPosition = Vector2Int.zero;
            Room initRoom = default;
            dungeon.OnRoomPlaced += (position, room) =>
            {
                roomPosition = position;
                initRoom = room;
            };
            
            dungeon.InitRoom(new Vector2Int(1, 2));
            
            Assert.AreEqual(new Vector2Int(1, 2), roomPosition);
            Assert.AreEqual(NodeConnections.All, initRoom.Connections);
        }

        [Test]
        public void PlaceFirstRoomTest()
        {
            Dungeon dungeon = InitDungeon(out var initRoom);
            
            var room = new Room(new Vector2Int(0, 1), NodeConnections.All);
            
            bool result = dungeon.TryPlaceRoom(room, initRoom);
            Assert.IsTrue(result);
        }

        [Test]
        public void PlaceRoomTest()
        {
            Dungeon dungeon = SetupBasicDungeon();
            Room testConnectedRoom = new Room(new Vector2Int(-1, 1), NodeConnections.None);
            
            var room = new Room(new Vector2Int(-1, 2), NodeConnections.Down | NodeConnections.Right);
            
            bool result = dungeon.TryPlaceRoom(room, testConnectedRoom);
            Assert.IsTrue(result);
        }

        [Test]
        public void AdjacencyTest()
        {
            Dungeon dungeon = SetupBasicDungeon();
            Room testConnectedRoom = new Room(new Vector2Int(-1, 1), NodeConnections.None);
            
            var room = new Room(new Vector2Int(0, 2), NodeConnections.Down | NodeConnections.Right);
            
            bool result = dungeon.TryPlaceRoom(room, testConnectedRoom);
            Assert.IsFalse(result);
        }
        
        [Test]
        public void ConnectionTest()
        {
            Dungeon dungeon = SetupBasicDungeon();
            Room testConnectedRoom = new Room(new Vector2Int(-1, 1), NodeConnections.None);
            
            var room = new Room(new Vector2Int(0, 2), NodeConnections.Down | NodeConnections.Right);
            
            bool result = dungeon.TryPlaceRoom(room, testConnectedRoom);
            Assert.IsFalse(result);
        }
        
        [Test]
        public void OverlapTest()
        {
            Dungeon dungeon = SetupBasicDungeon();
            Room testConnectedRoom = new Room(new Vector2Int(-1, 1), NodeConnections.None);
            
            var room = new Room(new Vector2Int(0, 1), NodeConnections.Down | NodeConnections.Right);
            
            bool result = dungeon.TryPlaceRoom(room, testConnectedRoom);
            Assert.IsFalse(result);
        }

        [Test]
        public void GetAvailablePlacesTest()
        {
            Dungeon dungeon = SetupBasicDungeon();
            Room testConnectedRoom = new Room(new Vector2Int(-1, 1), NodeConnections.None);
            
            var connections = NodeConnections.Down | NodeConnections.Right;
            
            var availablePlaces = dungeon.GetAvailablePlacesAt(testConnectedRoom.Position, connections);
            Assert.AreEqual(1, availablePlaces.Length);
            Assert.AreEqual(new Vector2Int(-1, 2), availablePlaces[0]);
            
            availablePlaces = dungeon.GetAvailablePlacesAt(testConnectedRoom.Position, NodeConnections.Left);
            Assert.AreEqual(0, availablePlaces.Length);
            
            availablePlaces = dungeon.GetAvailablePlacesAt(new Vector2Int(0, 0), NodeConnections.All);
            Assert.AreEqual(3, availablePlaces.Length);
        } 
        
        private Dungeon InitDungeon(out Room initRoom)
        {
            Dungeon dungeon = new Dungeon();
            initRoom = dungeon.InitRoom(Vector2Int.zero);
            return dungeon;
        }
        
        private Dungeon SetupBasicDungeon()
        {
            Dungeon dungeon = new Dungeon();
            
            var initRoom = dungeon.InitRoom(Vector2Int.zero);
            var topRoomConnections = new Room(new Vector2Int(0, 1), NodeConnections.Left | NodeConnections.Up | NodeConnections.Down);
            var topLeftRoom = new Room(new Vector2Int(-1, 1), NodeConnections.Right | NodeConnections.Up);
            
            dungeon.TryPlaceRoom(topRoomConnections, initRoom);
            dungeon.TryPlaceRoom(topLeftRoom, topRoomConnections);
            
            return dungeon;
        }
    }
}