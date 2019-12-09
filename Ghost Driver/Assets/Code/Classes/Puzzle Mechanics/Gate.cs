using UnityEngine;

[RequireComponent (typeof(Renderer), typeof (Collider))]
[RequireComponent (typeof (CollectableAudio))]
public class Gate : MonoBehaviour
{
    public Key Key { get; set; }

    [Tooltip ("The audio manager who controls the audio of this piece.")]
    [SerializeField] private AudioSource _AudioManager = null;
    [Tooltip ("The sound to play upon collection of this object.")]
    [SerializeField] private AudioClip _CollectionSFX = null;
    [Tooltip ("The key used to open the gate if it needs one.")]
    [SerializeField] private GameObject _Key = null;

    private Renderer _Renderer = null;
    private Collider _Collider = null;

    private void Start ()
    {
        if (Key != null)
        {
            _Renderer = GetComponent<Renderer> ();
            _Collider = GetComponent<Collider> ();

            _Renderer.enabled = false;
            _Collider.enabled = false;
            _Collider.isTrigger = true;
            transform.GetChild (0).gameObject.SetActive (false);
        }
    }

    public void Update ()
    {
        if (Key != null)
        {
            if (_Key.activeSelf == false && _Renderer != null)
            {
                _Renderer.enabled = true;
                _Collider.enabled = true;
                transform.GetChild (0).gameObject.SetActive (true);
            }
            else if (_Key.activeSelf == true && _Renderer != null)
            {
                _Renderer.enabled = false;
                _Collider.enabled = false;
                transform.GetChild (0).gameObject.SetActive (false);
            }
        }
    }

    private void OnTriggerEnter (Collider other)
    {
        if (other.CompareTag ("Player"))
        {
            //other.transform.position = new Vector3 (Mathf.RoundToInt (other.transform.position.x), Mathf.RoundToInt (other.transform.position.y), 0.0f);
            _AudioManager.PlayOneShot (_CollectionSFX);
            PuzzleSignals.CompletePuzzle ();
        }
    }
}
