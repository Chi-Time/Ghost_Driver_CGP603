using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent (typeof (AudioPlayer))]
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
    [Tooltip ("Should an editor path be displayed for the AI.")]
    [SerializeField] private bool _ShouldDisplayPath = false;
    [Tooltip ("The color to display the path lines as in the editor.")]
    [SerializeField] private Color _PathColor = Color.white;

    [Header ("Rotation")]
    [Tooltip ("How long the Agent takes to turn.")]
    [SerializeField] private float _TurnSpeed = 0.25f;

    private int _CurrentIndex = 0;
    private Transform _Transform = null;
    private Transform[] _WayPoints = null;
    private Vector3 _SpawnPosition = Vector3.zero;
    private Quaternion _SpawnRotation = Quaternion.identity;
    private PatrolState _CurrentState = PatrolState.Patrolling;
    private AudioPlayer _AudioPlayer = null;

    private void Awake ()
    {
        _Transform = GetComponent<Transform> ();

        _SpawnPosition = _Transform.position;
        _SpawnRotation = _Transform.rotation;
        _AudioPlayer = GetComponent<AudioPlayer> ();
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

    private void OnTriggerEnter (Collider other)
    {
        if (other.CompareTag ("Player"))
        {
            _AudioPlayer.Play ();
        }
    }

    private void Move ()
    {
        var currentWaypoint = GetNextWaypoint ();

        if (HasReachedNextPoint (currentWaypoint))
        {
            CheckState ();
            ChangeIndex ();
            StartCoroutine (RotateTo ());
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

    private void OnEnable ()
    {
        PuzzleSignals.OnPuzzleReset += OnPuzzleReset;
    }

    private void OnPuzzleReset ()
    {
        _CurrentIndex = 0;
        _Transform.position = _SpawnPosition;
        _Transform.rotation = _SpawnRotation;
        _CurrentState = PatrolState.Patrolling;
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
                if (_WaypointHolder.transform.GetChild (i).gameObject.activeSelf == false)
                    continue;

                from = _WaypointHolder.transform.GetChild (i).position;

                if (_ShouldReverse)
                {
                    if (i + 1 >= _WaypointHolder.transform.childCount)
                        return;
                    else
                        to = _WaypointHolder.transform.GetChild (i + 1).position;
                }
                else
                {
                    if (i + 1 >= _WaypointHolder.transform.childCount)
                        to = _WaypointHolder.transform.GetChild (0).position;
                    else
                        to = _WaypointHolder.transform.GetChild (i + 1).position;
                }

                Gizmos.color = _PathColor;
                Gizmos.DrawLine (from, to);
            }
        }
    }
}
