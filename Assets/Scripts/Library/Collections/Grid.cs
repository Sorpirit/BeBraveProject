using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public ReadOnlySpan<Vector2Int> GetConnectedNeighbours(Vector2Int position,
            NodeConnections connections = NodeConnectionsExtension.AllDirections)
        {
            Span<Vector2Int> span = stackalloc Vector2Int[4];
            GetNeighboursInternal(position, span, connections, true, out int count);

            if (count == 0)
                return ReadOnlySpan<Vector2Int>.Empty;

            return new ReadOnlySpan<Vector2Int>(span.Slice(0, count).ToArray());
        }

        /// <summary>
        /// Returns values right away
        /// </summary>
        public ReadOnlySpan<T> GetConnectedNeighbourValues(Vector2Int position,
            NodeConnections connections = NodeConnectionsExtension.AllDirections)
        {
            var neighboursPosition = GetConnectedNeighbours(position, connections);
            var array = new T[neighboursPosition.Length];

            for (int i = 0; i < neighboursPosition.Length; i++)
            {
                array[i] = Get(neighboursPosition[i]);
            }

            return new ReadOnlySpan<T>(array);
        }
        
        /// <summary>
        /// Return all available neighbours even if they are not connected
        /// </summary>
        public ReadOnlySpan<Vector2Int> GetNeighbours(Vector2Int position, NodeConnections connections = NodeConnectionsExtension.AllDirections)
        {
            Span<Vector2Int> span = stackalloc Vector2Int[4];
            GetNeighboursInternal(position, span, connections, false, out int count);

            if (count == 0)
                return ReadOnlySpan<Vector2Int>.Empty;

            var arr = span.Slice(0, count).ToArray();
            return new ReadOnlySpan<Vector2Int>(arr);
        }
        
        /// <summary>
        /// Returns values right away
        /// </summary>
        public ReadOnlySpan<T> GetNeighbourValues(Vector2Int position,
            NodeConnections connections = NodeConnectionsExtension.AllDirections)
        {
            var neighboursPosition = GetNeighbours(position, connections);
            var array = new T[neighboursPosition.Length];

            for (int i = 0; i < neighboursPosition.Length; i++)
            {
                array[i] = Get(neighboursPosition[i]);
            }

            return new ReadOnlySpan<T>(array);
        }

        public ReadOnlyCollection<(Vector2Int nodePosition, NodeConnections freeConnections)> GetEdgeNodes(NodeConnections limitation = NodeConnectionsExtension.AllDirections)
        {
            List<(Vector2Int nodePosition, NodeConnections freeConnections)> edges = new ();

            foreach (var node in _connections)
            {
                NodeConnections freeConnections = ~node.Value;
                NodeConnections availableConnections = freeConnections & limitation;
                if ((availableConnections) != 0) 
                {
                    edges.Add((node.Key, availableConnections));
                }
            }

            return new ReadOnlyCollection<(Vector2Int nodePosition, NodeConnections freeConnections)>(edges);
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
            NodeConnections connections, bool connectedOnly, out int count)
        {
            count = 0;

            if (!ContainsAt(position) || neighbours.Length != 4)
                return;
            
            var n = connections;
            if (connectedOnly)
            {
                n &= _connections[position];
                if (n == NodeConnections.None)
                    return;   
            }

            var nDirections = n.GetDirections();
            foreach (var direction in nDirections)
            {
                var neighbourPosition = position + direction;
                if (!ContainsAt(neighbourPosition))
                {
                    Assert.IsFalse(connectedOnly);
                    continue;
                }
                neighbours[count] = neighbourPosition;
                count++;
            }
        }
    }
}