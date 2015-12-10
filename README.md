# MusicParsingDR
##Music parsing project 2015 fall
*Contributors : Chuan Ren, Vibha Bhambhani, Lavanya Kumar, Vinayak Goyal*

The game uses mainly two class files:
1. Audio Spectrum Class : contains core logic of the game.
2. Audio Spectrum Inspector : contains UI logic and slider descriptions.

###**AUDIO SPECTRUM CLASS**
This class has 2 API calls. The *PeakLevels* function and the *PeakLevelsAt* function. 

### API Funtions

**public float[] PeakLevels()** :- 
This function will return the current peaks in an array of floats. The length of the array is 4 floats, i.e. it holds 4 floats. 

**public float[] PeakLevelsAt(int seconds)** :-
This function will return the peaks at a later or previous time given that the peaks have been calculated for that time period. If the peaks have not been calculated then we will return a float array with all negative values in it. An easy way to check that the function returned an error code is to take the sum of the array, if the array is -4 then we have not calculated for that time. The size of the float array returned is 4 i.e. it will hold 4 floats. 


### Internal Funtions

**void CheckBuffers ()** :- 
This function will allocate space for the internal arrays *peakLevels* and *rawSpectrum*. *peakLevels* stores the current peaks and the *rawSpectrum* captures the audio spectrum data. It will also make sure that the size of the peak levels array matches the number of bands and the raw spectrum array matches the number of samples. Size of peak levels has been fixed to 4 and raw spectrum has been set to 1024 samples for the purpose of the game. 

**int FrequencyToSpectrumIndex (float f)** :-
This function will convert the given frequency and will assign it into one of the bands. This will return the index at which we have to put the frequency peak in the *peakLevels* array. 

**void Awake ()** :- 
This function initializes the arrays. It will open the file for logging the peaks with the time and get the audiosource so that we can start parsing the music. 

**void Update ()** :-
This function is a standard function in any unity based game. It will parse 23ms of data and calculate the peaks. The peaks are written to file with a time stamp and are also stored in memory using the *allPeaks* array. To get data from the *allPeaks* array we can use the *PeakLevelsAt* function.  

###**AUDIO SPECTRUM INSPECTOR CLASS**
This class has 2 internal functions.The *UpdateCurve* function and *OnInspectorGUI* function.

**void UpdateCurve ()** :-
This function creates a new curve to update the UI and adds keys for each spectrum band.
 
**public override void OnInspectorGUI ()** :-
This function is a standard function for GUI in any unity based game.It ensures that the curve is updated only when music is playing.It also contains six slider components used to set four band frequencies,fall speed and sensibility. It ensures that the spectrum curve is displayed properly and updated frequently.
