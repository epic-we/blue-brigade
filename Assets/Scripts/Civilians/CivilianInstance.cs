using System;
using System.Collections.Generic;
using NesScripts.Controls.PathFind;
using UnityEngine;

public class CivilianInstance : MonoBehaviour
{
    public const int OCCUPIED_NODE_GVALUE = 3;
    public const int WALKING_NODE_GVALUE = 2;
    private const string IDLE_TRIGGER = "idle";
    private const string WALK_TRIGGER = "walk";

    [Header("Mechanic")]
    [SerializeField] private bool _debug;
    [SerializeField] private CivilianState _currentState;
    [SerializeField] private CivilianFault _fault;
    [Header("Visuals")]
    [SerializeField] private Animator _anim;
    [SerializeField] private SpriteRenderer _maiSr;
    [SerializeField] private SpriteRenderer[] _extraSpriteRenderers;

    private CivilianBrain _brain;

    // Node handling
    private Node _curNode;
    private List<Point> _curPath;
    private int _curPathPointI;
    private NodeTravelingInfo _curTravelInfo;
    private Node _targetFinalNode;

    private float _timeInCurrentState;

    [SerializeField] private TalkingBubble _talkingBubble;

    private void Start()
    {
        if (_debug)
        {
            FindFirstObjectByType<CivilianBrain>().ActiveCivilians.Add(this);
        }
        _fault.OnCensored += OnCensored;
    }

    public void Initialize(CivilianBrain brain, Node startNode, CivilianFaultType faultType)
    {
        _currentState = CivilianState.Idle;
        _brain = brain;

        transform.position = startNode.WorldPosition;


        _curNode = startNode; // Set Default

        _fault.Initialize(faultType);
        ChangeNode(_curNode, _fault.FaultType != CivilianFaultType.Group);

    }

    private void JoinLinkedActivity(CivilianInstance newCivilian)
    {
        _fault._linkedCivilians.Add(newCivilian);
    }

    private void DisbandLinkedActivity(bool censored, bool main)
    {
        if (main)
        {
            foreach (CivilianInstance civ in _fault._linkedCivilians)
            {
                if (civ == this)
                {
                    continue;
                }
                civ.DisbandLinkedActivity(censored, false);
            }
        }

        _fault._linkedCivilians.Clear();

        if (censored)
        {
            // Do stuff
        }
    }

