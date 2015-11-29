
using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(AudioSpectrum))]
public class AudioSpectrumInspector : Editor
{

    #region Temporary state variables
    AnimationCurve curve;
    #endregion

    #region Private functions
    void UpdateCurve ()
    {
        var spectrum = target as AudioSpectrum;

        // Create a new curve to update the UI.
        curve = new AnimationCurve ();

        // Add keys for the each band.
        var bands = spectrum.PeakLevels;
        for (var i = 0; i < bands.Length; i++) {
            curve.AddKey (1.0f / bands.Length * i, bands [i]);
        }
    }
    #endregion

    #region Editor callbacks
    public override void OnInspectorGUI ()
    {
        var spectrum = target as AudioSpectrum;

        // Update the curve only when it's playing.
        if (EditorApplication.isPlaying) {
            UpdateCurve ();
        } else if (curve == null) {
            curve = new AnimationCurve ();
        }

        // Component properties.
  
		spectrum.middleFrequenciesForBands [0] [0] = EditorGUILayout.Slider ("Low", spectrum.middleFrequenciesForBands[0][0], 20, 20000);
		spectrum.middleFrequenciesForBands [0] [1] = EditorGUILayout.Slider ("Middle 1", spectrum.middleFrequenciesForBands[0][1], 20, 20000);
		spectrum.middleFrequenciesForBands [0] [2] = EditorGUILayout.Slider ("Middle 2", spectrum.middleFrequenciesForBands[0][2], 20, 20000);
		spectrum.middleFrequenciesForBands [0] [3] = EditorGUILayout.Slider ("High", spectrum.middleFrequenciesForBands[0][3], 20, 20000);

		spectrum.fallSpeed = EditorGUILayout.Slider ("Fall speed", spectrum.fallSpeed, 0.01f, 0.5f);
        spectrum.sensibility = EditorGUILayout.Slider ("Sensibility", spectrum.sensibility, 1.0f, 20.0f);

        // Shows the spectrum curve.
        EditorGUILayout.CurveField (curve, Color.white, new Rect (0, 0, 1.0f, 0.1f), GUILayout.Height (64));

        // Update frequently while it's playing.
        if (EditorApplication.isPlaying) {
            EditorUtility.SetDirty (target);
        }
    }
    #endregion
}