using System.Collections;
using UnityEngine;

class Sighter : MonoBehaviour
{
    enum SighterState { Patrolling, Watching }

    [Header("Sight")]
    [Tooltip("How many tiles ahead the agent see.")]
    [SerializeField] private float _SightDistance = 3.0f;

    [Header("Movement")]
    [Tooltip("The speed at which the agent moves to each waypoint.")]
    [SerializeField] private float _Speed = 1.0f;
    [Tooltip("The gameobject containing all of the enemies waypoints.")]
    [SerializeField] GameObject _WaypointHolder = null;
    //[Tooltip ("Should the enemy go back to the start position at the end? \nOr should they reverse back through their route?")]
    //[SerializeField] private bool _ShouldReverse = false;

    [Header("Rotation")]
    [Tooltip("How long before the Agent turns to a new direction")]
    [SerializeField] private float _TurnTimer = 1.0f;
    [Tooltip("How long the Agent takes to turn.")]
    [SerializeField] private float _TurnSpeed = 3.0f;
    [Tooltip("Should the agent rotate in a clockwise fashion?")]
    [SerializeField] private bool _ClockwiseTurning = true;
    [Tooltip("Should the agent turn in 45, 90 or 180 degree angles?")]
    [SerializeField] private RotationType _CurrentRotationType = RotationType.Eighths;

    private int _CurrentIndex = 0;
    private LineDrawer _Line = null;
    private Transform[] _WayPoints = null;
    private Transform _Transform = null;
    private SighterState _CurrentState = SighterState.Patrolling;

    private void Awake()
    {
        _Line = new LineDrawer();
        _Transform = GetComponent<Transform>();
    }

    private void Start()
    {
        GetWaypointsFromHolder();
    }

    private void GetWaypointsFromHolder()
    {
        _WayPoints = new Transform[_WaypointHolder.transform.childCount];

        for (int i = 0; i < _WaypointHolder.transform.childCount; i++)
        {
            _WayPoints[i] = _WaypointHolder.transform.GetChild(i);
        }

        // Assign the reference to null to save memory.
        _WaypointHolder = null;
    }

    private void Update()
    {
        _Line.DrawLineInGameView(_Transform.position, _Transform.position + _Transform.up * _SightDistance, Color.red);

        if (Physics.Linecast(_Transform.position, _Transform.position + _Transform.up * _SightDistance, out RaycastHit hit))
        {
            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Player"))
                {
                    //TODO: Implement sight game logic.
                    print("The Sighter see's you");
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (_CurrentState == SighterState.Patrolling)
            Move();
    }

    private void Move()
    {
        var currentWayPoint = GetNextWaypoint();

        if (IsAtNextPoint(currentWayPoint))
        {
            _CurrentIndex++;
            StartCoroutine(RotateTo());
        }

        MoveToNextPoint(currentWayPoint);
    }

    private void Rotate()
    {
        var currentWayPoint = GetNextWaypoint();
        _Transform.up = currentWayPoint.position - _Transform.position;
    }

    private Transform GetNextWaypoint()
    {
        if (_CurrentIndex >= _WayPoints.Length)
            _CurrentIndex = 0;

        return _WayPoints[_CurrentIndex];
    }

    private bool IsAtNextPoint(Transform waypoint)
    {
        if (_Transform.position == waypoint.position)
        {
            return true;
        }

        return false;
    }

    private void MoveToNextPoint(Transform waypoint)
    {
        _Transform.position = Vector3.MoveTowards(_Transform.position, waypoint.position, _Speed * Time.deltaTime);
    }

    IEnumerator RotateTo()
    {
        _CurrentState = SighterState.Watching;

        float timer = 0.0f;
        Vector3 nextWaypoint = GetNextWaypoint().position;
        Quaternion currentRotation = _Transform.rotation;
        Quaternion nextRotation = Quaternion.LookRotation(Vector3.forward, nextWaypoint - _Transform.position);

        while (timer <= _TurnSpeed)
        {
            timer += Time.fixedDeltaTime;

            var rotation = Quaternion.Lerp(currentRotation, nextRotation, timer / _TurnSpeed);
            _Transform.rotation = rotation;

            yield return new WaitForFixedUpdate();
        }

        _Transform.rotation = nextRotation;

        _CurrentState = SighterState.Patrolling;
    }
}
