using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class Sentinel : MonoBehaviour
{
    enum RotationType { Eighths, Quarters, Half }

    [Header ("Sight")]
    [Tooltip ("How many tiles ahead the agent see.")]
    [SerializeField] private float _SightDistance = 3.0f;

    [Header ("Rotation")]
    [Tooltip ("How long before the Agent turns to a new direction")]
    [SerializeField] private float _TurnTimer = 1.0f;
    [Tooltip ("How long the Agent takes to turn.")]
    [SerializeField] private float _TurnSpeed = 3.0f;
    [Tooltip ("Should the agent rotate in a clockwise fashion?")]
    [SerializeField] private bool _ClockwiseTurning = true;
    [Tooltip ("Should the agent turn in 45, 90 or 180 degree angles?")]
    [SerializeField] private RotationType _CurrentRotationType = RotationType.Eighths;

    private LineDrawer _Line = null;
    private Transform _Transform = null;

    private void Awake ()
    {
        _Line = new LineDrawer ();
        _Transform = GetComponent<Transform> ();
    }

    private void Start ()
    {
        StartCoroutine (RotateTo ());
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
                    print ("I see you");
                }
            }
        }
    }

    IEnumerator RotateTo ()
    {
        yield return new WaitForSeconds (_TurnTimer);

        float timer = 0.0f;
        Vector3 nextRotation = GetNextRotation ();
        Vector3 currentRotation = _Transform.rotation.eulerAngles;

        while (timer <= _TurnSpeed)
        {
            timer += Time.fixedDeltaTime;

            var rotation = Vector3.Lerp (currentRotation, nextRotation, timer / _TurnSpeed);
            _Transform.rotation = Quaternion.Euler (rotation);

            yield return new WaitForFixedUpdate ();
        }

        _Transform.rotation = Quaternion.Euler (nextRotation);

        StartCoroutine (RotateTo ());
    }

    private Vector3 GetNextRotation ()
    {
        float offset = GetOffset ();
        float currentAngle = _Transform.rotation.eulerAngles.z;
        var rotation = new Vector3 (0.0f, 0.0f, currentAngle - offset);

        return rotation;
    }

    private float GetOffset ()
    {
        float offset = 0.0f;

        switch (_CurrentRotationType)
        {
            case RotationType.Eighths:
                offset = 45f;
                break;
            case RotationType.Quarters:
                offset = 90f;
                break;
            case RotationType.Half:
                offset = 180f;
                break;
        }

        if (_ClockwiseTurning)
            return offset;
        else
            return -offset;
    }
}
