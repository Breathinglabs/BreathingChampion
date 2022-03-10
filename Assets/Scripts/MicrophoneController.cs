using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

public class MicrophoneController : MonoBehaviour
{

    private AudioClip clipRecord;

    private string deviceMicro;

    private int Samples = 1024;
  
    private int SampleWindow = 128;
    public bool checkVol = false;



    public  bool MicroStartedGame { get; set; }

    public bool GameStart { get; set; }

    public bool GameFinish { get; set; }

    public bool Connected { get; set; }

    public float Volume { get; set; }

    public float AverageNoise { get; set; }

    private List<float> listNoiseGame = new List<float>();

    // Start is called before the first frame update
    void Start()
    {
      
        
        CheckMicro();
        Volume = CheckLevelMicro();
    }


    // Update is called once per frame
    void Update()
    {
      
      

        Volume = CheckLevelMicro();
        print("Volume Microfone" + Volume.ToString("F2"));
        //print("Volume da Media" + DataController.playerData.AvarageNosie.ToString("F2"));


        if (MicroStartedGame && !GameStart)
        {
            if(Volume > DataController.playerData.AvarageNosie)
            {
                listNoiseGame.Add(Volume);
                if(listNoiseGame.Count > 10)
                {
                  
                    if (listNoiseGame.Average() > (DataController.playerData.AvarageNosie * 3))
                    {
                        GameStart = true;
                        listNoiseGame.Clear();
                    }
                }
               

            }
           
        }
        else if (MicroStartedGame && GameStart)
        {
           

            listNoiseGame.Add(Volume);

            
            if (listNoiseGame.Count > 4)
            {
               

                foreach (float vol in listNoiseGame)
                { 
                    if (vol <= DataController.playerData.AvarageNosie) checkVol = true;
                    else checkVol = false;
                }

                if (checkVol) GameFinish = true;
          

                listNoiseGame.Clear();
            }



        }

        // print("Avarage" + averageNoise);
    }



    public void StartMicroGame()
    {
        listNoiseGame.Clear();
        StartMicro();
        MicroStartedGame = true;
    }

    public void StartMicro()
    {
            
            if (deviceMicro == null) deviceMicro = Microphone.devices[0];
            clipRecord = Microphone.Start(deviceMicro, true, 999, 44100);
       

    }

    public void StartCheckMicro()
    {
       
        if (Connected)
        {

           
          
                      

        }

    }


    public float CheckLevelMicro()
    {
          
        float levelMax = 0;
        float[] waveData = new float[SampleWindow];
        int micPosition = Microphone.GetPosition(null) - (SampleWindow + 1); // null means the first microphone
        if (micPosition < 0) return 0;
        clipRecord.GetData(waveData, micPosition);

        // Getting a peak on the last 128 samples
        for (int i = 0; i < SampleWindow; i++)
        {
            float wavePeak = waveData[i] * waveData[i];
            if (levelMax < wavePeak)
            {
                levelMax = wavePeak;
            }
        }

              
        return levelMax * 100;
    }




    public bool CheckMicro()
    {

        if (DetectHeadset.Detect())
        {

            if (!Connected)
            {
                foreach (var device in Microphone.devices)
                {
                    deviceMicro = device;
                    print(device);
                    Connected = true;
                    StartMicro();
              
                }
            }
          
            return true;
        }
        else {
            Connected = false;
            return false;
        } 
        
      
    }
}
