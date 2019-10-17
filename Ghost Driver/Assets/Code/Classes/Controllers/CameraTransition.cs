using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class CameraTransition : MonoBehaviour
{
    [Tooltip ("How long the object should take to fade in.")]
    [SerializeField] private float _FadeLength = 2.0f;

    private Material _Material = null;

    private void Awake ()
    {
        var renderer = GetComponent<MeshRenderer> ();
        _Material = renderer.material;
        _Material.color = new Color (_Material.color.r, _Material.color.g, _Material.color.b, 0.0f);
    }

    private void OnEnable ()
    {
        Signals.OnLevelTransition += OnTransitionLevel;
    }

    private void OnDisable ()
    {
        Signals.OnLevelTransition -= OnTransitionLevel;
    }

    private void OnTransitionLevel ()
    {
        StartCoroutine (Fade (_FadeLength, _Material, 1.0f));
    }

    IEnumerator Fade (float length, Material material, float targetOpacity)
    {
        float timer = 0.0f;
        float currentOpacity = material.color.a;

        while (timer < length)
        {
            timer += Time.deltaTime;
            float opacity = Mathf.Lerp (currentOpacity, targetOpacity, timer / length);
            material.color = new Color (material.color.r, material.color.g, material.color.b, opacity);

            yield return new WaitForEndOfFrame ();
        }

        // Change blending mode from Fade to Opaque and stop z depth drawing.
        material.SetColor ("_Color", new Color (material.color.r, material.color.g, material.color.b, targetOpacity));

        Signals.FinishTransition ();
    }
}
