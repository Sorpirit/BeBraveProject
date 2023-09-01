using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Library.Collections
{
    public class Grid<T>
    {
        private readonly Dictionary<Vector2Int, T> _dictionary = new();
        private readonly Dictionary<Vector2Int, NodeConnections> _connections = new();

        public void Add(Vector2Int position, T value)
        {
            Assert.IsTrue(!ContainsAt(position), "Node is already added");

            _dictionary.Add(position, value);
            _connections.Add(position, NodeConnections.None);
        }

        public bool Remove(Vector2Int position)
        {
            if (!ContainsAt(position))
                return false;

            var nodeConnections = _connections[position];
            if (nodeConnections != NodeConnections.None)
                Disconnect(position, nodeConnections);

            _dictionary.Remove(position);
            _connections.Remove(position);

            return true;
        }

        public T Get(Vector2Int position) => _dictionary[position];

        public T this[Vector2Int i] => _dictionary[i];

        public bool TryGet(Vector2Int position, out T value)
        {
            value = default;
            if (!ContainsAt(position))
                return false;

            value = Get(position);
            return true;
        }

        public bool ContainsAt(Vector2Int position) => _dictionary.ContainsKey(position);

        /// <summary>
        /// Connect node to all requested nodes. On failure will assert
        /// </summary>
        public void Connect(Vector2Int position, NodeConnections neighbourConnections)
        {
            Assert.IsTrue(ContainsAt(position), "Cant connect from non existent node");
            Assert.IsTrue(neighbourConnections != NodeConnections.None, "Cant connect node to None neighbour");

            var singleNodeConnections = neighbourConnections.Split();
            ConnectInternal(position, singleNodeConnections, out int maidConnections);
            Assert.AreEqual(singleNodeConnections.Length, maidConnections, "Unable to connect nodes");
        }

        /// <summary>
        /// Disconnects from all requested nodes. On failure will assert
        /// </summary>
        public void Disconnect(Vector2Int position, NodeConnections neighbourConnections)
        {
            Assert.IsTrue(ContainsAt(position), "Cant disconnect from non existent node");
            Assert.IsTrue(neighbourConnections != NodeConnections.None, "Cant disconnect from none neighbours");

            var singleNodeConnections = neighbourConnections.Split();
            DisconnectInternal(position, singleNodeConnections, out int disconnectedNodes);
            Assert.AreEqual(singleNodeConnections.Length, disconnectedNodes, "Unable to disconnect nodes");
        }

        /// <summary>
        /// Will return true if was able to connect at least to one neighbour. Otherwise will return false
        /// </summary>
        public bool TryConnectAny(Vector2Int position, NodeConnections neighbourConnections)
        {
            if (!ContainsAt(position) || neighbourConnections == NodeConnections.None)
                return false;

            var singleNodeConnections = neighbourConnections.Split();
            ConnectInternal(position, singleNodeConnections, out int maidConnections);
            return maidConnections > 0;
        }

        /// <summary>
        /// Will return true if was able to disconnect from at least to one neighbour. Otherwise will return false
        /// </summary>
        public bool TryDisconnect(Vector2Int position, NodeConnections neighbourConnections)
        {
            if (!ContainsAt(position) || neighbourConnections == NodeConnections.None)
                return false;

            var singleNodeConnections = neighbourConnections.Split();
            DisconnectInternal(position, singleNodeConnections, out int disconnectedNodes);
            return disconnectedNodes > 0;
        }

        /// <summary>
        /// Return all available requested neighbours
        /// </summary>
        public ReadOnlySpan<Vector2Int> GetNeighbours(Vector2Int position,
            NodeConnections connectionses = NodeConnectionsExtension.AllDirections)
        {
            Span<Vector2Int> span = stackalloc Vector2Int[4];
            GetNeighboursInternal(position, span, connectionses, out int count);

            if (count == 0)
                return ReadOnlySpan<Vector2Int>.Empty;

            return new ReadOnlySpan<Vector2Int>(span.Slice(0, count).ToArray());
        }


        public ReadOnlySpan<T> GetNeighbourValues(Vector2Int position,
            NodeConnections connectionses = NodeConnectionsExtension.AllDirections)
        {
            var neighboursPosition = GetNeighbours(position, connectionses);
            var array = new T[neighboursPosition.Length];

            for (int i = 0; i < neighboursPosition.Length; i++)
            {
                array[i] = Get(neighboursPosition[i]);
            }

            return new ReadOnlySpan<T>(array);
        }

        private void ConnectInternal(Vector2Int position, ReadOnlySpan<NodeConnections> neighbourConnections,
            out int connectedNodes)
        {
            connectedNodes = 0;
            if (!ContainsAt(position) || neighbourConnections.Length == 0)
                return;

            foreach (var connection in neighbourConnections)
            {
                Assert.IsTrue(connection.IsSingular());
                var neighbour = position + connection.GetDirection();

                if (!ContainsAt(neighbour))
                    continue;

                if (_connections[position].HasFlag(connection))
                    continue;

                connectedNodes++;
                _connections[position] |= connection;
                _connections[neighbour] |= connection.Invert();
            }
        }

        private void DisconnectInternal(Vector2Int position, ReadOnlySpan<NodeConnections> neighbourConnections,
            out int disconnectedNodes)
        {
            disconnectedNodes = 0;
            if (!ContainsAt(position) || neighbourConnections.Length == 0)
                return;

            foreach (var connection in neighbourConnections)
            {
                Assert.IsTrue(connection.IsSingular());
                var neighbour = position + connection.GetDirection();

                if (!ContainsAt(neighbour))
                    continue;

                if (!_connections[position].HasFlag(connection))
                    continue;

                disconnectedNodes++;
                _connections[position] &= ~connection;
                _connections[neighbour] &= ~connection.Invert();
            }
        }

        private void GetNeighboursInternal(Vector2Int position, Span<Vector2Int> neighbours,
            NodeConnections connections, out int count)
        {
            count = 0;

            if (!ContainsAt(position) || neighbours.Length != 4)
                return;

            var n = _connections[position] & connections;
            if (n == NodeConnections.None)
                return;

            var nDirections = n.GetDirections();
            count = nDirections.Length;
            for (int i = 0; i < nDirections.Length; i++)
            {
                var neighbourPosition = position + nDirections[i];
                Assert.IsTrue(ContainsAt(neighbourPosition));
                neighbours[i] = neighbourPosition;
            }
        }
    }

    [Flags]
    public enum NodeConnections
    {
        None = 0,
        Up = 1,
        Right = 2,
        Down = 4,
        Left = 8
    }

    public static class NodeConnectionsExtension
    {
        public const NodeConnections AllDirections =
            NodeConnections.Up | NodeConnections.Right | NodeConnections.Down | NodeConnections.Left;

        public const NodeConnections Vertical =
            NodeConnections.Up | NodeConnections.Down;

        public const NodeConnections Horizontal =
            NodeConnections.Right | NodeConnections.Left;

        public static bool IsSingular(this NodeConnections nodeConnections) => nodeConnections switch
        {
            NodeConnections.Up => true,
            NodeConnections.Right => true,
            NodeConnections.Down => true,
            NodeConnections.Left => true,
            _ => false
        };

        public static Vector2Int GetDirection(this NodeConnections connections) => connections switch
        {
            NodeConnections.Up => Vector2Int.up,
            NodeConnections.Right => Vector2Int.right,
            NodeConnections.Down => Vector2Int.down,
            NodeConnections.Left => Vector2Int.left,
            _ => throw new ArgumentException("Unknown direction: " + connections)
        };

        public static ReadOnlySpan<Vector2Int> GetDirections(this NodeConnections connections)
        {
            var singleConnections = Split(connections);
            var directions = new Vector2Int[singleConnections.Length];
            for (int i = 0; i < singleConnections.Length; i++)
            {
                directions[i] = singleConnections[i].GetDirection();
            }

            return new ReadOnlySpan<Vector2Int>(directions);
        }

        public static ReadOnlySpan<NodeConnections> Split(this NodeConnections connections) => connections switch
        {
            NodeConnections.Up => new[] { NodeConnections.Up },
            NodeConnections.Right => new[] { NodeConnections.Right },
            NodeConnections.Down => new[] { NodeConnections.Down },
            NodeConnections.Left => new[] { NodeConnections.Left },

            NodeConnections.Up | NodeConnections.Right => new[] { NodeConnections.Up, NodeConnections.Right },
            NodeConnections.Up | NodeConnections.Down => new[] { NodeConnections.Up, NodeConnections.Down },
            NodeConnections.Up | NodeConnections.Left => new[] { NodeConnections.Up, NodeConnections.Left },
            NodeConnections.Right | NodeConnections.Down => new[] { NodeConnections.Right, NodeConnections.Down },
            NodeConnections.Right | NodeConnections.Left => new[] { NodeConnections.Right, NodeConnections.Left },
            NodeConnections.Down | NodeConnections.Left => new[] { NodeConnections.Down, NodeConnections.Left },

            NodeConnections.Up | NodeConnections.Right | NodeConnections.Down => new[]
                { NodeConnections.Up, NodeConnections.Right, NodeConnections.Down },
            NodeConnections.Up | NodeConnections.Right | NodeConnections.Left => new[]
                { NodeConnections.Up, NodeConnections.Right, NodeConnections.Left },
            NodeConnections.Up | NodeConnections.Down | NodeConnections.Left => new[]
                { NodeConnections.Up, NodeConnections.Down, NodeConnections.Left },
            NodeConnections.Right | NodeConnections.Down | NodeConnections.Left => new[]
                { NodeConnections.Right, NodeConnections.Down, NodeConnections.Left },

            AllDirections => new[]
                { NodeConnections.Up, NodeConnections.Right, NodeConnections.Down, NodeConnections.Left },
            _ => throw new ArgumentException("Unknown direction: " + connections)
        };

        public static NodeConnections Invert(this NodeConnections connections) => connections switch
        {
            NodeConnections.Up => NodeConnections.Down,
            NodeConnections.Right => NodeConnections.Left,
            NodeConnections.Down => NodeConnections.Up,
            NodeConnections.Left => NodeConnections.Right,
            _ => throw new ArgumentException("Unable to invert combined direction: " + connections)
        };

        public static NodeConnections ToSingleNodeConnection(this Vector2Int direction)
        {
            if (direction.Equals(Vector2Int.up))
                return NodeConnections.Up;

            if (direction.Equals(Vector2Int.right))
                return NodeConnections.Right;

            if (direction.Equals(Vector2Int.down))
                return NodeConnections.Down;

            if (direction.Equals(Vector2Int.left))
                return NodeConnections.Left;

            throw new AggregateException("Unknown direction");
        }
    }
}