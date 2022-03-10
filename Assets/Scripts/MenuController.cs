using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{


    public GameObject InfoGObject;

    public Button BtnSound;

    public Sprite SptSoundOn;

    public Sprite SptSoundOff;

    public GameObject ObjCalibrate;

    public TextMeshProUGUI TxtCalibrate;

    private CalibrateController calibrateController;

 
    private bool enterCalibrate;

    // Start is called before the first frame update
    void Start()
    {
        
        calibrateController = this.gameObject.GetComponent<CalibrateController>();

        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        ManagerSoundController.instance.StartSound();
        ManagerSoundController.instance.PlayMusic(true);


        if (!DataController.playerData.IsMute)
        {
            BtnSound.image.sprite = SptSoundOn;

        }
        else
        {
            BtnSound.image.sprite = SptSoundOff;

        }


       
       

     

        InfoGObject.SetActive(false);


        
        if (calibrateController.Connected && !DataController.playerData.Calibrated && !enterCalibrate)
        {
          
                StartCoroutine(Calibrate());

        }
        else if (!calibrateController.Connected)
        {

        }

    }

    // Update is called once per frame
    void Update()
    {
       

        if (calibrateController.Connected && !DataController.playerData.Calibrated && !enterCalibrate)
        {

            StartCoroutine(Calibrate());

        }
        else if (!calibrateController.Connected)
        {
            ConnectMicrophone(true);
           // ManagerSoundController.instance.PlayMusic(false);
            calibrateController.CheckMicro();
        }
        if(calibrateController.Connected && !enterCalibrate)
        {

            ConnectMicrophone(false);
            //ManagerSoundController.instance.PlayMusic(true);
        }

        calibrateController.CheckMicro();

    }



    void ConnectMicrophone(bool show)
    {
        if (show)
        {
            ObjCalibrate.SetActive(true);
            TxtCalibrate.text = "Connect the breathing device.";
        }
        else
        {
            ObjCalibrate.SetActive(false);
        }
    }


    IEnumerator Calibrate()
    {
        print("entrou");
        enterCalibrate = true;
        ObjCalibrate.SetActive(true);
        ManagerSoundController.instance.Audio.mute = true;
        TxtCalibrate.text = "Quiet to calibrate your device.";
        yield return new WaitForSeconds(2f);
        TxtCalibrate.text = "Quiet to calibrate your device in 3...";
        yield return new WaitForSeconds(1f);
        TxtCalibrate.text = "Quiet to calibrate your device in 2...";
        yield return new WaitForSeconds(1f);
        TxtCalibrate.text = "Quiet to calibrate your device in 1...";
        yield return new WaitForSeconds(1f);
        TxtCalibrate.text = "Keep Quiet Calibrating...";
        calibrateController.StartCalibrate();
        yield return new WaitForSeconds(3f);
        calibrateController.StopCalibrate();
        TxtCalibrate.text = "Ready";
        yield return new WaitForSeconds(1f);
        ObjCalibrate.SetActive(false);
        ManagerSoundController.instance.Audio.mute = DataController.playerData.IsMute;
        enterCalibrate = false;
        print(DataController.playerData.AvarageNosie);

      
    }

    public void ButtonInfo()
    {

        if(InfoGObject.activeSelf == false) InfoGObject.SetActive(true);
        else InfoGObject.SetActive(false);

    }
    public void ButtonSound()
    {
       

        SoundController();


    }

    public void StartCalibrate()
    {
        print("Start");
        if (calibrateController.Connected && !enterCalibrate)
        {
            DataController.playerData.Calibrated = false;
            StartCoroutine(Calibrate());

        }
    }
    void SoundController()
    {
        ManagerSoundController.instance.Muted();
        

        if (!DataController.playerData.IsMute)
        {
            BtnSound.image.sprite = SptSoundOn;
           
        }
        else
        {
            BtnSound.image.sprite = SptSoundOff;
          
        }

    }

    public void ButtonExit()
    {
        Application.Quit();
    }


    public void ButtonStartLevel(string level)
    {
        SceneManager.LoadScene(level);
    }
}
