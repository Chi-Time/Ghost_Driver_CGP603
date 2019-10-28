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
        if (Instance != null && Instance != this)
        {
            Destroy (this.gameObject);
            return;
        }

        Instance = this;

        WakeSleepers ();
    }

    private void WakeSleepers ()
    {
        var sleepyObjects = this.gameObject.FindAllObjectsOfType<IWakeable> ();

        foreach (IWakeable sleeper in sleepyObjects)
        {
            sleeper.Waken ();
        }
    }
}
