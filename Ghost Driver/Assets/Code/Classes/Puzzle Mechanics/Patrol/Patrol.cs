using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class Patrol : MonoBehaviour
{
    enum PatrolState { Patrolling, Turning }

    [Tooltip ("The speed at which the agent moves to each waypoint.")]
    [SerializeField] private float _Speed = 4.0f;
    [Tooltip ("The gameobject containing all of the enemies waypoints.")]
    [SerializeField] GameObject _WaypointHolder = null;
    //[Tooltip ("Should the enemy go back to the start position at the end? \nOr should they reverse back through their route?")]
    //[SerializeField] private bool _ShouldReverse = false;

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

        _WaypointHolder = null;
    }

    private void FixedUpdate ()
    {
        if (_CurrentState == PatrolState.Patrolling)
            Move ();
    }

    private void Move ()
    {
        var currentWayPoint = GetNextWaypoint ();

        if (IsAtNextPoint (currentWayPoint))
        {
            _CurrentIndex++;
            Rotate ();
        }

        MoveToNextPoint (currentWayPoint);
    }

    private void Rotate ()
    {
        var currentWayPoint = GetNextWaypoint ();
        _Transform.up = currentWayPoint.position - _Transform.position;
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

    private bool Approximately (float a, float b, float tolerance)
    {
        return (Mathf.Abs (a - b) < tolerance);
    }

    private bool VectorApproximation (Vector3 a, Vector3 b, float tolerance)
    {
        return (Mathf.Abs(a.x - b.x) < tolerance && Mathf.Abs (a.y - b.y) < tolerance && Mathf.Abs (a.y - b.y) < tolerance);
    }

    IEnumerator RotateTo ()
    {
        _CurrentState = PatrolState.Turning;

        float timer = 0.0f;
        Vector3 nextWaypoint = GetNextWaypoint ().position;
        Quaternion currentRotation = _Transform.rotation;
        Quaternion nextRotation = Quaternion.LookRotation (Vector3.forward, nextWaypoint - _Transform.position);

        while (timer <= _TurnSpeed)
        {
            timer += Time.fixedDeltaTime;

            var rotation = Quaternion.Lerp (currentRotation, nextRotation, timer / _TurnSpeed);
            _Transform.rotation = rotation;

            yield return new WaitForFixedUpdate ();
        }

        _Transform.rotation = nextRotation;

        _CurrentState = PatrolState.Patrolling;
    }
}
