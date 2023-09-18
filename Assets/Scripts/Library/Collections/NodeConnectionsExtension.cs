using System;
using UnityEngine;

namespace Library.Collections
{
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

        public static NodeConnections Invert(this NodeConnections connections)
        {
            NodeConnections result = NodeConnections.None;
            
            if ((connections & NodeConnections.Up) != 0)
                result |= NodeConnections.Down;
            
            if ((connections & NodeConnections.Down) != 0)
                result |= NodeConnections.Up;
            
            if ((connections & NodeConnections.Right) != 0)
                result |= NodeConnections.Left;
            
            if ((connections & NodeConnections.Left) != 0)
                result |= NodeConnections.Right;

            return result;
        }

        public static bool TryGetNodeConnection(Vector2Int node1, Vector2Int node2, out NodeConnections connection)
        {
            return TryGetNodeConnection(node1 - node2, out connection);
        }

        public static NodeConnections ClampRange(this NodeConnections connections) => connections & NodeConnections.All;

        public static bool TryGetNodeConnection(Vector2Int direction, out NodeConnections connection)
        {
            connection = NodeConnections.None;
            
            if (direction.Equals(Vector2Int.up))
            {
                connection = NodeConnections.Up;
                return true;
            }

            if (direction.Equals(Vector2Int.right))
            {
                connection = NodeConnections.Right;
                return true;
            }

            if (direction.Equals(Vector2Int.down))
            {
                connection = NodeConnections.Down;
                return true;
            }

            if (direction.Equals(Vector2Int.left))
            {
                connection = NodeConnections.Left;
                return true;
            }

            return false;
        }
        
        public static NodeConnections ToSingleNodeConnection(this Vector2Int direction)
        {
            if(!TryGetNodeConnection(direction, out NodeConnections connection))
                throw new AggregateException("Unknown direction");

            return connection;
        }
    }
}