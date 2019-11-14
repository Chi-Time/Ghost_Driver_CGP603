using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent (typeof (Outline), typeof (Collider))]
public class Relic : MonoBehaviour
{
    [SerializeField] private AudioSource _AudioManager = null;
    [SerializeField] private AudioClip _CollectionSFX = null;
    [Tooltip ("The relic's text to display to the player.")]
    [SerializeField] private TextAsset _RelicText = null;

    /// <summary>Reference to the outline compnent added to the object.</summary>
    private Outline _Outline = null;
    /// <summary>Reference to the parsed relic scene.</summary>
    private RelicInfo _RelicScene = null;

    private void Awake ()
    {
        _Outline = GetComponent<Outline> ();
        _Outline.enabled = false;
        GetComponent<Collider> ().isTrigger = true;

        _RelicScene = new RelicInfo (GetScene ());
    }

    private DialogueScene GetScene ()
    {
        var scene = JsonUtility.FromJson<DialogueScene> (_RelicText.text);

        if (scene == null)
        {
            Debug.LogError ("Relic text cannot be parsed! Ensure it's assigned and correct.");
            return null;
        }

        return scene;
    }

    private void OnMouseEnter ()
    {
        _Outline.enabled = true;
    }

    private void OnMouseExit ()
    {
        _Outline.enabled = false;
    }

    private void OnMouseOver ()
    {
        if (Input.GetKeyDown (KeyCode.E) | Input.GetMouseButtonDown (0))
        {
            Destroy (this.gameObject);
            CollectRelic ();
            _AudioManager.PlayOneShot (_CollectionSFX);
        }
    }

    private void CollectRelic ()
    {
        ExplorationSignals.CollectRelic (_RelicScene);
        LogBook.AddNewRelic (_RelicScene);
    }
}