    private void ChangeNode(Node newNode, bool listenToNearbyTiles)
    {
        if (_curNode.price == WALKING_NODE_GVALUE)
            _curNode.price = 0;
        _curNode.civs.Remove(this);

        if (newNode.price != OCCUPIED_NODE_GVALUE)
            newNode.price = WALKING_NODE_GVALUE;
        newNode.civs.Add(this);

        if (listenToNearbyTiles)
        {
            CivilianInstance foundCiv = SearchForNearbyActivity(newNode);
            if (foundCiv != null)
            {
                ChangeState(CivilianState.Group);
                //Debug.Log(foundCiv.name);
            }
        }

        _curNode = newNode;
    }
    private CivilianInstance SearchForNearbyActivity(Node curNode)
    {
        if (curNode.gCost == OCCUPIED_NODE_GVALUE) return null;

        List<CivilianInstance> foundCivilians;
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue; // Skip current tile

                foundCivilians = _brain.GetCiviliansInDirection(curNode, dx, dy);
                if (foundCivilians != null)
                {
                    foreach (CivilianInstance civ in foundCivilians)
                    {
                        if (civ._fault.FaultType == CivilianFaultType.Group || civ._fault.FaultType == CivilianFaultType.Talking &&
                        civ._currentState == CivilianState.Group &&
                        foundCivilians.Count < 5 && !civ == this)
                        {
                            civ.JoinLinkedActivity(this);
                            foreach (var l in civ._fault._linkedCivilians)
                            {
                                _fault._linkedCivilians.Add(l);
                            }
                            return civ;
                        }
                    }
                    foundCivilians = null;
                }
            }
        }

        return null;
    }

    public void I_Update(float delta)
    {
        _timeInCurrentState += delta;

        StateMachine(delta);
    }
    public void I_LateUpdate(float delta)
    {
        UpdateVisuals();
    }

    private void SetNewFinalNode(Node nodeToMoveTo, bool running = false)
    {
        if (_targetFinalNode != default)
        {
            _targetFinalNode.price = 0;
        }
        nodeToMoveTo.price = OCCUPIED_NODE_GVALUE;

        _targetFinalNode = nodeToMoveTo;
        _curPath = _brain.PathfindingManager.FindPath(_curNode, _targetFinalNode);
        _curPathPointI = 0;

        TravelToNextNode();
        ChangeState(running ? CivilianState.Traveling_Fast : CivilianState.Traveling);
    }

    private void TravelToNextNode()
    {
        int count = _curPath.Count;
        if (_targetFinalNode == _curTravelInfo.TargetNode || _curPathPointI + 1 == count || count == 0) // Reached Destination
        {
            StopMoving();
            return;
        }

        _curPathPointI++;
        Node nextTravelNode = _brain.PathfindingManager.Nodes[_curPath[_curPathPointI].x, _curPath[_curPathPointI].y];
        Vector2 dir = new Vector2(nextTravelNode.WorldPosition.x - transform.position.x, nextTravelNode.WorldPosition.y - transform.position.y).normalized;

        _curTravelInfo = new NodeTravelingInfo(dir, nextTravelNode);
        ChangeNode(_curTravelInfo.TargetNode, true);

        if (dir.x > 0)
            _anim.transform.right = Vector2.right;
        else
            _anim.transform.right = Vector2.left;
    }

    private void UpdateVisuals()
    {
        // Update Y sorting
        float newOrderZ = transform.position.y * .001f;
        Vector3 newPos = _maiSr.transform.position;
        newPos.z = newOrderZ;
        _maiSr.transform.position = newPos;
        foreach (SpriteRenderer sr in _extraSpriteRenderers)
        {
            newPos = sr.transform.position;
            newPos.z = newOrderZ + 0.0001f;
            sr.transform.position = newPos;
        }
    }

    private void StopMoving()
    {
        //Debug.Log("Stop moving");
        if ((_fault.FaultType == CivilianFaultType.Talking || _fault.FaultType == CivilianFaultType.Group) && !_fault.censored)
        {
            ChangeState(CivilianState.Group);
        }
        else if(UnityEngine.Random.Range(0, 100) < 10)
        {
            ChangeState(CivilianState.Group);
        }
        else
        {
            ChangeState(CivilianState.Idle);
        }
    }

    private void ChangeState(CivilianState newState)
    {
        if (newState == _currentState) return;
        _currentState = newState;
        _timeInCurrentState = 0;
        OnStateChanged?.Invoke(newState);

        switch (newState)
        {
            case CivilianState.Idle:
                _anim.speed = 1;
                _anim.SetTrigger(IDLE_TRIGGER);
                break;
            case CivilianState.Traveling:
                _anim.speed = 1;
                _anim.SetTrigger(WALK_TRIGGER);
                break;
            case CivilianState.Traveling_Fast:
                _anim.speed = 2;
                _anim.SetTrigger(WALK_TRIGGER);
                break;
            case CivilianState.Group:
                _anim.speed = 1;
                _anim.SetTrigger(IDLE_TRIGGER);
                _talkingBubble.StartTalking();
                break;
        }
    }

    private void StateMachine(float delta)
    {
        switch (_currentState)
        {
            case CivilianState.Idle:
                if (_timeInCurrentState > 2)
                {
                    Node newNode = _brain.GetFreeCivilianSpot();
                    SetNewFinalNode(newNode);
                }
                break;
            case CivilianState.Traveling:
                UpdateTravel(delta);
                break;
            case CivilianState.Traveling_Fast:
                UpdateTravel(delta * 4);
                break;
            case CivilianState.Group:
                break;
        }
    }

    private void UpdateTravel(float delta)
    {
        transform.position = transform.position + (Vector3)_curTravelInfo.Direction * delta;

        if (Vector2.Distance(transform.position, _curNode.WorldPosition) < 0.3f)
        {
            TravelToNextNode();
        }
    }

    private void OnCensored(bool correctlyCensored)
    {
        //Debug.Log("Censored on instance civillian");
        Node newNode;
        if (correctlyCensored)
        {
            newNode = _brain.GetFreeCivilianSpot();
        }
        else
        {
            newNode = _brain.GetFreeCivilianSpot();
        }

        foreach (var civ in _fault._linkedCivilians)
        {
            //civ.ProxyCensored(correctlyCensored);
        }

        _anim.SetTrigger("censored");

        DisbandLinkedActivity(true, true);

        SetNewFinalNode(newNode, true);
    }

    private void ProxyCensored(bool correctlyCensored)
    {
        Node newNode = _brain.GetFreeCivilianSpot();
        SetNewFinalNode(newNode, true);
    }

    public enum CivilianState
    {
        Idle,
        Traveling,
        Group,
        Traveling_Fast,
    }

    private struct NodeTravelingInfo
    {
        public Vector2 Direction;
        public Node TargetNode;

        public NodeTravelingInfo(Vector2 dir, Node n)
        {
            Direction = dir;
            TargetNode = n;
        }
    }

    public event Action<CivilianState> OnStateChanged;
}
