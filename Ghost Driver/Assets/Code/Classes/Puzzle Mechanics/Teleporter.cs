using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent (typeof (Collider))]
public class Teleporter : MonoBehaviour
{
    public Transform Transform { get { return _Transform; } }
    public int ID { get { return _ID; } }
    public bool CanTeleport { get { return _CanTeleport; } set { _CanTeleport = value; } }

    [Tooltip ("The ")]
    [SerializeField] private int _ID = 0;

    private bool _CanTeleport = true;
    private Teleporter _Partner = null;
    private Transform _Transform = null;

    private void Awake ()
    {
        _Transform = GetComponent<Transform> ();
        GetComponent<Collider> ().isTrigger = true;

        FindPartner ();
    }

    /// <summary>Loop through every teleporter in the level to find the partner to this one.</summary>
    private void FindPartner ()
    {
        var teleporters = GameObject.FindObjectsOfType<Teleporter> ();

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
            _Partner.CanTeleport = false;
            other.transform.position = new Vector3 (_Partner.Transform.position.x, _Partner.Transform.position.y, 0.0f);
            Invoke ("ActivatePartner", 0.05f);
        }
    }

    private void ActivatePartner ()
    {
        _Partner.CanTeleport = true;
    }
}
