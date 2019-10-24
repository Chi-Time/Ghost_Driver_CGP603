using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

class UIRelicScreenController : MonoBehaviour
{
    private Text _RelicText = null;

    private FirstPersonController _FPSController = null;

    private void Start ()
    {
        _FPSController = FindObjectOfType<FirstPersonController> ().GetComponent<FirstPersonController> ();
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
