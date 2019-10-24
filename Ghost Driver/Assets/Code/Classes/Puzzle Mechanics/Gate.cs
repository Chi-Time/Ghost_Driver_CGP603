﻿using UnityEngine;

[RequireComponent (typeof (Collider))]
public class Gate : MonoBehaviour
{
    public Key Key { get; set; }

    [SerializeField] private GameObject _Key = null;

    private Renderer _Renderer = null;
    private Collider _Collider = null;

    private void Start ()
    {
        if (Key != null)
        {
            _Renderer = GetComponent<Renderer> ();
            _Collider = GetComponent<Collider> ();

            _Renderer.enabled = false;
            _Collider.enabled = false;
            transform.GetChild (0).gameObject.SetActive (false);
        }
    }

    public void Update ()
    {
        if (Key != null)
        {
            if (_Key.activeSelf == false && _Renderer != null)
            {
                _Renderer.enabled = true;
                _Collider.enabled = true;
                transform.GetChild (0).gameObject.SetActive (true);
            }
            else if (_Key.activeSelf == true && _Renderer != null)
            {
                _Renderer.enabled = false;
                _Collider.enabled = false;
                transform.GetChild (0).gameObject.SetActive (false);
            }
        }
    }

    private void OnTriggerEnter (Collider other)
    {
        if (other.CompareTag ("Player"))
        {
            //other.transform.position = new Vector3 (Mathf.RoundToInt (other.transform.position.x), Mathf.RoundToInt (other.transform.position.y), 0.0f);

            print ("YO");

            Signals.CompletePuzzle ();
        }
    }
}