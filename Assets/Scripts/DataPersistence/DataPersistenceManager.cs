using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using System.IO;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]

    public string fileName;
    public string fileNamee;
    [SerializeField] private bool useEncryption;

    public GameData gameData;

    private List<IDataPersistence> dataPersistenceObjects;

    private FileDataHandler dataHandler;
    

    
    public static DataPersistenceManager instance { get; private set; }




    void Awake()
    {
        
      if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
      else if (instance != this)
            {
                Debug.Log("Instance already exists, destroying object!");
                Destroy(this);
            }  
            
        

    }
    
    void Start()
    {
       
        

    }

    public void SceneStart()
    {
        if(SceneManager.GetActiveScene().buildIndex != 0)
        {
            
            if (dataPersistenceObjects != null) { dataPersistenceObjects.Clear(); }
            
            dataPersistenceObjects = FindAllDataPersistenceObjects();
            
            LoadGame();


            
        }
      
    }
    public int SceneHandlingCall()
    {
        string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
        string customFolderPath = Path.Combine(documentsPath, "MyFirstGame's Saves");
        this.dataHandler = new FileDataHandler(customFolderPath, fileNamee, useEncryption);
        if (dataHandler.Load() != null)
        {
            return dataHandler.Load().sceneNumber;
        }
        return 1;
        

    }


    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void SaveGame()
    {
        foreach(IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref gameData);
        }
        Debug.Log("Your Hp " + gameData.playerHealth.ToString());

        dataHandler.Save(gameData);
    }


    public void LoadGame()
    {
        this.gameData = dataHandler.Load();

        if(this.gameData == null)
        {
            Debug.Log("There is no valid GameData file, starting game over");
            NewGame();
        }

        foreach(IDataPersistence dataPersitenceObj in dataPersistenceObjects)
        {
            dataPersitenceObj.LoadData(gameData);
        }

        

        Debug.Log("Your Hp " + gameData.playerHealth.ToString());

    }

    void OnApplicationQuit()
    {
        if(dataPersistenceObjects != null) { SaveGame(); }
        
    }

    List<IDataPersistence> FindAllDataPersistenceObjects()
    {

        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);

    }

}
