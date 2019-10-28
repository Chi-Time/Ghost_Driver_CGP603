using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class PuzzleCamera : MonoBehaviour
{
    [SerializeField] private float _DampTime = 0.45f;
    [SerializeField] private float _ZoomTime = 1.0f;
    [SerializeField] private Vector3 _ZoomDistance = Vector3.zero;
    [SerializeField] private Vector3 _NormalDistance = Vector3.zero;
    [SerializeField] private Transform _Target = null;

    private Camera _Camera = null;
    private Transform _Transform = null;
    private bool _IsZoomedOut = false;
    private Vector3 velocity = Vector3.zero;
    private Vector3 _Distance = Vector3.zero;

    private void Awake ()
    {
        _Distance = _NormalDistance;
        _Camera = GetComponent<Camera> ();
        _Transform = GetComponent<Transform> ();
    }

    private void Update ()
    {


        if (Input.GetKeyUp (KeyCode.Tab) || Input.GetKeyUp (KeyCode.Backspace))
        {
            _IsZoomedOut = !_IsZoomedOut;

            if (_IsZoomedOut)
                StartCoroutine (ZoomTo (_ZoomDistance));
            else
                StartCoroutine (ZoomTo (_NormalDistance));
        }
    }

    private IEnumerator ZoomTo (Vector3 toDistance)
    {
        float timer = 0.0f;
        Vector3 fromDistance = _Distance;

        while (timer <= _ZoomTime)
        {
            timer += Time.fixedDeltaTime;

            var distance = Vector3.Lerp (fromDistance, toDistance, timer / _ZoomTime);
            _Distance = distance;

            yield return new WaitForFixedUpdate ();
        }

        _Distance = toDistance;
    }

    private void FixedUpdate ()
    {
        if (_Target)
        {
            Vector3 point = _Camera.WorldToViewportPoint (_Target.position);
            Vector3 delta = _Target.position - _Camera.ViewportToWorldPoint (_Distance);
            Vector3 destination = _Transform.position + delta;

            _Transform.position = Vector3.SmoothDamp (_Transform.position, destination, ref velocity, _DampTime);
        }
    }

    private void OnEnable ()
    {
        PuzzleSignals.OnPuzzleReset += OnPuzzleReset;
    }

    private void OnPuzzleReset ()
    {
        StopAllCoroutines ();
        _Distance = _NormalDistance;
    }

    private void OnDisable ()
    {
        PuzzleSignals.OnPuzzleReset -= OnPuzzleReset;
    }
}
