using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent (typeof (Outline), typeof (Collider))]
public class Relic : MonoBehaviour
{
    [SerializeField] private UIRelicScreenController _RelicScreen = null;

    /// <summary>Reference to the outline compnent added to the object.</summary>
    private Outline _Outline = null;

    private void Awake ()
    {
        _Outline = GetComponent<Outline> ();
        _Outline.enabled = false;
        GetComponent<Collider> ().isTrigger = true;
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
        if (Input.GetKey (KeyCode.E) | Input.GetMouseButtonDown (0))
        {
            Destroy (this.gameObject);
            _RelicScreen.gameObject.SetActive (true);
        }
    }

    private void CollectRelic ()
    {
        //TODO: Implement relic collection logic here.
    }
}
