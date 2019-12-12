using UnityEngine;

class FPSDisplay : MonoBehaviour
{
    [Tooltip ("The size of the font on-screen.")]
    [SerializeField] private int _FontSize = 60;
    [Tooltip ("The color of the text on-screen.")]
    [SerializeField] private Color _TextColor = Color.white;
    [Tooltip ("The position of the label on the screen using the Top Left as origin.")]
    [SerializeField] private Vector2 _LabelPosition = Vector2.zero;

    private Rect _LabelRect = Rect.zero;
    private Rect _ShadowRect = Rect.zero;
    private GUIStyle _BaseStyle = null;
    private GUIStyle _ShadowStyle = null;

    /// <summary>The currently elapsed time since the game began.</summary>
    private float _DeltaTime = 0.0f;

    private void Awake ()
    {
        SetLabelRect ();
        _BaseStyle = GetBaseStyle ();
        _ShadowStyle = GetShadowStyle ();
    }

    private GUIStyle GetBaseStyle ()
    {
        int fontSize = (int)_LabelRect.height * 2 / _FontSize;

        var baseStyle = new GUIStyle
        {
            alignment = TextAnchor.UpperLeft,
            fontSize = fontSize
        };

        baseStyle.normal.textColor = _TextColor;

        return baseStyle;
    }

    private GUIStyle GetShadowStyle ()
    {
        int fontSize = (int)_LabelRect.height * 2 / _FontSize;
        var shadowStyle = new GUIStyle
        {
            alignment = TextAnchor.UpperLeft,
            fontSize = fontSize
        };

        shadowStyle.normal.textColor = new Color (0f, 0f, 0f, .5f);

        return shadowStyle;
    }

    private void Update ()
    {
        CalculateTime ();
    }

    private void CalculateTime ()
    {
        const float interval = 0.1f;
        _DeltaTime += ( Time.unscaledDeltaTime - _DeltaTime ) * interval;
    }

    private void OnGUI ()
    {
        SetLabelRect ();
        DisplayLabel ();
    }

    private void SetLabelRect ()
    {
        int width = Screen.width;
        int height = Screen.height;

        _LabelRect = new Rect (_LabelPosition.x, _LabelPosition.y, width, height * 2 / 100);
        _ShadowRect = new Rect (_LabelRect.x + 1, _LabelRect.y + 1, _LabelRect.width, _LabelRect.height);
    }

    private void DisplayLabel ()
    {
        string text = GetFPSText ();

        GUI.Label (_LabelRect, text, _BaseStyle);
        GUI.Label (_ShadowRect, text, _ShadowStyle);
    }

    private string GetFPSText ()
    {
        float milliseconds = _DeltaTime * 1000.0f;
        float fps = 1.0f / _DeltaTime;
        string text = string.Format ("{0:0.0} ms ({1:0.} fps)", milliseconds, fps);

        return text;
    }
}
