using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CalibrateController : MicrophoneController
{

  
    public bool calibrateStart;
    private float volume;
    private List<float> listNoise = new List<float>();
    // Start is called before the first frame update
    void Start()
    {
        CheckMicro();


    }

    // Update is called once per frame
    void Update()
    {

        if (calibrateStart)
        {
            volume = CheckLevelMicro();
            listNoise.Add(volume);

        }     
    }


    public void StartCalibrate()
    {
        calibrateStart = true;
        listNoise.Clear();
        StartMicro();
    }




    public float StopCalibrate()
    {
        float avarageNoise = listNoise.Average();
        DataController.playerData.AvarageNosie = avarageNoise;
        calibrateStart = false;
        DataController.playerData.Calibrated = true;
        return avarageNoise;
    }
}
