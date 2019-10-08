using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//TODO: Implement storing of settings to config file.

public class UIVideoSettingsController : MonoBehaviour
{
    [Tooltip ("The label to display the current AA level to.")]
    [SerializeField] private Text _AALabel = null;
    [Tooltip ("The label to display the current Vsync level to.")]
    [SerializeField] private Text _VsyncLabel = null;
    [Tooltip ("The label to display the current quality level to.")]
    [SerializeField] private Text _QualityLevelLabel = null;

    public void OnEnable ()
    {
        SetLabels ();
    }

    private void SetLabels ()
    {
        if (_AALabel != null)
            _AALabel.text = QualitySettings.antiAliasing.ToString ();

        if (_VsyncLabel != null)
        {
            if (QualitySettings.vSyncCount <= 0)
                _VsyncLabel.text = "Disabled";
            else
                _VsyncLabel.text = "Enabled";
        }

        if (_QualityLevelLabel != null)
            _QualityLevelLabel.text = QualitySettings.names[QualitySettings.GetQualityLevel ()];
    }

    public void IncreaseAA ()
    {
        if (QualitySettings.antiAliasing <= 0)
            QualitySettings.antiAliasing += 2;
        else
            QualitySettings.antiAliasing += QualitySettings.antiAliasing;

        if (_AALabel != null)
            _AALabel.text = QualitySettings.antiAliasing.ToString ();
    }

    public void DecreaseAA ()
    {
        QualitySettings.antiAliasing -= QualitySettings.antiAliasing / 2;

        if (_AALabel != null)
            _AALabel.text = QualitySettings.antiAliasing.ToString ();
    }

    public void EnableVsync (bool enable)
    {
        if (enable)
        {
            QualitySettings.vSyncCount = 1;

            if (_VsyncLabel != null)
                _VsyncLabel.text = "Enabled";

            return;
        }

        QualitySettings.vSyncCount = 0;

        if (_VsyncLabel != null)
            _VsyncLabel.text = "Disabled";
    }

    public void IncreaseQualityLevel ()
    {
        QualitySettings.IncreaseLevel (true);

        if (_QualityLevelLabel != null)
            _QualityLevelLabel.text = QualitySettings.names[QualitySettings.GetQualityLevel ()];
    }

    public void DecreaseQualityLevel ()
    {
        QualitySettings.DecreaseLevel (true);

        if (_QualityLevelLabel != null)
            _QualityLevelLabel.text = QualitySettings.names[QualitySettings.GetQualityLevel ()];
    }
}
