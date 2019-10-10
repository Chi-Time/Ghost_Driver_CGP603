using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class PuzzleController : GameController
{
    public static PuzzleController Instance { get; private set; }

    private void Awake ()
    {
        // If more than one instance exists, delete any extras.
        if (Instance != null && Instance != this)
        {
            Destroy (this);
            return;
        }

        Instance = this;
    }
}
