using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

static class Extensions
{
    public static T[] FindAllObjectsOfType<T> (this GameObject gameObject)
    {
        var objects = new List<T> ();
        var scene = SceneManager.GetActiveScene ();
        var roots = scene.GetRootGameObjects ();

        foreach (GameObject root in roots)
        {
            objects.AddRange (root.GetComponentsInChildren<T> (true));
        }

        return objects.ToArray<T> ();
    }

    public static Color Alpha (this Color color, float alpha)
    {
        return new Color (color.r, color.g, color.b, alpha);
    }
}
