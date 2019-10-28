using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent (typeof (AudioSource))]
class MusicController : MonoBehaviour
{
    public static MusicController Instance { get; private set; }

    private AudioSource _AudioSource = null;

    public void ChangeTrack (AudioClip song)
    {
        _AudioSource.clip = song;
        _AudioSource.Play ();
    }

    public string GetCurrentTrackName ()
    {
        if (_AudioSource.clip == null)
            return " ";

        return _AudioSource.clip.name;
    }

    private void Awake ()
    {
        // If more than one instance exists, delete any extras.
        if (Instance != null && Instance != this)
        {
            Destroy (this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad (this);
        _AudioSource = GetComponent<AudioSource> ();
    }
}
