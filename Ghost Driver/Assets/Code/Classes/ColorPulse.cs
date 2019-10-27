using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

class ColorPulse : MonoBehaviour
{
    [Tooltip ("The speed of the fade.")]
    [SerializeField] private float _FadeSpeed = 2.0f;

    private Text _Material = null;
    private bool _HasFadedOut = false;

    private void Awake ()
    {
        _Material = GetComponent<Text> ();
    }

    private void OnEnable ()
    {
        StartCoroutine (FadeTo (0.0f));
    }

    private IEnumerator FadeTo (float toAlpha)
    {
        float timer = 0.0f;
        float fromAlpha = _Material.color.a;
        Color startColor = _Material.color;
        _HasFadedOut = !_HasFadedOut;

        while (timer < _FadeSpeed)
        {
            timer += Time.deltaTime;
            float t = timer / _FadeSpeed;
            t = Mathf.Lerp (fromAlpha, toAlpha, LerpCurves.SmootherStep (t));
            _Material.color = new Color (startColor.r, startColor.g, startColor.b, t);

            yield return new WaitForEndOfFrame ();
        }

        _Material.color = new Color (_Material.color.r, _Material.color.g, _Material.color.b, toAlpha);

        if (_HasFadedOut)
            StartCoroutine (FadeTo (1.0f));
        else
            StartCoroutine (FadeTo (0.0f));
    }
}
