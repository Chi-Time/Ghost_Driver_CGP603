using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class SceneController : MonoBehaviour
{
    public static SceneController Instance = null;

    private void Awake ()
    {
        SetupInstance ();
    }

    private void Start ()
    {
        WakeSleepers ();
    }

    private void SetupInstance ()
    {
        if (Instance != null && Instance != this)
        {
            Destroy (this.gameObject);
            return;
        }

        Instance = this;
    }

    // Find all inactive objects which have a wakeable interface and wake them up.
    private void WakeSleepers ()
    {
        var sleepyObjects = this.gameObject.FindAllObjectsOfType<IWakeable> ();

        foreach (IWakeable sleeper in sleepyObjects)
        {
            sleeper.Waken ();
        }
    }
}
