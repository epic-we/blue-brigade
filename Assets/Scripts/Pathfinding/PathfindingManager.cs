using UnityEngine;
using NesScripts.Controls.PathFind;
using Grid = NesScripts.Controls.PathFind.Grid;
using System.Collections.Generic;

public class PathfindingManager : MonoBehaviour
{
    [SerializeField] private bool _showGrid;
    [SerializeField] private LayerMask _wallLayer;
    [SerializeField] private Vector2Int _gridSize;
    [SerializeField] private Vector2 _tileSize;

    private Vector2 _worldCenterGridOffset;

    private Grid _grid;

    public Node[,] Nodes => _grid.nodes;

    private void Awake()
    {
        _grid = GenerateGrid(_gridSize.x, _gridSize.y);
        _worldCenterGridOffset = new Vector2(_gridSize.x / 2f * _tileSize.x - _tileSize.x / 2f, _gridSize.y / 2f * _tileSize.y - _tileSize.y / 2f);
        foreach (Node n in _grid.nodes)
        {
            n.WorldPosition = ConvertGridToWorldPosition(n);
            n.walkable = Physics2D.OverlapBox(n.WorldPosition, _tileSize / 2f, 0, _wallLayer) == null;
        }
    }


    private Grid GenerateGrid(int width, int height)
    {
        bool[,] tilesmap = new bool[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                tilesmap[x, y] = true;
            }
        }

        return new Grid(tilesmap);
    }
    public Vector2 ConvertGridToWorldPosition(int x, int y)
    {
        return Nodes[x, y].WorldPosition;
    }

    private Vector2 ConvertGridToWorldPosition(Node n)
    {
        return new Vector2(n.gridX, n.gridY) * _tileSize - _worldCenterGridOffset;
    }
    public Node ConvertWorldPositionToGrid(Vector2 worldPosition)
    {
        Node n = null;
        Vector2 gridPos = (worldPosition + _worldCenterGridOffset) / _tileSize;
        int x = Mathf.RoundToInt(gridPos.x);
        int y = Mathf.RoundToInt(gridPos.y);
        if (x < _grid.nodes.GetLength(0) && y < _grid.nodes.GetLength(1))
            n = _grid.nodes[x, y];

        return n;
    }

    public List<Point> FindPath(Node from, Node to)
    {
        return Pathfinding.FindPath(_grid, new Point(from.gridX, from.gridY), new Point(to.gridX, to.gridY));
    }

    public List<CivilianInstance> GetCiviliansInDirection(Node from, int dx, int dy)
    {
        if (from.gridY + dy >= _gridSize.y || from.gridX + dx >= _gridSize.x ||
            from.gridY + dy < 0 || from.gridX + dx < 0)
            return null;
        return _grid.nodes[from.gridX + dx, from.gridY + dy].civs;
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying || !_showGrid) return;

        foreach (Node n in _grid.nodes)
        {
            if (!n.walkable)
                Gizmos.color = Color.red;
            else if (n.price == CivilianInstance.WALKING_NODE_GVALUE)
                Gizmos.color = Color.green;
            else if (n.price == CivilianInstance.OCCUPIED_NODE_GVALUE)
                Gizmos.color = Color.blue;
            else
                Gizmos.color = Color.gray;

            Gizmos.DrawWireCube(ConvertGridToWorldPosition(n), _tileSize * 0.9f);
        }
    }

}


// From: https://github.com/RonenNess/UnityUtils/tree/master/Controls/PathFinding/2dTileBasedPathFinding

// First, copy the folder 'PathFinding' to anywhere you want your asset scripts folder. Once you have it in your project, use the pathfinding like this:

// // create the tiles map
// float[,] tilesmap = new float[width, height];
// // set values here....
// // every float in the array represent the cost of passing the tile at that position.
// // use 0.0f for blocking tiles.

// // create a grid
// PathFind.Grid grid = new PathFind.Grid(tilesmap);

// // create source and target points
// PathFind.Point _from = new PathFind.Point(1, 1);
// PathFind.Point _to = new PathFind.Point(10, 10);

// // get path
// // path will either be a list of Points (x, y), or an empty list if no path is found.
// List<PathFind.Point> path = PathFind.Pathfinding.FindPath(grid, _from, _to);

// // for Manhattan distance
// List<PathFind.Point> path = PathFind.Pathfinding.FindPath(grid, _from, _to, Pathfinding.DistanceType.Manhattan);
// If you don't care about price of tiles (eg tiles can only be walkable or blocking), you can also pass a 2d array of booleans when creating the grid:

// // create the tiles map
// bool[,] tilesmap = new bool[width, height];
// // set values here....
// // true = walkable, false = blocking

// // create a grid
// PathFind.Grid grid = new PathFind.Grid(tilesmap);

// // rest is the same..
// After creating the grid with a tilemap, you can update the grid using:

// // create a grid
// PathFind.Grid grid = new PathFind.Grid(tilesmap);

// // change the tilemap here

// // update later
// grid.UpdateGrid (tilesmap);