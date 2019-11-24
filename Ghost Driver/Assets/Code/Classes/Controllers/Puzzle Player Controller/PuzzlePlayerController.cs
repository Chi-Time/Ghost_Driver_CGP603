﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Make a snake style highlight tile mover. 
// Every time the player moves there are a dynamic number of tiles that move
// Progressively behind them with varied transparency.
[RequireComponent (typeof (Collider), typeof (Rigidbody))]
public class PuzzlePlayerController : MonoBehaviour
{
    [Tooltip ("The speed at which the player moves.")]
    [SerializeField] private float _MovementSpeed = 5.0f;
    [Tooltip ("The tile prefab to use for highlighting.")]
    [SerializeField] private Transform _HighlightTile = null;

    private bool _CanMove = true;
    private bool _Latched = false;
    private Vector3 _Direction = Vector3.zero;
    private Vector3 _SpawnPosition = Vector3.zero;
    private Quaternion _SpawnRotation = Quaternion.identity;
    private Rigidbody _Rigidbody = null;
    private Transform _CurrentMagnet = null;

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
        _SpawnRotation = _Rigidbody.rotation;

        if (_HighlightTile != null)
        {
            _HighlightTile = Instantiate (_HighlightTile);
        }
    }

    private void SetupRigidbody ()
    {
        _Rigidbody.isKinematic = true;
        _Rigidbody.useGravity = false;
        _Rigidbody.freezeRotation = true;
    }

    public void Update ()
    {
        if (_Latched)
        {
            _Rigidbody.position = _CurrentMagnet.transform.position;
        }

        if (_CanMove)
        {
            GetInput ();
            CheckPosition ();
        }
    }

    private void GetInput ()
    {
        //TODO: Make it so that when leaving a moving block it only takes the move when it's at a whole value so that it doesn't go in-between the grid lines. OR: Make it so that immediately as it leaves it snaps to the moving direction. 
        if (_Direction == Vector3.zero)
        {
            if (Input.GetKeyDown (KeyCode.W))
            {
                _Latched = false;
                _Direction = Vector3.up;
                _Rigidbody.rotation = Quaternion.Euler (Vector3.zero);
            }
            else if (Input.GetKeyDown (KeyCode.S))
            {
                _Latched = false;
                _Direction = Vector3.down;
                _Rigidbody.rotation = Quaternion.Euler (new Vector3 (0, 0, 180f));
            }
            else if (Input.GetKeyDown (KeyCode.D))
            {
                _Latched = false;
                _Direction = Vector3.right;
                _Rigidbody.rotation = Quaternion.Euler (new Vector3 (0, 0, 270f));
            }
            else if (Input.GetKeyDown (KeyCode.A))
            {
                _Latched = false;
                _Direction = Vector3.left;
                _Rigidbody.rotation = Quaternion.Euler (new Vector3 (0, 0, 90f));
            }
        }
    }

    private void FixedUpdate ()
    {
        MoveBody ();
        //MoveHighlightTile ();
    }

    private void MoveBody ()
    {
        if (_Direction != Vector3.zero)
        {
            _Rigidbody.MovePosition (_Rigidbody.position + _Direction * _MovementSpeed * Time.fixedDeltaTime);
        }
    }

    private void MoveHighlightTile ()
    {
        if (_HighlightTile != null)
        {
            _HighlightTile.position = new Vector3 (Mathf.RoundToInt (_Rigidbody.position.x), Mathf.RoundToInt (_Rigidbody.position.y), _HighlightTile.position.z);
        }
    }

    private void CheckPosition ()
    {
        float rayDistance = 0.5f;
        var ray = new Ray (_Rigidbody.position, _Direction);

        if (Physics.Raycast (ray, out RaycastHit hit, rayDistance))
        {
            if (hit.collider.CompareTag ("Finish"))
            {
                Stop ();
            }
        }

        if (Approximation.Float (_Rigidbody.position.x, Mathf.RoundToInt (_Rigidbody.position.x), 0.1f))
        {
            if (Approximation.Float (_Rigidbody.position.y, Mathf.RoundToInt (_Rigidbody.position.y), 0.1f))
            {
                MoveHighlightTile ();
            }
        }
    }

    private void Stop ()
    {
        _Direction = Vector3.zero;
        _Rigidbody.position = new Vector3 (Mathf.RoundToInt (_Rigidbody.position.x), Mathf.RoundToInt (_Rigidbody.position.y), 0.0f);
    }

    private void OnTriggerEnter (Collider other)
    {
        if (other.CompareTag ("Magnet") && _Latched == false)
        {
            _Latched = true;
            _CurrentMagnet = other.transform;
        }

        if (other.CompareTag ("Enemy"))
        {
            PuzzleSignals.FailPuzzle ();
        }
    }

    private void OnTriggerExit (Collider other)
    {
        if (other.CompareTag ("Magnet"))
        {
            _Latched = false;
            _CurrentMagnet = null;
            _Rigidbody.position = new Vector3 (Mathf.RoundToInt (_Rigidbody.position.x), Mathf.RoundToInt (_Rigidbody.position.y), Mathf.RoundToInt (_Rigidbody.position.z));
        }
    }

    private void OnEnable ()
    {
        PuzzleSignals.OnPuzzleReset += OnPuzzleReset;
    }

    private void OnPuzzleReset ()
    {
        _Direction = Vector3.zero;
        _Rigidbody.velocity = Vector3.zero;
        _Rigidbody.position = _SpawnPosition;
        _Rigidbody.rotation = _SpawnRotation;
    }

    private void OnDisable ()
    {
        PuzzleSignals.OnPuzzleReset -= OnPuzzleReset;
    }
}
