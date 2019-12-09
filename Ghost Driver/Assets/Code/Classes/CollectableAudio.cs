using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class CollectableAudio : MonoBehaviour
{
    [Tooltip ("The audio clip to play upon the collection of this object.")]
    [SerializeField] private AudioClip _AudioClip = null;

    /// <summary>The audio manager who holds the audio source in the world.</summary>
    private GameObject _AudioManager = null;
    /// <summary>The audio source </summary>
    private AudioSource _AudioSource = null;

    private void Awake ()
    {
        const string audioHolderName = "Audio Holder";
        _AudioManager = new GameObject ();
        _AudioSource = _AudioManager.AddComponent<AudioSource> ();

        var audioHolder = GameObject.Find (audioHolderName);

        if (audioHolder == null)
            new GameObject (audioHolderName);

        _AudioManager.transform.SetParent (audioHolder.transform);
    }

    /// <summary>Plays a given audio source.</summary>
    public void Play ()
    {
        _AudioSource.PlayOneShot (_AudioClip);
        Destroy (_AudioManager, _AudioSource.clip.length);
    }
}
