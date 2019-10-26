using UnityEngine;

class FPSDisplay : MonoBehaviour
{
    [Tooltip ("The size of the font on-screen.")]
    [SerializeField] private int _FontSize = 60;
    [Tooltip ("The color of the text on-screen.")]
    [SerializeField] private Color _TextColor = Color.white;
    [Tooltip ("The position of the label on the screen using the Top Left as origin.")]
    [SerializeField] private Vector2 _LablePosition = Vector2.zero;

    /// <summary>The currently elapsed time since the game began.</summary>
    private float _DeltaTime = 0.0f;

    private void Update ()
    {
        CalculateTime ();
    }

    private void CalculateTime ()
    {
        _DeltaTime += ( Time.unscaledDeltaTime - _DeltaTime ) * 0.1f;
    }

    private void OnGUI ()
    {
        var baseStyle = new GUIStyle ();
        var shadowStyle = new GUIStyle ();
        var labelRect = GetLabelRect (baseStyle, shadowStyle);

        string text = GetFPSText ();

        GUI.Label (labelRect, text, baseStyle);
        GUI.Label (new Rect (labelRect.x + 1, labelRect.y + 1, labelRect.width, labelRect.height), text, shadowStyle);
    }

    private Rect GetLabelRect (GUIStyle baseStyle, GUIStyle shadowStyle)
    {
        int width = Screen.width;
        int height = Screen.height;

        var labelRect = new Rect (_LablePosition.x, _LablePosition.y, width, height * 2 / 100);
        baseStyle.alignment = TextAnchor.UpperLeft;
        baseStyle.fontSize = height * 2 / _FontSize;
        baseStyle.normal.textColor = _TextColor;
        shadowStyle.alignment = TextAnchor.UpperLeft;
        shadowStyle.fontSize = height * 2 / _FontSize;
        shadowStyle.normal.textColor = new Color (0f, 0f, 0f, .5f);

        return labelRect;
    }

    private string GetFPSText ()
    {
        float milliseconds = _DeltaTime * 1000.0f;
        float fps = 1.0f / _DeltaTime;
        string text = string.Format ("{0:0.0} ms ({1:0.} fps)", milliseconds, fps);

        return text;
    }
}
