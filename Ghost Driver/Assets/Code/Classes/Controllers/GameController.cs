using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent (typeof (SceneController))]
class GameController : MonoBehaviour
{
    [Tooltip ("The pause screen used for this level.")]
    [SerializeField] protected UIPauseScreenController _PauseScreen = null;

    protected bool _IsPaused = false;

    protected virtual void Update ()
    {
        if (Input.GetKeyDown (KeyCode.Escape))
        {
            PauseGame ();
        }
    }

    public virtual void PauseGame ()
    {
        _IsPaused = !_IsPaused;

        if (_IsPaused)
        {
            Time.timeScale = 0.0f;

            if (_PauseScreen != null)
            {
                _PauseScreen.gameObject.SetActive (true);
            }

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Time.timeScale = 1.0f;

            if (_PauseScreen != null)
            {
                _PauseScreen.gameObject.SetActive (false);
            }

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
