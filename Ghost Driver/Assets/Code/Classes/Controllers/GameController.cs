using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class GameController : MonoBehaviour
{
    [SerializeField] protected UIPauseScreenController _PauseScreen = null;

    protected bool _IsPaused = false;

    protected void Update ()
    {
        if (Input.GetKeyDown (KeyCode.Escape))
        {
            PauseGame ();
        }
    }

    public virtual void PauseGame ()
    {
        if (_IsPaused)
        {
            Time.timeScale = 1.0f;

            if (_PauseScreen != null)
                _PauseScreen.gameObject.SetActive (false);
        }
        else
        {
            if (_PauseScreen != null)
                _PauseScreen.gameObject.SetActive (true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            Time.timeScale = 0.0f;
        }

        _IsPaused = !_IsPaused;
    }
}
