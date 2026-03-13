using System.Collections;
using System.Collections.Generic;
using NesScripts.Controls.PathFind;
using UnityEngine;

public class CivilianBrain : MonoBehaviour
{
    [SerializeField] private CivilianInstance _civilianPrefab;
    public List<CivilianInstance> ActiveCivilians { get; private set; }

    public PathfindingManager PathfindingManager { get; private set; }

    private void Start()
    {
        PathfindingManager = FindFirstObjectByType<PathfindingManager>();
        ActiveCivilians = new List<CivilianInstance>();

        //CreateNewCivilian(default);
        //StartCoroutine(C_ConstantSpawn());
    }


    private IEnumerator C_ConstantSpawn()
    {
        for (int i = 0; i < 50; i++)
        {
            yield return new WaitForSeconds(.01f);
            CreateNewCivilian(default);
        }
    }

    private void Update()
    {
        float delta = Time.deltaTime;
        foreach (CivilianInstance ci in ActiveCivilians)
        {
            ci.I_Update(delta);
        }
    }

    private void LateUpdate()
    {
        float delta = Time.deltaTime;
        foreach (CivilianInstance ci in ActiveCivilians)
        {
            ci.I_LateUpdate(delta);
        }
    }

    public Node GetFreeCivilianSpot()
    {
        int x; // Columns
        int y; // Rows
        int i = 0;
        do
        {
            x = Random.Range(0, PathfindingManager.Nodes.GetLength(1)); // Columns
            y = Random.Range(0, PathfindingManager.Nodes.GetLength(0)); // Rows
            i++;

        } while ((PathfindingManager.Nodes[x, y].gCost == CivilianInstance.OCCUPIED_NODE_GVALUE && i < 5) || !PathfindingManager.Nodes[x, y].walkable);

        return PathfindingManager.Nodes[x, y];
    }

    public void CreateNewCivilian(CivilianFaultType civilianFaultType)
    {
        Node spawnNode = GetFreeCivilianSpot();
        CivilianInstance newCI = Instantiate(_civilianPrefab);

        ActiveCivilians.Add(newCI);
        newCI.Initialize(this, spawnNode, civilianFaultType);
        newCI.name = "Civ " + UnityEngine.Random.Range(-9999, 9999);
    }

    public List<CivilianInstance> GetCiviliansInDirection(Node from, int dx, int dy)
    {
        return PathfindingManager.GetCiviliansInDirection(from, dx, dy);
    }

    public void ClearCivillians()
    {
        foreach(CivilianInstance civillian in ActiveCivilians)
        {
            Destroy(civillian.gameObject);
        }

        ActiveCivilians.Clear();

        foreach (Node n in PathfindingManager.Nodes)
        {
            n.gCost = 0;
        }
    }
    
}


