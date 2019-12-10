using UnityEngine;

class AudioPlayer : MonoBehaviour
{
    [Tooltip ("The volume at which to play the audio clip.")]
    [SerializeField] private float _Volume = 1.0f;
    [Tooltip ("The minimum pitch at which to randomly play the clip.")]
    [SerializeField] private float _MinPitch = 0.95f;
    [Tooltip ("The maximum pitch at which to randomly play the clip.")]
    [SerializeField] private float _MaxPitch = 1.05f;
    [Tooltip ("The audio clip to play upon the collection of this object.")]
    [SerializeField] private AudioClip _AudioClip = null;

    /// <summary>The audio manager who holds the audio source in the world.</summary>
    private GameObject _AudioManager = null;
    /// <summary>The audio source </summary>
    private AudioSource _AudioSource = null;

    private void Awake ()
    {
        AssignReferences ();
        AttachToHolder ();
    }

    // Create a new object and assign our references upon creation.
    private void AssignReferences ()
    {
        string name = $"{_AudioClip.name}: Audio Manager";

        _AudioManager = new GameObject (name);
        _AudioSource = _AudioManager.AddComponent<AudioSource> ();
    }

    // Attach the reference to the scene holder responsible for audio.
    private void AttachToHolder ()
    {
        // Names of the objects to find in the scene.
        string audioHolderName = "Audio Holder";
        string ownerHolderName = $"{gameObject.name} Holder";

        // Grab the holder responsible for all scene audio.
        var audioHolder = GetHolder (audioHolderName);
        // Grab the child holder responsible for all audio relating to this gameobject.
        var ownerHolder = GetHolder (ownerHolderName);

        // Setup the holder hierarchy with owner as a child of audio and the manager beneath that.
        ownerHolder.transform.SetParent (audioHolder.transform);
        _AudioManager.transform.SetParent (ownerHolder.transform);
    }

    private GameObject GetHolder (string name)
    {
        var audioHolder = GameObject.Find (name);

        if (audioHolder == null)
            audioHolder = new GameObject (name);

        return audioHolder;
    }

    /// <summary>Plays the audio source.</summary>
    public void Play ()
    {
        _AudioSource.volume = _Volume;
        _AudioSource.pitch = Random.Range (_MinPitch, _MaxPitch);

        _AudioSource.PlayOneShot (_AudioClip);
    }
}
