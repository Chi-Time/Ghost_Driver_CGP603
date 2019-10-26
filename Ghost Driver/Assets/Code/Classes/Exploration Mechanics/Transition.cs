using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent (typeof (Rigidbody), typeof (Collider))]
class Transition : MonoBehaviour
{
    [Tooltip ("The scene to transition to.")]
    [SerializeField] private string _Scene = "";

    private void Awake ()
    {
        var rigidbody = GetComponent<Rigidbody> ();
        rigidbody.isKinematic = true;
        rigidbody.useGravity = false;
        rigidbody.freezeRotation = true;

        GetComponent<Collider> ().isTrigger = true;
    }

    private void OnEnable ()
    {
        Signals.OnTransitionFinished += OnTransitionFinished;
    }

    private void OnDisable ()
    {
        Signals.OnTransitionFinished += OnTransitionFinished;
    }

    private void OnTriggerEnter (Collider other)
    {
        if (other.CompareTag ("Player"))
        {
            Signals.TransitionLevel ();
        }
    }

    public void OnTransitionFinished ()
    {
        SceneManager.LoadScene (_Scene);
    }
}
