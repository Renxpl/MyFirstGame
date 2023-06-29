using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GameData 
{

    public int playerHealth;

    public Vector3 playerPosition;

    public SeriliazableDictionary<string, Vector3, bool> dataForEnemies;

    public int sceneNumber;

    public GameData() 
    {
        
        this.playerHealth = 12;
        playerPosition = Vector3.zero;
        dataForEnemies = new SeriliazableDictionary<string, Vector3, bool>();
        sceneNumber = 1;

    }

    public GameData(int scene, int health)
    {

        this.playerHealth = health;
        playerPosition = Vector3.zero;
        dataForEnemies = new SeriliazableDictionary<string, Vector3, bool>();
        sceneNumber = scene;

    }


}
