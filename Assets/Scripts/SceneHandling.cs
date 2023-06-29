using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandling : MonoBehaviour
{
    public GameObject finishScreen;
    Animator doorAnimator;
    bool isCoroutineStarted = false;
    int sceneNumberCameFromFile;

    void Start()
    {
        doorAnimator = GetComponent<Animator>();
    }


    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if(otherCollider.gameObject.CompareTag("Player") && !isCoroutineStarted)
        {
            doorAnimator.Play("Door");
            if (finishScreen != null && SceneManager.GetActiveScene().buildIndex == 5)
            {
                finishScreen.SetActive(true);
            }
            else
            {
                otherCollider.gameObject.transform.position = Vector3.zero;
                FindObjectOfType<DataPersistenceManager>().SaveGame();

                StartCoroutine(SceneTransitionForDoor());
            }
        }
        
    }

    public void NewGame()
    {
        FindObjectOfType<MainMenuHandler>().newGameScreen.SetActive(true);
        transform.parent.gameObject.SetActive(false);
        
    }
    public void LoadGame()
    {
        FindObjectOfType<MainMenuHandler>().loadGameScreen.SetActive(true);
        transform.parent.gameObject.SetActive(false);
    }

    public void Options()
    {
        FindObjectOfType<MainMenuHandler>().options.SetActive(true);
        transform.parent.gameObject.SetActive(false);
    }
    public void Back()
    {
        FindObjectOfType<MainMenuHandler>().mainMenu.SetActive(true);
        transform.parent.gameObject.SetActive(false);
    }
    public void NewGameFile1()
    {
        FindObjectOfType<DataPersistenceManager>().fileNamee = "SaveFile1";
        StartCoroutine(SceneTransition());
        FindObjectOfType<AudioManagerScript>().Stop("MenuMusic");
    }
    public void NewGameFile2()
    {
        FindObjectOfType<DataPersistenceManager>().fileNamee = "SaveFile2";
        StartCoroutine(SceneTransition());
        FindObjectOfType<AudioManagerScript>().Stop("MenuMusic");
    }
    public void NewGameFile3()
    {

        FindObjectOfType<DataPersistenceManager>().fileNamee = "SaveFile3";
        StartCoroutine(SceneTransition());
        FindObjectOfType<AudioManagerScript>().Stop("MenuMusic");
    }
    public void LoadGameFile1()
    {
        StartCoroutine(SceneTransition());
        FindObjectOfType<AudioManagerScript>().Stop("MenuMusic");
    }
    public void LoadGameFile2()
    {
        StartCoroutine(SceneTransition());
        FindObjectOfType<AudioManagerScript>().Stop("MenuMusic");
    }
    public void LoadGameFile3()
    {
        StartCoroutine(SceneTransition());
        FindObjectOfType<AudioManagerScript>().Stop("MenuMusic");
    }
    public void Quit()
    {
        FindObjectOfType<AudioManagerScript>().Play("MenuSelect", false);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    public void GetBackToMenu()
    {


        if (FindObjectOfType<DataPersistenceManager>() != null) { FindObjectOfType<DataPersistenceManager>().SaveGame(); }
        
        SceneManager.LoadScene(0);
    }
    public void GetBackToMenuD()
    {


       

        SceneManager.LoadScene(0);
    }
    public void Save()
    {


        if (FindObjectOfType<DataPersistenceManager>() != null) { FindObjectOfType<DataPersistenceManager>().SaveGame(); }
        
    }
    public void Load()
    {

        
        if (FindObjectOfType<DataPersistenceManager>() != null) { FindObjectOfType<DataPersistenceManager>().LoadGame(); }
        

    }

    IEnumerator SceneTransition()
    {
        int scene = FindObjectOfType<DataPersistenceManager>().SceneHandlingCall();

        yield return new WaitForSeconds(0.25f);
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            FindObjectOfType<AudioManagerScript>().Stop("MenuMusic");
            FindObjectOfType<AudioManagerScript>().Play("MainMusic", true);
        }
      
        SceneManager.LoadScene(scene);
        
        
    }
    IEnumerator SceneTransitionForDoor()
    {

        isCoroutineStarted = true;
        yield return new WaitForSeconds(0.25f);
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            FindObjectOfType<AudioManagerScript>().Stop("MenuMusic");
            FindObjectOfType<AudioManagerScript>().Play("MainMusic", true);
        }
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        isCoroutineStarted = false;
    }


    public void DeleteSaves()
    {
        string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
        string customFolderPath = Path.Combine(documentsPath, "MyFirstGame's Saves");
        string path = customFolderPath;

        if (Directory.Exists(path))
        {
            var directory = new DirectoryInfo(path);

            foreach (var file in directory.GetFiles())
            {
                file.Delete();
            }
        }
    }

}
