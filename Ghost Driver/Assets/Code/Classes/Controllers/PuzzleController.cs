using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

class PuzzleController : GameController
{
    public static PuzzleController Instance { get; private set; }
    [SerializeField] private string _Scene = "";

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

    private void OnEnable ()
    {
        Signals.OnPuzzleComplete += OnPuzzleComplete;
    }

    private void OnPuzzleComplete ()
    {
        SceneManager.LoadScene (_Scene);
    }

    private void OnDisable ()
    {
        Signals.OnPuzzleComplete += OnPuzzleComplete;
    }
}
