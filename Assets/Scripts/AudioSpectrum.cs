
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class AudioSpectrum : MonoBehaviour
{
    #region Band type definition
    public enum BandType {
        FourBand
    };

    public float[][] middleFrequenciesForBands = {
		new float[]{ 80f,240f ,3000f, 32000000f} //have to get two other frequencies
    				//kick drum, snare drum, hi hat
	};
    static float[] bandwidthForBands = {
        1.260f
    };
    #endregion

    #region Public variables
    public int numberOfSamples = 1024;
    public BandType bandType = BandType.FourBand;
    public float fallSpeed = 0.08f; //inital value was 0.08f
    //public float sensibility = 8.0f;
    #endregion

    #region Private variables
    float[] rawSpectrum;

    float[] peakLevels;

	List<float> allPeaks = new List<float> ();
	AudioSource audioSource;
	private StreamWriter writer;
	int time=23;
	private int time_duration = 23; // 23 ms
	
    #endregion

    #region Public property


    public float[] PeakLevels {
        get { return peakLevels; }
    }
    
	 
	public float[] PeakLevelsAt(int seconds){
		float[] retVal = new float[4];
		int index = (seconds * 1000 / 23) - 1;
		if (index < allPeaks.Count) {
			for (int i =0; i<4; i++) {
				retVal [i] = allPeaks [index + i];
			}
			return retVal;
		} else {
			for(int i=0; i<4; i++) {
				retVal [i] = -1; 
			}
			return retVal;
		}
	}

    #endregion

    #region Private functions
    void CheckBuffers ()
    {
        if (rawSpectrum == null || rawSpectrum.Length != numberOfSamples) {
            rawSpectrum = new float[numberOfSamples];
        }
        var bandCount = middleFrequenciesForBands [(int)bandType].Length;
        if (peakLevels == null || peakLevels.Length != bandCount) {

            peakLevels = new float[bandCount];

        }
    }

    int FrequencyToSpectrumIndex (float f)
    {
        var i = Mathf.FloorToInt (f / AudioSettings.outputSampleRate * 2.0f * rawSpectrum.Length);
        return Mathf.Clamp (i, 0, rawSpectrum.Length - 1);
    }
    #endregion

    #region Monobehaviour functions
    void Awake ()
    {
        CheckBuffers ();
		audioSource = GetComponent<AudioSource>();
		writer=new StreamWriter("DebugLog.txt");
    }

    void Update ()
    {
		if (audioSource.isPlaying) 
		{
				CheckBuffers ();
				audioSource.GetSpectrumData (rawSpectrum, 0, FFTWindow.BlackmanHarris);

				float[] middlefrequencies = middleFrequenciesForBands [(int)bandType];
				var bandwidth = bandwidthForBands [(int)bandType];

				var falldown = fallSpeed * Time.deltaTime;
				//var filter = Mathf.Exp (-sensibility * Time.deltaTime);

				writer.WriteLine("TIME:"+time +"ms");
				for (var bi = 0; bi < peakLevels.Length; bi++) {
					int imin = FrequencyToSpectrumIndex (middlefrequencies [bi] / bandwidth);
					int imax = FrequencyToSpectrumIndex (middlefrequencies [bi] * bandwidth);

					var bandMax = 0.0f;
					for (var fi = imin; fi <= imax; fi++) {
						bandMax = Mathf.Max (bandMax, rawSpectrum [fi]);
					}


					peakLevels [bi] = Mathf.Max (peakLevels [bi] - falldown, bandMax);
					allPeaks.Add (peakLevels [bi]);
					Debug.Log (allPeaks[allPeaks.Count-1]);
					Debug.Log (allPeaks.Count);
					writer.WriteLine(peakLevels[bi]);
			
				}
			time=time+time_duration;
		}

    }
    #endregion


}
