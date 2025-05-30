using System;
using System.Collections.Generic;
using GamePlay.GridSystem;
using GridSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace GamePlay.PathFinding
{
    public class PathFinding : MonoBehaviour
    {
        private const int MOVE_STRAIGHT_COST = 10;
        private const int MOVE_DIAGONAL_COST = 14;
        [SerializeField] private Transform gridDebugObjectPrefab;
        [SerializeField] private LayerMask obstaclesLayerMask;
        private int width;
        private int height;
        private float cellSize;
        private GridSystem<PathNode> gridSystem;
        public static PathFinding Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        public void SetUp(int width, int height, float cellSize)
        {
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;
            gridSystem = new GridSystem<PathNode>(this.width, this.height, this.cellSize,
                (GridSystem<PathNode> g, GridPosition gridPosition) => new PathNode(gridPosition));
            gridSystem.CreateDebugObjects(gridDebugObjectPrefab);

            for (int x = 0; x < this.width; x++)
            {
                for (int z = 0; z < this.height; z++)
                {
                    GridPosition gridPosition = new GridPosition(x, z);
                    Vector3 worldPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
                    float rayCastOffsetDistance = .5f;
                    if (Physics.Raycast(worldPosition + Vector3.down * rayCastOffsetDistance, Vector3.up,
                            rayCastOffsetDistance * 2, obstaclesLayerMask))
                    {
                        GetNode(x, z).SetIsWalkable(false);
                    }
                }
            }
        }

        public List<GridPosition> FindPath(GridPosition startPosition, GridPosition endPosition, out int pathLength)
        {
            List<PathNode> openList = new List<PathNode>();
            List<PathNode> closeList = new List<PathNode>();
            PathNode startNode = gridSystem.GetGridObject(startPosition);
            PathNode endNode = gridSystem.GetGridObject(endPosition);
            openList.Add(startNode);
            for (int x = 0; x < gridSystem.GetWidth(); x++)
            {
                for (int z = 0; z < gridSystem.GetHeight(); z++)
                {
                    GridPosition gridPosition = new GridPosition(x, z);
                    PathNode pathNode = gridSystem.GetGridObject(gridPosition);
                    pathNode.SetGCost(int.MaxValue);
                    pathNode.SetHCost(0);
                    pathNode.CalculateFCost();
                    pathNode.ResetCameFromPathNode();
                }
            }

            startNode.SetGCost(0);
            startNode.SetHCost(CalculateDistance(startPosition, endPosition));
            startNode.CalculateFCost();
            while (openList.Count > 0)
            {
                PathNode currentNode = GetLowestFCostPath(openList);
                if (currentNode == endNode)
                {
                    // final node
                    pathLength = endNode.GetHCost();
                    return CalculatePath(endNode);
                }

                openList.Remove(currentNode);
                closeList.Add(currentNode);
                foreach (PathNode neighbourNode in GetNeighborList(currentNode))
                {
                    if (closeList.Contains(neighbourNode))
                    {
                        continue;
                    }

                    if (!neighbourNode.IsWalkable())
                    {
                        closeList.Add(neighbourNode);
                        continue;
                    }

                    int tentativeGCost = currentNode.GetGCost() +
                                         CalculateDistance(currentNode.GetGridPosition(),
                                             neighbourNode.GetGridPosition());
                    if (tentativeGCost < neighbourNode.GetGCost())
                    {
                        neighbourNode.SetCameFromPathNode(currentNode);
                        neighbourNode.SetGCost(tentativeGCost);
                        neighbourNode.SetHCost(CalculateDistance(neighbourNode.GetGridPosition(), endPosition));
                        neighbourNode.CalculateFCost();
                        if (!openList.Contains(neighbourNode))
                        {
                            openList.Add(neighbourNode);
                        }
                    }
                }
            }

            // No path found
            pathLength = 0;
            return null;
        }

        private List<GridPosition> CalculatePath(PathNode endNode)
        {
            List<PathNode> pathNodeList = new List<PathNode>();
            pathNodeList.Add(endNode);
            PathNode currentNode = endNode;
            while (currentNode.GetCameFromPathNode() != null)
            {
                pathNodeList.Add(currentNode.GetCameFromPathNode());
                currentNode = currentNode.GetCameFromPathNode();
            }

            pathNodeList.Reverse();

            List<GridPosition> gridPositionList = new List<GridPosition>();

            foreach (PathNode pathNode in pathNodeList)
            {
                gridPositionList.Add(pathNode.GetGridPosition());
            }

            return gridPositionList;
        }

        public int CalculateDistance(GridPosition gridPositionA, GridPosition gridPositionB)
        {
            GridPosition gridPositionDistance = gridPositionB - gridPositionA;
            int xDistance = Mathf.Abs(gridPositionDistance.x);
            int zDistance = Mathf.Abs(gridPositionDistance.z);
            int remainingDistance = Mathf.Abs(xDistance - zDistance);
            return MOVE_DIAGONAL_COST * Mathf.Min(xDistance + zDistance) * MOVE_STRAIGHT_COST * remainingDistance;
        }

        private PathNode GetLowestFCostPath(List<PathNode> pathNodeList)
        {
            PathNode lowestFCostPathNode = pathNodeList[0];
            for (int i = 0; i < pathNodeList.Count; i++)
            {
                if (pathNodeList[i].GetFCost() < lowestFCostPathNode.GetFCost())
                {
                    lowestFCostPathNode = pathNodeList[i];
                }
            }

            return lowestFCostPathNode;
        }

        private PathNode GetNode(int x, int z)
        {
            return gridSystem.GetGridObject(new GridPosition(x, z));
        }

        private List<PathNode> GetNeighborList(PathNode currentNode)
        {
            List<PathNode> neighborList = new List<PathNode>();
            GridPosition gridPosition = currentNode.GetGridPosition();

            if (gridPosition.x - 1 >= 0)
            {
                //left
                neighborList.Add(GetNode(gridPosition.x - 1, gridPosition.z + 0));

                if (gridPosition.z - 1 >= 0)
                {
                    //leftDown
                    neighborList.Add(GetNode(gridPosition.x - 1, gridPosition.z - 1));
                }

                if (gridPosition.z + 1 < gridSystem.GetHeight())
                {
                    //leftUp
                    neighborList.Add(GetNode(gridPosition.x - 1, gridPosition.z + 1));
                }
            }

            if (gridPosition.x + 1 < gridSystem.GetWidth())
            {
                //right
                neighborList.Add(GetNode(gridPosition.x + 1, gridPosition.z + 0));


                if (gridPosition.z - 1 >= 0)
                {
                    //right down
                    neighborList.Add(GetNode(gridPosition.x + 1, gridPosition.z - 1));
                }

                if (gridPosition.z + 1 < gridSystem.GetHeight())
                {
                    //right up
                    neighborList.Add(GetNode(gridPosition.x + 1, gridPosition.z + 1));
                }
            }

            if (gridPosition.z - 1 >= 0)
            {
                //down
                neighborList.Add(GetNode(gridPosition.x + 0, gridPosition.z - 1));
            }

            if (gridPosition.z + 1 < gridSystem.GetHeight())
            {
                //up
                neighborList.Add(GetNode(gridPosition.x + 0, gridPosition.z + 1));
            }


            return neighborList;
        }
        public bool IsWalkableGridPosition(GridPosition gridPosition) => gridSystem.GetGridObject(gridPosition).IsWalkable();
        public bool HasPath(GridPosition startGridPosition, GridPosition endGridPosition) =>  FindPath(startGridPosition, endGridPosition,out int pathLength) != null;

        public int GetPathLength(GridPosition startGridPosition, GridPosition endGridPosition)
        {
            FindPath(startGridPosition, endGridPosition, out int pathLength);
            return pathLength;
        }
        
    }
}