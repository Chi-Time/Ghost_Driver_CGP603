using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class Patrol : MonoBehaviour
{
    enum PatrolState { Patrolling, Reversing, Turning }

    [Tooltip ("The speed at which the agent moves to each waypoint.")]
    [SerializeField] private float _Speed = 4.0f;
    [Tooltip ("The gameobject containing all of the enemies waypoints.")]
    [SerializeField] GameObject _WaypointHolder = null;
    [Tooltip ("Should the enemy go back to the start position at the end? " +
        "\nOr should they reverse back through their route?")]
    [SerializeField] private bool _ShouldReverse = false;

    [Header ("Rotation")]
    [Tooltip ("How long the Agent takes to turn.")]
    [SerializeField] private float _TurnSpeed = 0.25f;

    private int _CurrentIndex = 0;
    private Transform[] _WayPoints = null;
    private Transform _Transform = null;
    private PatrolState _CurrentState = PatrolState.Patrolling;

    private void Awake ()
    {
        _Transform = GetComponent<Transform> ();
    }

    private void Start ()
    {
        _WayPoints = new Transform[_WaypointHolder.transform.childCount];

        for (int i = 0; i < _WaypointHolder.transform.childCount; i++)
        {
            _WayPoints[i] = _WaypointHolder.transform.GetChild (i);
        }
    }

    private void FixedUpdate ()
    {
        if (_CurrentState == PatrolState.Patrolling || _CurrentState == PatrolState.Reversing)
            Move ();
    }

    private void Move ()
    {
        var currentWaypoint = GetNextWaypoint ();

        if (HasReachedNextPoint (currentWaypoint))
        {
            CheckState ();
            ChangeIndex ();
            Rotate ();
        }

        MoveToNextPoint (currentWaypoint);
    }

    private Transform GetNextWaypoint ()
    {
        if (_CurrentIndex >= _WayPoints.Length)
            _CurrentIndex = 0;

        return _WayPoints[_CurrentIndex];
    }

    private void Rotate ()
    {
        var currentWayPoint = GetNextWaypoint ();
        _Transform.up = currentWayPoint.position - _Transform.position;
    }

    private bool HasReachedNextPoint (Transform waypoint)
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

    private IEnumerator RotateTo ()
    {
        float timer = 0.0f;
        PatrolState startState = _CurrentState;
        Vector3 nextWaypoint = GetNextWaypoint ().position;
        Quaternion currentRotation = _Transform.rotation;
        Quaternion nextRotation = Quaternion.LookRotation (Vector3.forward, nextWaypoint - _Transform.position);

        _CurrentState = PatrolState.Turning;

        while (timer <= _TurnSpeed)
        {
            timer += Time.fixedDeltaTime;

            var rotation = Quaternion.Lerp (currentRotation, nextRotation, timer / _TurnSpeed);
            _Transform.rotation = rotation;

            yield return new WaitForFixedUpdate ();
        }

        _Transform.rotation = nextRotation;
        _CurrentState = startState;
    }

    private void CheckState ()
    {
        if (RouteHasEnded ())
        {
            if (_ShouldReverse)
                _CurrentState = PatrolState.Reversing;
            else
                _CurrentState = PatrolState.Patrolling;
        }

        if (_CurrentIndex <= 0)
            _CurrentState = PatrolState.Patrolling;
    }

    private void ChangeIndex ()
    {
        if (RouteHasEnded ())
        {
            if (_CurrentState == PatrolState.Patrolling)
                _CurrentIndex = 0;
            else if (_CurrentState == PatrolState.Reversing)
                _CurrentIndex--;
        }
        else
        {
            if (_CurrentState == PatrolState.Patrolling)
                _CurrentIndex++;
            else if (_CurrentState == PatrolState.Reversing)
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
}
