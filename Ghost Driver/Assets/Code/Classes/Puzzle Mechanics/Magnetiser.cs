using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;  

class Magnetiser : MonoBehaviour
{
    enum MagnetiserState { Patrolling, Reversing }

    [Header ("Movement")]
    [Tooltip ("The speed at which the agent moves to each waypoint.")]
    [SerializeField] private float _Speed = 1.0f;
    [Tooltip ("The gameobject containing all of the enemies waypoints.")]
    [SerializeField] GameObject _WaypointHolder = null;
    [Tooltip ("Should the enemy go back to the start position at the end? " +
        "\nOr should they reverse back through their route?")]
    [SerializeField] private bool _ShouldReverse = false;
    [Tooltip ("Should an editor path be displayed for the AI.")]
    [SerializeField] private bool _ShouldDisplayPath = false;
    [Tooltip ("The color to display the path lines as in the editor.")]
    [SerializeField] private Color _PathColor = Color.white;

    private int _CurrentIndex = 0;
    private Transform _Transform = null;
    private Transform[] _WayPoints = null;
    private Vector3 _SpawnPosition = Vector3.zero;
    private Quaternion _SpawnRotation = Quaternion.identity;
    private MagnetiserState _CurrentState = MagnetiserState.Patrolling;

    private void Awake ()
    {
        _Transform = GetComponent<Transform> ();

        _SpawnPosition = _Transform.position;
        _SpawnRotation = _Transform.rotation;
    }

    private void Start ()
    {
        GetWaypointsFromHolder ();
    }

    private void GetWaypointsFromHolder ()
    {
        _WayPoints = new Transform[_WaypointHolder.transform.childCount];

        for (int i = 0; i < _WaypointHolder.transform.childCount; i++)
        {
            _WayPoints[i] = _WaypointHolder.transform.GetChild (i);
        }
    }

    private void FixedUpdate ()
    {
        if (_CurrentState == MagnetiserState.Patrolling || _CurrentState == MagnetiserState.Reversing)
            Move ();
    }

    private void Move ()
    {
        var currentWayPoint = GetNextWaypoint ();

        if (IsAtNextPoint (currentWayPoint))
        {
            CheckState ();
            ChangeIndex ();
        }

        MoveToNextPoint (currentWayPoint);
    }

    private Transform GetNextWaypoint ()
    {
        if (_CurrentIndex >= _WayPoints.Length)
            _CurrentIndex = 0;

        return _WayPoints[_CurrentIndex];
    }

    private bool IsAtNextPoint (Transform waypoint)
    {
        if (_Transform.position == waypoint.position)
        {
            return true;
        }

        return false;
    }

    private void MoveToNextPoint (Transform waypoint)
    {
        _Transform.position = Vector3.MoveTowards (_Transform.position, waypoint.position, _Speed * Time.deltaTime);
    }

    private void CheckState ()
    {
        if (RouteHasEnded ())
        {
            if (_ShouldReverse)
                _CurrentState = MagnetiserState.Reversing;
            else
                _CurrentState = MagnetiserState.Patrolling;
        }

        if (_CurrentIndex <= 0)
            _CurrentState = MagnetiserState.Patrolling;
    }

    private void ChangeIndex ()
    {
        if (RouteHasEnded ())
        {
            if (_CurrentState == MagnetiserState.Patrolling)
                _CurrentIndex = 0;
            else if (_CurrentState == MagnetiserState.Reversing)
                _CurrentIndex--;
        }
        else
        {
            if (_CurrentState == MagnetiserState.Patrolling)
                _CurrentIndex++;
            else if (_CurrentState == MagnetiserState.Reversing)
                _CurrentIndex--;
        }
    }

    private bool RouteHasEnded ()
    {
        if (_CurrentIndex >= _WayPoints.Length - 1)
        {
            return true;
        }

        return false;
    }

    private void OnEnable ()
    {
        PuzzleSignals.OnPuzzleReset += OnPuzzleReset;
    }

    private void OnPuzzleReset ()
    {
        _CurrentIndex = 0;
        _Transform.position = _SpawnPosition;
        _Transform.rotation = _SpawnRotation;
        _CurrentState = MagnetiserState.Patrolling;
    }

    private void OnDisable ()
    {
        PuzzleSignals.OnPuzzleReset -= OnPuzzleReset;
    }

    private void OnDrawGizmos ()
    {
        if (false == _ShouldDisplayPath)
            return;

        var from = Vector3.zero;
        var to = Vector3.zero;

        if (_WaypointHolder != null)
        {
            for (int i = 0; i < _WaypointHolder.transform.childCount; i++)
            {
                if (i + 1 >= _WaypointHolder.transform.childCount)
                    return;

                if (_WaypointHolder.transform.GetChild (i).gameObject.activeSelf == false)
                    continue;

                from = _WaypointHolder.transform.GetChild (i).position;
                to = _WaypointHolder.transform.GetChild (i + 1).position;

                Gizmos.color = _PathColor;
                Gizmos.DrawLine (from, to);
            }
        }
    }
}
