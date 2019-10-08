using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

class UIOptionsScreenController : MonoBehaviour
{
    [SerializeField] private Button _CurrentTab = null;
    [SerializeField] private GameObject _CurrentTabScreen = null;

    private void OnEnable ()
    {
        _CurrentTab.Select ();
        _CurrentTabScreen.SetActive (true);
    }

    public void DisplayTab (GameObject tab)
    {
        if (_CurrentTabScreen != null)
            _CurrentTabScreen.SetActive (false);

        _CurrentTabScreen = tab;
        _CurrentTabScreen.SetActive (true);
    }

    public void DisplayScreen (GameObject screen)
    {
        if (screen != null)
        {
            this.gameObject.SetActive (false);
            screen.SetActive (true);
        }
    }
}
