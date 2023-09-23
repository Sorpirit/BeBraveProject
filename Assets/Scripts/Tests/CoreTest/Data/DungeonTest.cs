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
            
            RoomCard roomCard = new RoomCard(RoomId.Empty, NodeConnections.All);
            
            bool result = dungeon.PlaceRoom(new Vector2Int(0, 1), roomCard, initRoom, out var room);
            Assert.IsTrue(result);
        }

        [Test]
        public void PlaceRoomTest()
        {
            Dungeon dungeon = SetupBasicDungeon();
            Room testConnectedRoom = new Room(new Vector2Int(-1, 1), NodeConnections.None);
            
            RoomCard roomCard = new RoomCard(RoomId.Empty, NodeConnections.Down | NodeConnections.Right);
            
            bool result = dungeon.PlaceRoom(new Vector2Int(-1, 2), roomCard, testConnectedRoom, out var room);
            Assert.IsTrue(result);
        }

        [Test]
        public void AdjacencyTest()
        {
            Dungeon dungeon = SetupBasicDungeon();
            Room testConnectedRoom = new Room(new Vector2Int(-1, 1), NodeConnections.None);
            
            RoomCard roomCard = new RoomCard(RoomId.Empty, NodeConnections.Down | NodeConnections.Right);
            
            bool result = dungeon.PlaceRoom(new Vector2Int(0, 2), roomCard, testConnectedRoom, out var room);
            Assert.IsFalse(result);
        }
        
        [Test]
        public void ConnectionTest()
        {
            Dungeon dungeon = SetupBasicDungeon();
            Room testConnectedRoom = new Room(new Vector2Int(-1, 1), NodeConnections.None);
            
            RoomCard roomCard = new RoomCard(RoomId.Empty, NodeConnections.Down | NodeConnections.Right);
            
            bool result = dungeon.PlaceRoom(new Vector2Int(-1, 0), roomCard, testConnectedRoom, out var room);
            Assert.IsFalse(result);
        }
        
        [Test]
        public void OverlapTest()
        {
            Dungeon dungeon = SetupBasicDungeon();
            Room testConnectedRoom = new Room(new Vector2Int(-1, 1), NodeConnections.None);
            
            RoomCard roomCard = new RoomCard(RoomId.Empty, NodeConnections.Down | NodeConnections.Right);
            
            bool result = dungeon.PlaceRoom(new Vector2Int(0, 1), roomCard, testConnectedRoom, out var room);
            Assert.IsFalse(result);
        }

        [Test]
        public void GetAvailablePlacesTest()
        {
            Dungeon dungeon = SetupBasicDungeon();
            Room testConnectedRoom = new Room(new Vector2Int(-1, 1), NodeConnections.None);
            
            RoomCard roomCard = new RoomCard(RoomId.Empty, NodeConnections.Down | NodeConnections.Right);
            
            var availablePlaces = dungeon.GetAvailablePlacesAt(testConnectedRoom.Position, roomCard.Connections);
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
            var topRoom = new RoomCard(RoomId.Empty, NodeConnections.Left | NodeConnections.Up | NodeConnections.Down);
            var topLeftRoom = new RoomCard(RoomId.Empty, NodeConnections.Right | NodeConnections.Up);
            
            dungeon.PlaceRoom(new Vector2Int(0, 1), topRoom, initRoom, out var room);
            dungeon.PlaceRoom(new Vector2Int(-1, 1), topLeftRoom, room, out room);
            
            return dungeon;
        }
    }
}