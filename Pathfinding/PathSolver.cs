﻿using System;
using System.Collections.Generic;
using Pathfinding.Datastructures;
using System.Threading;

namespace Pathfinding
{
    public class PathSolver<PathNodeType> where PathNodeType : IPathNode
    {
        private void TryQueueNewTile(IPathNode pNewNode, PathLink pLink, AStarStack pNodesToVisit, IPathNode pGoal)
        {
            IPathNode previousNode = pLink.GetOtherNode(pNewNode);
            float linkDistance = pLink.distance;
            float newPathCost = previousNode.pathCostHere + pNewNode.baseCost + linkDistance;
            
            if (pNewNode.linkLeadingHere == null || (pNewNode.pathCostHere > newPathCost)) {
                pNewNode.distanceToGoal = pNewNode.DistanceTo(pGoal) * 2f;
                pNewNode.pathCostHere = newPathCost;
                pNewNode.linkLeadingHere = pLink;
                pNodesToVisit.Push(pNewNode);
            }
        }
	
        public Path<PathNodeType> FindPath(IPathNode pStart, IPathNode pGoal, IPathNetwork<PathNodeType> pNetwork, bool pReset)
        {
#if DEBUG
			if(pNetwork == null) {
				throw new Exception("pNetwork is null");
			}
#endif
			if (pStart == null || pGoal == null) {
				return new Path<PathNodeType>(new PathNodeType[] {}, 0f, PathStatus.DESTINATION_UNREACHABLE, 0);
			}

			if (pStart == pGoal) {
				return new Path<PathNodeType>(new PathNodeType[] {}, 0f, PathStatus.ALREADY_THERE, 0);
			}

            int testCount = 0;
			
			if(pReset) {
            	pNetwork.Reset();
			}
			
            pStart.isStartNode = true;
            pGoal.isGoalNode = true;
            List<PathNodeType> resultNodeList = new List<PathNodeType>();
            
            IPathNode currentNode = pStart;
            IPathNode goalNode = pGoal;
            
            currentNode.visited = true;
            currentNode.linkLeadingHere = null;
            AStarStack nodesToVisit = new AStarStack();
            PathStatus pathResult = PathStatus.NOT_CALCULATED_YET;
            testCount = 1;
            
            while (pathResult == PathStatus.NOT_CALCULATED_YET) {
                foreach (PathLink l in currentNode.links) {
                    IPathNode otherNode = l.GetOtherNode(currentNode);
                    
                    if (!otherNode.visited) {
                        TryQueueNewTile(otherNode, l, nodesToVisit, goalNode);
                    }
                }
                
                if (nodesToVisit.Count == 0) {
                    pathResult = PathStatus.DESTINATION_UNREACHABLE;
                }
                else {
                    currentNode = nodesToVisit.Pop();
                    testCount++;

                    // Console.WriteLine("testing new node: " + (currentNode as TileNode).localPoint);
                    currentNode.visited = true;
                    
                    if (currentNode == goalNode) {
                        pathResult = PathStatus.FOUND_GOAL;
                    }
                }
            }
            
            // Path finished, collect
            float tLength = 0;

            if (pathResult == PathStatus.FOUND_GOAL) {
                tLength = currentNode.pathCostHere;
                
                while (currentNode != pStart) {
                    resultNodeList.Add((PathNodeType)currentNode);
                    currentNode = currentNode.linkLeadingHere.GetOtherNode(currentNode);
                }
                
                resultNodeList.Add((PathNodeType)currentNode);
                resultNodeList.Reverse();
            }
            
            return new Path<PathNodeType>(resultNodeList.ToArray(), tLength, pathResult, testCount);
        }
		
		/*
        public delegate void PathWasFound(Path<PathNodeType> newPath);

        public void FindPathAsync(IPathNode pStart, IPathNode pGoal, IPathNetwork<PathNodeType> pNetwork, PathWasFound pOnPathWasFound, bool pReset)
        {
           	//ThreadPool.QueueUserWorkItem(o => {
                Path<PathNodeType> path = FindPath(pStart, pGoal, pNetwork, pReset);
				pOnPathWasFound(path);
           	//});
        }
        */
    }
}
