using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent (typeof (Material))]
class EmissivePulser : MonoBehaviour
{
    [Tooltip ("The speed at which the emission will pulse.")]
    [SerializeField] private float _PulseSpeed = 0.0f;
    [Tooltip ("The strength of the emission at it's peak.")]
    [SerializeField] private float _PulseIntensity = 0.75f;
    [Tooltip ("The type of lerp to give the pulse animation.")]
    [SerializeField] private LerpType _LerpType = LerpType.SmootherStep;

    private Material _Material = null;
    private Color _BaseColor = Color.white;

    private void Awake ()
    {
        _Material = GetComponent<Renderer> ().material;
    }

    private void Start ()
    {
        _BaseColor = _Material.GetColor ("_EmissionColor");
        Color.RGBToHSV (_BaseColor, out _BaseColor.r, out _BaseColor.g, out _BaseColor.b);
        _Material.SetColor ("_EmissionColor", Color.HSVToRGB (0f, 0f, 0f));

        StartCoroutine (PulseTo (_PulseIntensity));
    }

    //TODO: Fix weird intensity pop.
    private IEnumerator PulseTo (float endValue)
    {
        float timer = 0.0f;
        Color startColor = _Material.GetColor ("_EmissionColor");
        float startValue = 0.0f;
        Color.RGBToHSV (startColor, out startColor.r, out startColor.g, out startValue);

        while (timer <= _PulseSpeed)
        {
            timer += Time.fixedDeltaTime;
            float t = LerpCurves.Curve (timer / _PulseSpeed, _LerpType);

            float value = Mathf.Lerp (startColor.b, endValue, t);
            _Material.SetColor ("_EmissionColor", Color.HSVToRGB (_BaseColor.r, _BaseColor.g, value));

            yield return new WaitForFixedUpdate ();
        }

        _Material.SetColor ("_EmissionColor", Color.HSVToRGB (_BaseColor.r, _BaseColor.g, endValue));

        StartCoroutine (PulseTo (startValue));
    }
}
