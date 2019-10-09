using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Collider), typeof (Rigidbody))]
public class PuzzlePlayerController : MonoBehaviour
{
    [Tooltip ("The speed at which the player moves.")]
    [SerializeField] private float _MovementSpeed = 5.0f;

    private bool _CanMove = true;
    private Vector3 _Direction = Vector3.zero;
    private Vector3 _SpawnPosition = Vector3.zero;
    private Rigidbody _Rigidbody = null;

    protected void Awake ()
    {
        AssignReferences ();
        SetupRigidbody ();
    }

    private void AssignReferences ()
    {
        _Rigidbody = GetComponent<Rigidbody> ();
        GetComponent<Collider> ().isTrigger = true;

        _SpawnPosition = _Rigidbody.position;
    }

    private void SetupRigidbody ()
    {
        _Rigidbody.isKinematic = true;
        _Rigidbody.useGravity = false;
        _Rigidbody.freezeRotation = true;
    }

    public void Update ()
    {
        if (_CanMove)
        {
            GetInput ();
            CheckPosition ();
        }
    }

    private void GetInput ()
    {
        if (_Direction == Vector3.zero)
        {
            if (Input.GetKeyDown (KeyCode.W))
            {
                _Direction = Vector3.up;
                //_Rigidbody.rotation = Quaternion.Euler (Vector3.zero);
            }
            else if (Input.GetKeyDown (KeyCode.S))
            {
                _Direction = Vector3.down;
                //_Rigidbody.rotation = Quaternion.Euler (new Vector3 (0, 0, 180f));
            }
            else if (Input.GetKeyDown (KeyCode.D))
            {
                _Direction = Vector3.right;
                //_Rigidbody.rotation = Quaternion.Euler (new Vector3 (0, 0, 90f));
            }
            else if (Input.GetKeyDown (KeyCode.A))
            {
                _Direction = Vector3.left;
                //_Rigidbody.rotation = Quaternion.Euler (new Vector3 (0, 0, 270f));
            }
        }
    }

    private void FixedUpdate ()
    {
        Move ();
    }

    private void Move ()
    {
        if (_Direction != Vector3.zero)
            _Rigidbody.MovePosition (_Rigidbody.position + _Direction * _MovementSpeed * Time.fixedDeltaTime);
    }

    private void CheckPosition ()
    {
        float rayDistance = 0.5f;
        var ray = new Ray (_Rigidbody.position, _Direction);

        if (Physics.Raycast (ray, out RaycastHit hit, rayDistance))
        {
            if (hit.collider.CompareTag ("Finish"))
            {
                _Direction = Vector3.zero;
                _Rigidbody.position = new Vector3 (Mathf.RoundToInt (_Rigidbody.position.x), Mathf.RoundToInt (_Rigidbody.position.y), 0.0f);
            }
        }
    }
}
