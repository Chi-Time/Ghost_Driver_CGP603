using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

//TODO: Make sight preview look nicer and learn how line renderers actually work.

public class LineDrawer
{
    private LineRenderer lineRenderer;
    private float lineSize;

    public LineDrawer (float lineSize = 0.2f)
    {
        GameObject lineObj = new GameObject ("LineObj");
        lineRenderer = lineObj.AddComponent<LineRenderer> ();
        //Particles/Additive
        lineRenderer.material = new Material (Shader.Find ("Unlit/Color"));

        this.lineSize = lineSize;
    }

    private void init (float lineSize = 0.2f)
    {
        if (lineRenderer == null)
        {
            GameObject lineObj = new GameObject ("LineObj");
            lineRenderer = lineObj.AddComponent<LineRenderer> ();
            //Particles/Additive
            lineRenderer.material = new Material (Shader.Find ("Unlit/Color"));

            this.lineSize = lineSize;
        }
    }

    //Draws lines through the provided vertices
    public void DrawLineInGameView (Vector3 start, Vector3 end, Color color)
    {
        if (lineRenderer == null)
        {
            init (0.2f);
        }

        //Set color
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;

        //Set width
        lineRenderer.startWidth = lineSize;
        lineRenderer.endWidth = lineSize;

        //Set line count which is 2
        lineRenderer.positionCount = 2;

        //Set the postion of both two lines
        lineRenderer.SetPosition (0, start);
        lineRenderer.SetPosition (1, end);
    }

    public void Destroy ()
    {
        if (lineRenderer != null)
        {
            UnityEngine.Object.Destroy (lineRenderer.gameObject);
        }
    }
}
