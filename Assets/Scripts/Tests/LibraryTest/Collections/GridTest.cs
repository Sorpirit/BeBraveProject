using System;
using Library.Collections;
using NUnit.Framework;
using UnityEngine;

public class GridTest
{
    [Test]
    public void GridAddTest()
    {
        Grid<int> grid = new Grid<int>();

        Vector2Int position = new Vector2Int();
        int value = 2016;
        grid.Add(position, value);
        Assert.AreEqual(value, grid.Get(position));
        
        int secondValue = 2003;
        Vector2Int secondPosition = new Vector2Int(0, 1);
        grid.Add(secondPosition, secondValue);
        Assert.AreEqual(secondValue, grid[secondPosition]);

        var exception = Assert.Catch<UnityEngine.Assertions.AssertionException>(() => grid.Add(position, 0));
        Assert.True(exception.Message.Contains("Node is already added"));
    }

    [Test]
    public void GridRemoveTest()
    {
        Grid<int> grid = new Grid<int>();

        Vector2Int position = new Vector2Int();
        int value = 2016;
        grid.Add(position, value);
        
        int secondValue = 2003;
        Vector2Int secondPosition = new Vector2Int(0, 1);
        grid.Add(secondPosition, secondValue);

        Assert.True(grid.ContainsAt(position));
        bool result = grid.Remove(position);
        Assert.True(result);
        Assert.False(grid.ContainsAt(position));
        Assert.False(grid.Remove(position));
        
        int thirdValue = 0;
        Vector2Int thirdPosition = new Vector2Int(1, 1);
        grid.Add(thirdPosition, thirdValue);
        
        grid.Connect(secondPosition, NodeConnections.Right);
        Assert.That(grid.GetConnectedNeighbours(thirdPosition).ToArray(), Is.EquivalentTo(new [] { secondPosition }));

        grid.Remove(secondPosition);
        
        Assert.That(grid.GetConnectedNeighbours(thirdPosition).ToArray(), Is.EquivalentTo( Array.Empty<Vector2Int>() ));
    }

    [Test]
    public void GridConnectionTest()
    {
        Grid<int> grid = new Grid<int>();

        Vector2Int position = new Vector2Int();
        int value = 2016;
        grid.Add(position, value);
        grid.Add(new Vector2Int(0, 1), 1);
        grid.Add(new Vector2Int(1, 1), 2);
        grid.Add(new Vector2Int(1, 0), 3);
        grid.Add(new Vector2Int(1, -1), 4);
        grid.Add(new Vector2Int(0, -1), 5);
        
        grid.Connect(position, NodeConnections.Up);
        grid.Connect(position, NodeConnections.Right | NodeConnections.Down);
        
        Assert.That(grid.GetConnectedNeighbourValues(position).ToArray(), Is.EquivalentTo(new [] {1, 3, 5}));

        bool bResult = grid.TryConnectAny(new Vector2Int(1, 0), NodeConnections.Right);
        Assert.False(bResult);
        
        grid.Disconnect(position, NodeConnections.Up);
        
        Assert.That(grid.GetConnectedNeighbourValues(position).ToArray(), Is.EquivalentTo(new [] {3, 5}));
        
        grid.Disconnect(position, NodeConnections.Right | NodeConnections.Down);
        
        Assert.That(grid.GetConnectedNeighbourValues(position).ToArray(), Is.EquivalentTo( Array.Empty<int>()));
        
        bResult = grid.TryDisconnect(new Vector2Int(1, 0), NodeConnections.Right);
        Assert.False(bResult);
    }

    [Test]
    public void GridNeighboursTest()
    {
        Grid<int> grid = new Grid<int>();

        Vector2Int position = new Vector2Int();
        int value = 0;
        grid.Add(position, value);
        grid.Add(new Vector2Int(0, 1), 1);
        grid.Add(new Vector2Int(1, 1), 2);
        grid.Add(new Vector2Int(1, 0), 3);
        grid.Add(new Vector2Int(1, -1), 4);
        grid.Add(new Vector2Int(0, -1), 5);
        
        grid.Connect(position, NodeConnections.Up | NodeConnections.Right | NodeConnections.Down);
        
        Assert.That(grid.GetConnectedNeighbourValues(position, NodeConnections.Up).ToArray(), Is.EquivalentTo(new [] {1}));
        grid.Remove(new Vector2Int(0, 1));
        Assert.That(grid.GetConnectedNeighbourValues(position).ToArray(), Is.EquivalentTo(new [] {3, 5}));
        
        grid.Connect(new Vector2Int(1, 0), NodeConnections.Down | NodeConnections.Up);
        Assert.That(grid.GetConnectedNeighbourValues(new Vector2Int(1, 0), NodeConnections.Down | NodeConnections.Up).ToArray(), Is.EquivalentTo(new [] {2, 4}));
    }
}