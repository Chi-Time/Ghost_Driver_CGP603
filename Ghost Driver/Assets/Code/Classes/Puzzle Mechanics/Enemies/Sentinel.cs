using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent (typeof (AudioPlayer))]
class Sentinel : MonoBehaviour
{
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
    private Quaternion _SpawnRotation = Quaternion.identity;
    private AudioPlayer _AudioPlayer = null;

    private void Awake ()
    {
        //_Line = new LineDrawer ();
        _Transform = GetComponent<Transform> ();
        _AudioPlayer = GetComponent<AudioPlayer> ();

        _SpawnRotation = _Transform.rotation;
    }

    private void Start ()
    {
        StartCoroutine (RotateTo ());
    }

    private void Update ()
    {
        // Draw an in-game sight line so that the player can tell the enemie's sight length.
        //_Line.DrawLineInGameView(_Transform.position, _Transform.position + _Transform.forward * _SightDistance, Color.red);

        See();
    }

    private void See ()
    {
        if (Physics.Linecast(_Transform.position, _Transform.position + _Transform.forward * _SightDistance, out RaycastHit hit))
        {
            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Player"))
                {
                    PuzzleSignals.FailPuzzle ();
                    _AudioPlayer.Play ();
                }
            }
        }
    }

    IEnumerator RotateTo ()
    {
        yield return new WaitForSeconds (_TurnTimer);

        float timer = 0.0f;
        Vector3 nextRotation = GetNextRotation ();
        Vector3 currentRotation = _Transform.localRotation.eulerAngles;

        while (timer <= _TurnSpeed)
        {
            timer += Time.fixedDeltaTime;

            var rotation = Vector3.Lerp (currentRotation, nextRotation, timer / _TurnSpeed);
            _Transform.localRotation = Quaternion.Euler (rotation);

            yield return new WaitForFixedUpdate ();
        }

        _Transform.localRotation = Quaternion.Euler (nextRotation);

        StartCoroutine (RotateTo ());
    }

    private Vector3 GetNextRotation ()
    {
        float offset = GetOffset ();
        float currentAngle = _Transform.localRotation.eulerAngles.y;
        var rotation = new Vector3 (0.0f, currentAngle - offset, 0.0f);

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

    private void OnEnable ()
    {
        PuzzleSignals.OnPuzzleReset += OnPuzzleReset;
    }

    private void OnPuzzleReset ()
    {
        StopAllCoroutines ();
        _Transform.rotation = _SpawnRotation;
        StartCoroutine (RotateTo ());
    }

    private void OnDisable ()
    {
        PuzzleSignals.OnPuzzleReset -= OnPuzzleReset;
    }
}
