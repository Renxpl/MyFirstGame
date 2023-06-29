using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Trap : MonoBehaviour
{

    PlayerMovement playerScript;
    Rigidbody2D playerRb;
    Animator playerAnim;
    HealtPointSystem hp;
    bool isCoroutineStarted = false;
    bool StartMyCoroutine = false;
    // Start is called before the first frame update
    void Start()
    {
        playerScript = FindObjectOfType<PlayerMovement>();
        playerAnim = playerScript.gameObject.GetComponent<Animator>();
        playerRb = playerScript.gameObject.GetComponent<Rigidbody2D>();
        hp = Resources.Load<HealtPointSystem>("PlayerHp");  
    }

    // Update is called once per frame
    void Update()
    {
        if (!isCoroutineStarted && StartMyCoroutine)
        {
            StartCoroutine(GetDamage());
        }


    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            StartMyCoroutine= true;
           


        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            StartMyCoroutine = false;



        }
    }

    IEnumerator GetDamage()
    {
        isCoroutineStarted= true;
        hp.health -= 2;
        yield return new WaitForSeconds(1.25f);
        isCoroutineStarted= false;


    }




}
