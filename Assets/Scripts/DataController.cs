using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataController : MonoBehaviour
{

  
    public static DataController instance;

    public static Data playerData;

    private string _namePrefab = "data";

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        playerData = LoadData();

        SaveData(playerData);

       

    }


    private void OnDestroy()
    {

        SaveData(playerData);
       // PlayerPrefs.DeleteAll();
    }

 
    public void SaveData(Data data)
    {
       

        string json = JsonUtility.ToJson(data);

        PlayerPrefs.SetString(_namePrefab, json);

    }


    public Data LoadData()
    {


        if (!PlayerPrefs.HasKey(_namePrefab) || (PlayerPrefs.GetString(_namePrefab).Length < 2)) {
            return new Data();
        } 


        print("Chegouuuu");
        print("Mostra string " +PlayerPrefs.GetString(_namePrefab));

        Data data = JsonUtility.FromJson<Data>(PlayerPrefs.GetString(_namePrefab));

        print("Mostra data " + data.ToString());
        return data;
    }


    public void ResetData()
    {
       
        PlayerPrefs.DeleteAll();

    }

}
