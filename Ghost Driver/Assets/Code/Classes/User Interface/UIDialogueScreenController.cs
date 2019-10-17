using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

class UIDialogueScreenController : MonoBehaviour
{
    private Text _RelicText = null;

    private void OnEnable ()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GameObject.FindGameObjectWithTag ("Player").GetComponent<FirstPersonController> ().enabled = false;
    }

    public void Close ()
    {
        this.gameObject.SetActive (false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        GameObject.FindGameObjectWithTag ("Player").GetComponent<FirstPersonController> ().enabled = true;
    }
}
