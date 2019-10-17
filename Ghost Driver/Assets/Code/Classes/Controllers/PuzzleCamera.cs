using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class PuzzleCamera : MonoBehaviour
{
    [SerializeField] private float _DampTime = 0.45f;
    [SerializeField] private Vector3 _Distance = Vector3.zero;
    [SerializeField] private Transform _Target = null;

    private Camera _Camera = null;
    private Transform _Transform = null;
    private Vector3 velocity = Vector3.zero;

    private void Awake ()
    {
        _Camera = GetComponent<Camera> ();
        _Transform = GetComponent<Transform> ();
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
}
