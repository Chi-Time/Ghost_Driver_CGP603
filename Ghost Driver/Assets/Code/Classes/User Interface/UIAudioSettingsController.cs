using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

//TODO: Implement storing of settings to config file.

class UIAudioSettingsController : MonoBehaviour
{
    [Tooltip ("The label to display the current music volume.")]
    [SerializeField] private Text _MusicVolumeLabel = null;
    [Tooltip ("The label to display the current sfx volume.")]
    [SerializeField] private Text _SFXVolumeLabel = null;
    [Tooltip ("The label to display the current ambient volume.")]
    [SerializeField] private Text _AmbientVolumeLabel = null;

    public void OnEnable ()
    {
        SetLabels ();
    }

    private void SetLabels ()
    {
        //TODO: Implement grabbing audio from mixers.

        //if (_MusicVolumeLabel != null)
        //    _MusicVolumeLabel.text = musicVolume.ToString ("0.0");

        //if (_SFXVolumeLabel != null)
        //    _SFXVolumeLabel.text = sfxVolume.ToString ("0.0");

        //if (_AmbientVolumeLabel != null)
        //    _AmbientVolumeLabel.text = ambientVolume.ToString ("0.0");
    }

    public void MusicVolume (float musicVolume)
    {
        if (_MusicVolumeLabel != null)
            _MusicVolumeLabel.text = musicVolume.ToString ("0");
    }

    public void SFXVolume (float sfxVolume)
    {
        if (_SFXVolumeLabel != null)
            _SFXVolumeLabel.text = sfxVolume.ToString ("0");
    }

    public void AmbientVolume (float ambientVolume)
    {
        if (_AmbientVolumeLabel != null)
            _AmbientVolumeLabel.text = ambientVolume.ToString ("0");
    }
}
