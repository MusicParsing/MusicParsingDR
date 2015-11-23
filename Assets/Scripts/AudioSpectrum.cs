
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class AudioSpectrum : MonoBehaviour
{
    #region Band type definition
    public enum BandType {
        FourBand
		//FourBandVisual,
		//EightBand,
		//TenBand,
		//TwentySixBand,
		//ThirtyOneBand
    };

    static float[][] middleFrequenciesForBands = {
        //new float[]{ 125.0f, 500, 1000, 2000 },
        //new float[]{ 250.0f, 400, 600, 800 },
		//new float[]{ 60f, 80f, 125.0f, 200f },
		new float[]{ 80f,240f ,24000000f, 32000000f}
        //new float[]{ 63.0f, 125, 500, 1000, 2000, 4000, 6000, 8000 },
        //new float[]{ 31.5f, 63, 125, 250, 500, 1000, 2000, 4000, 8000, 16000 },
        //new float[]{ 25.0f, 31.5f, 40, 50, 63, 80, 100, 125, 160, 200, 250, 315, 400, 500, 630, 800, 1000, 1250, 1600, 2000, 2500, 3150, 4000, 5000, 6300, 8000 },
        //new float[]{ 20.0f, 25, 31.5f, 40, 50, 63, 80, 100, 125, 160, 200, 250, 315, 400, 500, 630, 800, 1000, 1250, 1600, 2000, 2500, 3150, 4000, 5000, 6300, 8000, 10000, 12500, 16000, 20000 },
    };
    static float[] bandwidthForBands = {
        //1.414f, // 2^(1/2)
        1.260f, // 2^(1/3)
        //1.414f, // 2^(1/2)
        //1.414f, // 2^(1/2)
        //1.122f, // 2^(1/6)
        //1.122f  // 2^(1/6)
    };
    #endregion

    #region Public variables
    public int numberOfSamples = 1024;
    public BandType bandType = BandType.FourBand;
    public float fallSpeed = 0.08f; //inital value was 0.08f
    public float sensibility = 8.0f;
    #endregion

    #region Private variables
    float[] rawSpectrum;
    //float[] levels;
    float[] peakLevels;
    //float[] meanLevels;

	List<float> allPeaks = new List<float> ();
	AudioSource audioSource;
	private StreamWriter writer;
	int time=23;
	private int time_duration = 23; // 23 ms
	
    #endregion

    #region Public property
//    public float[] Levels {
//        get { return levels; }
//    }

    public float[] PeakLevels {
        get { return peakLevels; }
    }
    
//    public float[] MeanLevels {
//        get { return meanLevels; }
//    }

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
            //levels = new float[bandCount];
            peakLevels = new float[bandCount];
            //meanLevels = new float[bandCount];
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
				//AudioListener.GetSpectrumData (rawSpectrum, 0, FFTWindow.BlackmanHarris);

				float[] middlefrequencies = middleFrequenciesForBands [(int)bandType];
				var bandwidth = bandwidthForBands [(int)bandType];

				var falldown = fallSpeed * Time.deltaTime;
				var filter = Mathf.Exp (-sensibility * Time.deltaTime);

				//var file=File.CreateText("C:\\Users\\pc\\Desktop\\log.txt");
				//file.WriteLine("Peak Levels:");
				//int t=0;
				writer.WriteLine("TIME:"+time +"ms");
				for (var bi = 0; bi < peakLevels.Length; bi++) {
					int imin = FrequencyToSpectrumIndex (middlefrequencies [bi] / bandwidth);
					int imax = FrequencyToSpectrumIndex (middlefrequencies [bi] * bandwidth);

					var bandMax = 0.0f;
					for (var fi = imin; fi <= imax; fi++) {
						bandMax = Mathf.Max (bandMax, rawSpectrum [fi]);
					}

					//levels [bi] = bandMax;
				
					peakLevels [bi] = Mathf.Max (peakLevels [bi] - falldown, bandMax);
					allPeaks.Add (peakLevels [bi]);
					Debug.Log (allPeaks[allPeaks.Count-1]);
					Debug.Log (allPeaks.Count);
					//meanLevels [bi] = bandMax - (bandMax - meanLevels [bi]) * filter;
					writer.WriteLine(peakLevels[bi]);
					//t=t+23;
					//file.WriteLine("Level:"+peakLevels[bi]+"\t Time:"+ t);
			
				}
			time=time+time_duration;
		}
		//file.close();
//		float[] a = PeakLevelsAt (1);
//		for (int i=0; i<4; i++) {
//			Debug.Log(a[i]);
//		}
    }
    #endregion


}