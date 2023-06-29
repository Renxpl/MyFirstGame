using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Processors;
//using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class HitPoints : MonoBehaviour, IDataPersistence
{
    public HealtPointSystem hp;
    [SerializeField] GameObject hitboxObject;
    PlayerMovement pmScript;
    [SerializeField] Collider2D playerColliderToBeAttacked;
    Animator playerAnimator;
    [SerializeField]Collider2D enemyColliderr;
    Collider2D enemyCollider;
    int dmgCounter;
    bool isCoroutineStarted;
    public int playerHealth;
    bool isDead;
    
    // Start is called before the first frame update
    void Start()
    {
        dmgCounter= 0;
        hitboxObject.SetActive(false);
        pmScript = GetComponent<PlayerMovement>();
        playerAnimator = GetComponent<Animator>();
        playerHealth= hp.health;
    }

    // Update is called once per frame
    void Update()
    {

        if (pmScript.isAttackingAn == true)
        {
            hitboxObject.SetActive(true);
        }
        else { hitboxObject.SetActive(false); }

        if(hp.health <= 0 && !isDead)
        {
            playerAnimator.gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            
            playerAnimator.SetBool("isDead", true);

            FindObjectOfType<AudioManagerScript>().Play("DeathSound", false);

            isDead = true;
        }
        
        if(isCoroutineStarted && dmgCounter == 0 && hp.health > 0) 
        {
            StartCoroutine(GetDamage());
        }
        



    }
    
   

    void OnTriggerEnter2D(Collider2D collision)
    {
        
        enemyCollider = collision;
        


        if (enemyCollider.CompareTag("EnemyHitbox") && collision is CapsuleCollider2D && hp.health > 0) 
        {

            if (!isCoroutineStarted)
            {
                isCoroutineStarted = true;
                
            }
        }

    }

    void OnTriggerExit2D(Collider2D collision)
    {
        isCoroutineStarted = false;

    }
    

    IEnumerator GetDamage()
    {
         
        hp.health--; 
        playerHealth = hp.health;   
        dmgCounter++;
        yield return new WaitForSeconds(1.25f);
        dmgCounter = 0;
        
    }

    public void LoadData(GameData data)
    {
        hp.health= data.playerHealth;
    }
    
    public void SaveData(ref GameData data)
    {
        data.playerHealth= hp.health;
    }


}
