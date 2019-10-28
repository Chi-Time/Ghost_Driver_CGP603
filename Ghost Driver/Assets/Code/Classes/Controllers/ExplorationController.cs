﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class ExplorationController : GameController
{
    public static ExplorationController Instance { get; private set; }

    [Tooltip ("The song to play as the background music for the level.")]
    [SerializeField] private AudioClip _BGM = null;

    private void Awake ()
    {
        // If more than one instance exists, delete any extras.
        if (Instance != null && Instance != this)
        {
            Destroy (this);
            return;
        }

        Instance = this;

        var currentTrack = MusicController.Instance.GetCurrentTrackName ();

        if (currentTrack != _BGM.name)
            MusicController.Instance.ChangeTrack (_BGM);
    }
}
