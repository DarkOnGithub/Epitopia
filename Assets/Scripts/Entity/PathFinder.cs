using System.Collections.Generic;
using UnityEngine;
using World;
using World.Blocks;

namespace Entities
{
    public class PathFinder
    {
        private AbstractWorld _worldIn;

        // Movement costs – you can adjust these to balance your game.
        private const int NORMAL_COST = 1;
        private const int JUMP_COST = 3; // higher cost for a jump move
        private const int FALL_COST = 1; // cost per falling move

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

                    // Only consider nodes that are accessible (non-solid)
                    if (closedSet.Contains(new Node(neighborPos)) || !IsAccessible(neighborPos))
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

            // Return best found path if no exact match was reached.
            return RetracePath(startNode, bestNodeSoFar);
        }

        /// <summary>
        /// Checks if the given tile is accessible (i.e. not blocked by a solid block).
        /// </summary>
        private bool IsAccessible(Vector2Int pos)
        {
            if (!_worldIn.ServerHandler.TryGetBlockFromWorldPosition(pos, out var block))
                return false;
            return !BlockRegistry.GetBlock(block.Id).Properties.IsSolid;
        }

        /// <summary>
        /// Determines whether there is solid ground (a block) immediately beneath the position.
        /// </summary>
        private bool IsGrounded(Vector2Int pos)
        {
            Vector2Int below = new Vector2Int(pos.x, pos.y - 1);
            if (!_worldIn.ServerHandler.TryGetBlockFromWorldPosition(below, out var block))
                return false;
            return BlockRegistry.GetBlock(block.Id).Properties.IsSolid;
        }

        /// <summary>
        /// Generates neighbor moves based on whether the entity is standing or falling.
        /// </summary>
        private IEnumerable<NeighborInfo> GetNeighbors(Vector2Int pos)
        {
            bool grounded = IsGrounded(pos);

            if (grounded)
            {
                // WALKING: horizontal moves from solid ground.
                Vector2Int left = new Vector2Int(pos.x - 1, pos.y);
                Vector2Int right = new Vector2Int(pos.x + 1, pos.y);
                if (IsAccessible(left))
                    yield return new NeighborInfo(left, NORMAL_COST, false);
                if (IsAccessible(right))
                    yield return new NeighborInfo(right, NORMAL_COST, false);

                // JUMPING: allow upward moves only when on solid ground.
                // Jump moves can include straight up or diagonally upward.
                Vector2Int up = new Vector2Int(pos.x, pos.y + 1);
                Vector2Int upLeft = new Vector2Int(pos.x - 1, pos.y + 1);
                Vector2Int upRight = new Vector2Int(pos.x + 1, pos.y + 1);

                // Check that not only the destination is free but also the space above it (simulating head clearance)
                if (IsAccessible(up) && IsAccessible(new Vector2Int(pos.x, pos.y + 2)))
                    yield return new NeighborInfo(up, JUMP_COST, true);
                if (IsAccessible(upLeft) && IsAccessible(new Vector2Int(pos.x - 1, pos.y + 2)))
                    yield return new NeighborInfo(upLeft, JUMP_COST, true);
                if (IsAccessible(upRight) && IsAccessible(new Vector2Int(pos.x + 1, pos.y + 2)))
                    yield return new NeighborInfo(upRight, JUMP_COST, true);
            }
            else
            {
                // FALLING: when not grounded, the entity should fall.
                Vector2Int down = new Vector2Int(pos.x, pos.y - 1);
                if (IsAccessible(down))
                    yield return new NeighborInfo(down, FALL_COST, false);

                // Optionally allow slight horizontal drift while falling.
                Vector2Int downLeft = new Vector2Int(pos.x - 1, pos.y - 1);
                Vector2Int downRight = new Vector2Int(pos.x + 1, pos.y - 1);
                if (IsAccessible(downLeft))
                    yield return new NeighborInfo(downLeft, FALL_COST, false);
                if (IsAccessible(downRight))
                    yield return new NeighborInfo(downRight, FALL_COST, false);
            }
        }

        private int Heuristic(Vector2Int a, Vector2Int b)
        {
            // Manhattan distance is still a good heuristic for grid-based movement.
            return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
        }

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
        /// Encapsulates neighbor information including move cost and whether the move is a jump.
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
        /// Represents a node in the pathfinding grid.
        /// </summary>
        private class Node
        {
            public Vector2Int Position;
            public int G; // cost from the start node
            public int H; // heuristic cost to the destination
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
