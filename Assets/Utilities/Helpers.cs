using System.Collections.Generic;
using UnityEngine;

public static class Helpers
{
    public static Dictionary<Vector3, List<Vector3>> ValidMovementPositionsToAdjacencyList(Vector3 startingPosition, HashSet<Vector3> validMovementPositions)
    {
        Dictionary<Vector3, List<Vector3>> result = new Dictionary<Vector3, List<Vector3>>();
        result[startingPosition] = FindAdjacentPositions(startingPosition, validMovementPositions);
        foreach (Vector3 position in validMovementPositions)
        {
            result[position] = FindAdjacentPositions(position, validMovementPositions);
        }
        return result;
    }
    private static List<Vector3> FindAdjacentPositions(Vector3 startingPosition, HashSet<Vector3> validMovementPositions)
    {
        List<Vector3> adjacentPositions = new List<Vector3>();
        Vector3 pos1 = startingPosition + Vector3.up;
        Vector3 pos2 = startingPosition + Vector3.down;
        Vector3 pos3 = startingPosition + Vector3.left;
        Vector3 pos4 = startingPosition + Vector3.right;

        if (validMovementPositions.Contains(pos1)) adjacentPositions.Add(pos1);
        if (validMovementPositions.Contains(pos2)) adjacentPositions.Add(pos2);
        if (validMovementPositions.Contains(pos3)) adjacentPositions.Add(pos3);
        if (validMovementPositions.Contains(pos4)) adjacentPositions.Add(pos4);

        return adjacentPositions;
    }
    public static List<Vector3> BFS(Dictionary<Vector3, List<Vector3>> graph, Vector3 startPosition, Vector3 endPosition)
    {

        Queue<List<Vector3>> queue = new Queue<List<Vector3>>();
        List<Vector3> path = new List<Vector3>();

        if (!graph.ContainsKey(endPosition))
        {
            Debug.LogError("Target Is Uncreachable From Current Position");
            return path;
        }

        queue.Enqueue(new List<Vector3> { startPosition });
        while (queue.Count > 0) {

            path = queue.Dequeue();
            Vector3 node = path[^1];
            if (node == endPosition) return path;

            if (!graph.ContainsKey(node)) continue;

            foreach ( Vector3 adjacent in graph[node])
            {
                List<Vector3> newPath = new List<Vector3>(path);
                newPath.Add(adjacent);
                queue.Enqueue(newPath);
            }
        }
        return path;
    }
}
