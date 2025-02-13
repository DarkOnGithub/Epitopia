using System.Collections.Generic;
using UnityEngine;
using World;
using World.Blocks;

namespace Entities
{
    public class PathFinder
    {
        private AbstractWorld _worldIn;

        private const int NORMAL_COST = 1;
        private const int JUMP_COST = 2; // jump moves cost more than normal moves

        public PathFinder(AbstractWorld worldIn)
        {
            _worldIn = worldIn;
        }

        public List<Vector2Int> FindPath(Vector2 startPosition, Vector2 endPosition)
        {
            var start = Vector2Int.RoundToInt(startPosition);
            var end = Vector2Int.RoundToInt(endPosition);
            
            List<Node> openSet = new List<Node>();
            HashSet<Node> closedSet = new HashSet<Node>();

            Node startNode = new Node(start);
            startNode.G = 0;
            startNode.H = Heuristic(start, end);
            openSet.Add(startNode);

            Node bestNodeSoFar = startNode; 

            while (openSet.Count > 0)
            {
                Node current = GetLowestF(openSet);

                if (current.H < bestNodeSoFar.H)
                    bestNodeSoFar = current;

                if (current.Position == end)
                    return RetracePath(startNode, current);

                openSet.Remove(current);
                closedSet.Add(current);

                foreach (var neighborInfo in GetNeighbors(current.Position))
                {
                    Vector2Int neighborPos = neighborInfo.Position;
                    int moveCost = neighborInfo.MoveCost;

                    if (closedSet.Contains(new Node(neighborPos)) || !IsAccessible(neighborPos, neighborInfo.IsJump))
                        continue;

                    int tentativeG = current.G + moveCost;
                    Node neighborNode = openSet.Find(n => n.Position == neighborPos);
                    if (neighborNode == null)
                    {
                        neighborNode = new Node(neighborPos);
                        neighborNode.G = tentativeG;
                        neighborNode.H = Heuristic(neighborPos, end);
                        neighborNode.Parent = current;
                        openSet.Add(neighborNode);
                    }
                    else if (tentativeG < neighborNode.G)
                    {
                        neighborNode.G = tentativeG;
                        neighborNode.Parent = current;
                    }
                }
            }

            return RetracePath(startNode, bestNodeSoFar);
        }


        private bool IsAccessible(Vector2Int pos, bool isJump)
        {
            if(!_worldIn.ServerHandler.TryGetBlockFromWorldPosition(pos, out var block))
                return false;

            return !BlockRegistry.GetBlock(block.Id).Properties.IsSolid;
        }

        private IEnumerable<NeighborInfo> GetNeighbors(Vector2Int pos)
        {
            yield return new NeighborInfo(new Vector2Int(pos.x - 1, pos.y), NORMAL_COST, false);
            yield return new NeighborInfo(new Vector2Int(pos.x + 1, pos.y), NORMAL_COST, false);
            yield return new NeighborInfo(new Vector2Int(pos.x, pos.y - 1), NORMAL_COST, false);
            yield return new NeighborInfo(new Vector2Int(pos.x, pos.y + 1), JUMP_COST, true);
        }

        private int Heuristic(Vector2Int a, Vector2Int b)
        {
            return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
        }

        /// <summary>
        private Node GetLowestF(List<Node> openSet)
        {
            Node lowest = openSet[0];
            foreach (var node in openSet)
                if (node.F < lowest.F)
                    lowest = node;
            return lowest;
        }


        private List<Vector2Int> RetracePath(Node start, Node end)
        {
            List<Vector2Int> path = new List<Vector2Int>();
            Node current = end;
            while (current != null)
            {
                path.Add(current.Position);
                current = current.Parent;
            }
            path.Reverse();
            return path;
        }

        /// <summary>
        /// Stores information about neighbor nodes including move cost and jump flag.
        /// </summary>
        private struct NeighborInfo
        {
            public Vector2Int Position;
            public int MoveCost;
            public bool IsJump;

            public NeighborInfo(Vector2Int pos, int cost, bool isJump)
            {
                Position = pos;
                MoveCost = cost;
                IsJump = isJump;
            }
        }

        /// <summary>
        /// Internal class representing a node in the pathfinding grid.
        /// </summary>
        private class Node
        {
            public Vector2Int Position;
            public int G; // cost from start
            public int H; // heuristic cost to destination
            public Node Parent;

            public int F => G + H;

            public Node(Vector2Int pos)
            {
                Position = pos;
                G = int.MaxValue;
                H = 0;
                Parent = null;
            }

            public override bool Equals(object obj)
            {
                if (obj is Node other)
                    return Position.Equals(other.Position);
                return false;
            }

            public override int GetHashCode()
            {
                return Position.GetHashCode();
            }
        }
    }
}