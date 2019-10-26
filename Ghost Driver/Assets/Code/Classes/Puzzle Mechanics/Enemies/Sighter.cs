using System.Collections;
using UnityEngine;

//TODO: Consider ripping out the movement logic along the route to it's own sub-component as well as the sight logic. 
//TODO: Find way to seperate rotation logic to it's own sub-component too if possible.

class Sighter : MonoBehaviour
{
    enum SighterState { Patrolling, Reversing, Turning }

    [Header ("Sight")]
    [Tooltip ("How many tiles ahead the agent see.")]
    [SerializeField] private float _SightDistance = 3.0f;

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

    [Header ("Rotation")]
    [Tooltip ("How long the Agent takes to turn.")]
    [SerializeField] private float _TurnSpeed = 1.0f;

    private int _CurrentIndex = 0;
    private LineDrawer _Line = null;
    private Transform[] _WayPoints = null;
    private Transform _Transform = null;
    private SighterState _CurrentState = SighterState.Patrolling;

    private void Awake ()
    {
        _Line = new LineDrawer ();
        _Transform = GetComponent<Transform> ();
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

    private void Update ()
    {
        _Line.DrawLineInGameView (_Transform.position, _Transform.position + _Transform.up * _SightDistance, Color.red);

        if (Physics.Linecast (_Transform.position, _Transform.position + _Transform.up * _SightDistance, out RaycastHit hit))
        {
            if (hit.collider != null)
            {
                if (hit.collider.CompareTag ("Player"))
                {
                    //TODO: Implement sight game logic.
                    print ("The Sighter see's you");
                }
            }
        }
    }

    private void FixedUpdate ()
    {
        if (_CurrentState == SighterState.Patrolling || _CurrentState == SighterState.Reversing)
            Move ();
    }

    private void Move ()
    {
        var currentWayPoint = GetNextWaypoint ();

        if (IsAtNextPoint (currentWayPoint))
        {
            CheckState ();
            ChangeIndex ();
            StartCoroutine (RotateTo ());
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

    IEnumerator RotateTo ()
    {
        float timer = 0.0f;
        SighterState startState = _CurrentState;
        Vector3 nextWaypoint = GetNextWaypoint ().position;
        Quaternion startRotation = _Transform.rotation;
        Quaternion nextRotation = Quaternion.LookRotation (Vector3.forward, nextWaypoint - _Transform.position);

        _CurrentState = SighterState.Turning;

        while (timer <= _TurnSpeed)
        {
            timer += Time.fixedDeltaTime;

            var rotation = Quaternion.Lerp (startRotation, nextRotation, timer / _TurnSpeed);
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
                _CurrentState = SighterState.Reversing;
            else
                _CurrentState = SighterState.Patrolling;
        }

        if (_CurrentIndex <= 0)
            _CurrentState = SighterState.Patrolling;
    }

    private void ChangeIndex ()
    {
        if (RouteHasEnded ())
        {
            if (_CurrentState == SighterState.Patrolling)
                _CurrentIndex = 0;
            else if (_CurrentState == SighterState.Reversing)
                _CurrentIndex--;
        }
        else
        {
            if (_CurrentState == SighterState.Patrolling)
                _CurrentIndex++;
            else if (_CurrentState == SighterState.Reversing)
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
