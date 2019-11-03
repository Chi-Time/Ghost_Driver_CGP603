using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

//TODO: Implement relic text system.

class UIRelicScreenController : MonoBehaviour, IWakeable
{
    [Tooltip ("The label to use for displaying the relic's name.")]
    [SerializeField] private Text _RelicNameLabel = null;
    [Tooltip ("The label to use for displaying the relic's description.")]
    [SerializeField] private Text _RelicTextLabel = null;

    //TODO: Consider creating signal states that define when something new has happened.
    //TODO: Seperate relic into a seperate signal state so that the player can check and disable themselves.
    /// <summary>Reference to the First Person Controller.</summary>
    private FirstPersonController _FPSController = null;

    public void Waken ()
    {
        _FPSController = FindObjectOfType<FirstPersonController> ().GetComponent<FirstPersonController> ();
    }

    public void DisplayText (string relicName, string relicText)
    {
        this.gameObject.SetActive (true);

        _RelicNameLabel.text = relicName;
        _RelicTextLabel.text = relicText;
    }

    private void OnEnable ()
    {
        DisplayRelicMenu (true);
    }

    private void DisplayRelicMenu (bool shouldShow)
    {
        if (shouldShow)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            _FPSController.enabled = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _FPSController.enabled = true;
            this.gameObject.SetActive (false);
        }
    }

    public void Close ()
    {
        DisplayRelicMenu (false);   
    }
}
