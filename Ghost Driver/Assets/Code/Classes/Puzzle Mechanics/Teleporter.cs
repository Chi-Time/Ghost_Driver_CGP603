using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

//TODO: Implement better teleporter system which makes use of a parent gameobject that contains them both under it so that it can tell who the partners are and organise it.

[RequireComponent (typeof (Collider))]
public class Teleporter : MonoBehaviour
{
    public char ID { get { return _ID; } }
    public Transform Transform { get { return _Transform; } }
    public bool CanTeleport { get { return _CanTeleport; } set { _CanTeleport = value; } }

    [Tooltip ("The ID of this teleporter and it's partner.")]
    [SerializeField] private char _ID = 'A';

    private bool _CanTeleport = true;
    private float _DelayTimer = 0.05f;
    private Teleporter _Partner = null;
    private Transform _Transform = null;

    private void Awake ()
    {
        Setup ();
    }

    private void Start ()
    {
        FindPartner ();
    }

    private void Setup ()
    {
        _Transform = GetComponent<Transform> ();
        GetComponent<Collider> ().isTrigger = true;
    }

    /// <summary>Loop through every teleporter in the level to find the partner to this one.</summary>
    private void FindPartner ()
    {
        var teleporters = FindObjectsOfType<Teleporter> ();

        // Loop through every teleporter, checking for a match. Ensure that the one found is not itself.
        for (int i = 0; i < teleporters.Length; i++)
            if (teleporters[i].ID == _ID && teleporters[i] != this)
                _Partner = teleporters[i];

        if (_Partner == null)
            Debug.LogError ("No partner found for: " + this.name + "! A teleporter must have a partner in scene with the same ID.");
    }

    private void OnTriggerEnter (Collider other)
    {
        if ((other.CompareTag ("Player") || other.CompareTag ("Enemy")) && _CanTeleport)
        {
            Teleport (other);
        }
    }

    private void Teleport (Collider other)
    {
        float zPos = 0.0f;
        _Partner.CanTeleport = false;
        other.transform.position = new Vector3 (_Partner.Transform.position.x, _Partner.Transform.position.y, zPos);
        Invoke ("ActivatePartner", _DelayTimer);
    }

    private void ActivatePartner ()
    {
        _Partner.CanTeleport = true;
    }
}
