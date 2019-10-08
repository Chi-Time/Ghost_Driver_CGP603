using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICreditsScreenController : MonoBehaviour
{
    public void BackToMenu (GameObject screen)
    {
        if (screen != null)
        {
            this.gameObject.SetActive (false);
            screen.SetActive (true);
        }
    }
}
