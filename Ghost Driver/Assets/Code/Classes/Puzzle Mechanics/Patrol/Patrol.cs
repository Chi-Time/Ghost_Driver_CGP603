using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class Patrol : MonoBehaviour
{
    [Tooltip ("The speed at which the agent moves to each waypoint.")]
    [SerializeField] private float _Speed = 1.0f;
    [Tooltip ("The gameobject containing all of the enemies waypoints.")]
    [SerializeField] GameObject _WaypointHolder = null;
    [Tooltip ("Should the enemy go back to the start position at the end? \nOr should they reverse back through their route?")]
    [SerializeField] private bool _ShouldReverse = false;

    private int _CurrentIndex = 0;
    private Transform[] _WayPoints = null;
    private Transform _Transform = null;

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
        if (_CurrentIndex >= _WayPoints.Length)
            _CurrentIndex = 0;

        Move ();
    }

    private void Move ()
    {
        var currentWayPoint = _WayPoints[_CurrentIndex];

        if (IsAtNextPoint (currentWayPoint))
            _CurrentIndex++;

        MoveToNextPoint (currentWayPoint);
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
}
