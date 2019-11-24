using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent (typeof (MeshRenderer))]
class FadeIn : MonoBehaviour
{
    [Tooltip ("How long the object should take to fade in.")]
    [SerializeField] private float _FadeLength = 2.0f;

    private Material[] _Materials = null;

    private void Awake ()
    {
        var renderer = GetComponent<MeshRenderer> ();
        _Materials = renderer.materials;

        SetDefaultState ();
    }

    private void SetDefaultState ()
    {
        foreach (Material material in _Materials)
        {
            if (material.HasProperty ("_Mode"))
            {
                // Change blending mode from Opaque to Fade and allow z depth drawing.
                material.SetFloat ("_Mode", 2);
                material.SetInt ("_ZWrite", 0);
                material.color = new Color (material.color.r, material.color.g, material.color.b, 0.0f);
            }
        }
    }

    public void Enable (bool shouldFadeIn)
    {
        StopAllCoroutines ();

        if (shouldFadeIn)
        {
            foreach (Material material in _Materials)
            {
                // Change blending mode from Opaque to Fade and allow z depth drawing.
                material.SetFloat ("_Mode", 2);
                material.SetInt ("_ZWrite", 0);
                StartCoroutine (Fade (_FadeLength, material, 1.0f));
            }

            return;
        }

        foreach (Material material in _Materials)
        {
            // Change blending mode from Opaque to Fade and allow z depth drawing.
            material.SetFloat ("_Mode", 2);
            material.SetInt ("_ZWrite", 0);
            StartCoroutine (Fade (_FadeLength, material, 0.0f));
        }
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
        material.SetFloat ("_Mode", 0);
        material.SetInt ("_ZWrite", 1);
        material.SetColor ("_Color", new Color (material.color.r, material.color.g, material.color.b, targetOpacity));
    }

    private void OnDisable ()
    {
        StopAllCoroutines ();
    }
}
