using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

//TODO: Create states for each part of the game. Paused state, Reading Relic State, Talking state, Level transition state, etc.

class ExplorationController : GameController
{
    public static ExplorationController Instance { get; private set; }

    [Tooltip ("The song to play as the background music for the level.")]
    [SerializeField] private AudioClip _BGM = null;

    private void Awake ()
    {
        SetInstance ();
        ChangeMusicTrack ();
    }

    private void SetInstance ()
    {
        // If more than one instance exists, delete any extras.
        if (Instance != null && Instance != this)
        {
            Destroy (this);
            return;
        }

        Instance = this;
    }

    private void ChangeMusicTrack ()
    {
        if (MusicController.Instance != null)
        {
            var currentTrack = MusicController.Instance.GetCurrentTrackName ();

            if (currentTrack != _BGM.name)
                MusicController.Instance.ChangeTrack (_BGM);
        }
    }

    //private void OnEnable ()
    //{
    //    ExplorationSignals.OnLevelTransition += OnLevelTransition;
    //}

    //private void OnDisable ()
    //{
    //    ExplorationSignals.OnLevelTransition -= OnLevelTransition;
    //}
}
