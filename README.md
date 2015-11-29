# MusicParsingDR
Music parsing project 2015 fall
Contributors : Chuan Ren, Vibha Bhambhani, Lavanya Kumar, Vinayak Goyal


The main class that can be used in a game is the Audio spectrum class. This class has 2 API calls. The PeakLevels function and the PeakLevelsAt function. 

*********API*****************
public float[] PeakLevels() :- 
This function will return the current peaks in an array of floats. The length of the array is 4 floats, i.e. it holds 4 floats. 


********API******************
public float[] PeakLevelsAt(int seconds) :-
This function will return the peaks at a later or previous time given that the peaks have been calculated for that time period. If the peaks have not been calculated then we will return a float array with all negative values in it. An easy way to check that the function returned an error code is to take the sum of the array, if the array is -4 then we have not calculated for that time. The size of the float array returned is 4 i.e. it will hold 4 floats. 


********Internal*************
void CheckBuffers () :- 
This function will allocate space for the internal arrays and will open the file for logging the peaks with the time. It will also make sure that the size of the array matches the number of bands. But that has been fixed to 4 for the purpose of the game. 


********Internal*************
int FrequencyToSpectrumIndex (float f) :-
This function will convert the given frequency and will assign it into one of the bands. This will return the index at which we have to put the frequency peak in the peakLevels array. 


********Internal*************
void Awake () :- 
This function initializes the arrays. Opens the file for logging and gets the audiosource so that we can start parsing the music. 


********Internal*************
void Update () :-
This function is a standard function in any unity based game. It will parse 23ms of data and calculate the peaks. The peaks are written to file with a time stamp and are also stored in memory using the allPeaks array. To get data from the allPeaks array we can use the PeakLevels or PeakLevelsAt functions.  