using UnityEngine;
using System.Collections;

public class SpectrumBar : MonoBehaviour
{
    public enum BarType { Realtime, PeakLevel, MeanLevel };

    public int index;
    public BarType barType;

    AudioSpectrum spectrum;
	
    void Awake()
    {
        spectrum = FindObjectOfType(typeof(AudioSpectrum)) as AudioSpectrum;
    }

    void Update ()
    {
        if (index < spectrum.Levels.Length) {
            float scale = 0.0f;

            switch (barType) {
            case BarType.Realtime:
                scale = spectrum.Levels[index];
                break;
            case BarType.PeakLevel:
                scale = spectrum.PeakLevels[index];
                break;
            case BarType.MeanLevel:
                scale = spectrum.MeanLevels[index];
                break;
            }

            var vs = transform.localScale;
            vs.y = scale * 10.0f;
            transform.localScale = vs;
        }
		//var bar1= Instantiate (barPrefab, new Vector3(0,0,0), transform.rotation) as GameObject;
		//bar1.transform.position=new Vector3(0,20,0);
    }
}