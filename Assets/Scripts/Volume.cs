using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volume : MonoBehaviour
{

    public static float MicLoudness;

    private string _device;

    bool _isInitialized;

    AudioClip _clipRecord;
    int _sampleWindow = 128;

    void OnDisable()
    {
        StopMicrophone();
    }

    private void Start()
    {
        InitMic();
    }
    private void Update()
    {
        MicLoudness = LevelMax();

        print(MicLoudness);
    }


    //mic initialization
    void InitMic()
    {
        if (_device == null) _device = Microphone.devices[0];
        _clipRecord = Microphone.Start(_device, true, 999, 44100);
    }

    void StopMicrophone()
    {
        Microphone.End(_device);
    }



    //get data from microphone into audioclip
    float LevelMax()
    {
        float levelMax = 0;
        float[] waveData = new float[_sampleWindow];
        int micPosition = Microphone.GetPosition(null) - (_sampleWindow + 1); // null means the first microphone
        if (micPosition < 0) return 0;
        _clipRecord.GetData(waveData, micPosition);

        // Getting a peak on the last 128 samples
        for (int i = 0; i < _sampleWindow; i++)
        {
            float wavePeak = waveData[i] * waveData[i];
            if (levelMax < wavePeak)
            {
                levelMax = wavePeak;
            }
        }
        return levelMax * 100;
    }


}
