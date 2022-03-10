using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class GameController : MonoBehaviour
{

    public VideoPlayer VPlayer;

    public AudioClip AudioEffects;

    public VideoClip GameVideo;

    public GameState CurrentGameState;


    public double TimeStopBreathing;

    public double TimeStartFinish;

    public double TimeFinish;



    public Image ImgPopPup;

    public Sprite SptBlow;

    public Sprite SptConnected;

    public Sprite SptGoodJob;



    public TextMeshProUGUI TextTimer;

    public TextMeshProUGUI TextHightScore;

    private bool stopTimer;

 

    private float timerRound;


    private MicrophoneController microController;

    // Start is called before the first frame update

    private void Awake()
    {
        ManagerSoundController.instance.StartSound();
        ManagerSoundController.instance.StopAudio();
    }
    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

       

        microController = this.gameObject.GetComponent<MicrophoneController>();

        float tempHightScore = PlayerPrefs.GetFloat("score", 00);
        TextHightScore.text = tempHightScore.ToString("F2");

        ImgPopPup.gameObject.SetActive(true);
        ImgPopPup.enabled = false;

      
        VPlayer.clip = GameVideo;
        VPlayer.Play();
        ManagerSoundController.instance.PlayAudioEffects(AudioEffects);


        CurrentGameState = GameState.START;

       // ManagerSoundController.instance.Audio.mute = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        UiTimer();


       
        if (!microController.CheckMicro()) SceneManager.LoadScene("menu");

        if (CurrentGameState == GameState.START)
        {
            if (VPlayer.time >= TimeStopBreathing)
            {
               
                VPlayer.Pause();
                ManagerSoundController.instance.PauseAudio();

                if (!microController.Connected)
                {
                    ImgPopPup.sprite = SptConnected;
                    ImgPopPup.enabled = true;

                }
                else
                {
                    microController.StartCheckMicro();
                    CurrentGameState = GameState.WAITINPUT;
                    ImgPopPup.sprite = SptBlow;
                    ImgPopPup.enabled = true;
                    microController.MicroStartedGame = true;



                }

               

            }
        }
        else if (CurrentGameState == GameState.WAITINPUT)
        {


            if (microController.GameStart)
            {
                CurrentGameState = GameState.PLAY;
                VPlayer.Play();
                ManagerSoundController.instance.PlayAudio();
                ImgPopPup.enabled = false;

            }


            if (Input.GetKeyDown(KeyCode.W))
            {

                CurrentGameState = GameState.PLAY;
                VPlayer.Play();
                ManagerSoundController.instance.PlayAudio();
                ImgPopPup.enabled = false;

            }
        }
        else if (CurrentGameState == GameState.PLAY)
        {

            if (microController.GameFinish && !stopTimer)
            {
                VPlayer.time = TimeStartFinish;
                ManagerSoundController.instance.TimeAduio(TimeStartFinish);
                stopTimer = true;
            }

            //Start test Breathing
            if (Input.GetKeyUp(KeyCode.W))
            {

                VPlayer.time = TimeStartFinish;
                ManagerSoundController.instance.TimeAduio(TimeStartFinish);
                stopTimer = true;

            }


        

            if (VPlayer.time >= TimeFinish)
            {
                ImgPopPup.sprite = SptGoodJob;
                ImgPopPup.enabled = true;
                ImgPopPup.transform.gameObject.transform.GetChild(0).gameObject.SetActive(true);
         
                CurrentGameState = GameState.FINISH;
                VPlayer.Pause();
                ManagerSoundController.instance.StopAudio();
            }
        }
    }



    void UiTimer()
    {
        if (CurrentGameState == GameState.PLAY && !stopTimer)
        {
            timerRound += Time.deltaTime;
            TextTimer.text = timerRound.ToString("F2");

       

        }

        if (CurrentGameState == GameState.FINISH)
        {
          
            if (timerRound > PlayerPrefs.GetFloat("score"))
            {
                PlayerPrefs.SetFloat("score", timerRound);
               
              

            }

            StartCoroutine(StartMenuAfterDelay());

        }

    }

    IEnumerator StartMenuAfterDelay()
    {
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene("menu");
    }

    private void OnDestroy()
    {
        ManagerSoundController.instance.StopAudio();
    }

}
