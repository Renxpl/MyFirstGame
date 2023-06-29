using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using static UnityEngine.UI.Image;
using System.Linq;

public class EnemyMovement : MonoBehaviour, IDataPersistence
{
    
    // transform list, enum
    [SerializeField] List<Transform> waypoints;
    [SerializeField] Transform patrollingPointer;
    public LayerMask wallLayerMask;
    [SerializeField] float enemySpeed = 1f;
    [SerializeField]float timeToWait;
    RaycastHit2D hit;
    Coroutine waiting;
    public bool isPushed = false;
    GameObject player;
    Rigidbody2D enemyRb;
    Vector2 Moving;
    float direction;
    float distance;   
    float convertedSpeed;
    float distanceToTurning;
    int currentindex;
    bool isWaiting;
    bool isHit;
    public bool isAlive = true;
    bool stopSignal = false;
    enum State
    {
        Patrolling,
        Following,
        Stopping
    }

    State currentState;

    
    void Start()
    {
          
        enemyRb= GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        currentState = State.Patrolling;
        currentindex= 0;
        
        
        
    }
   

    private void Update()
    {

        hit = Physics2D.Raycast(transform.position, new Vector2(direction, 0) , 1f, wallLayerMask);
        Debug.DrawRay(transform.position, new Vector2(direction, 0) * 1f, Color.red);

        if (hit && !isHit ) { Debug.Log("Hit"); isHit = true;}
        if (!hit) { isHit = false; }

        convertedSpeed = enemySpeed * Time.deltaTime;
        distance = Vector2.Distance(player.transform.position, transform.position);
        
        switch (currentState)
        {

            case State.Patrolling:
                Patrol();
                if (distance < 5)
                {
                    currentState = State.Following;
                }

                else
                {
                    enemyRb.MovePosition(Moving);
                    SpriteChangesInAction();
                }

                    break;

            case State.Following:
                Follow();
                
                if (distance >= 5 )
                {
                    currentState = State.Stopping;                                 
                }

                else
                {
                    if (!isHit) { enemyRb.MovePosition(Moving); }
                    SpriteChangesInAction();
                }

                break;

            case State.Stopping:
                
                if (!isWaiting)
                {
                   waiting = StartCoroutine(WaitAfterFollow());
                    
                }
                if (distance < 5)
                {
                    if(waiting != null)  StopCoroutine(waiting);

                    isWaiting = false;
                    
                    currentState = State.Following;
                   
                }


                break;

        }

        //not necessary but for an extra safety code for not moving through walls

        if(isHit || distance < 1f)
        {
            enemyRb.position = transform.position;
            stopSignal= false;
        }
        if (isPushed)
        {

            enemyRb.AddForce(new Vector2((-direction) * 100f, 0f), ForceMode2D.Impulse);
            isPushed= false;

        }

    }
   


    void Follow()
    {
        
            Moving = Vector2.MoveTowards(transform.position, new Vector2(player.transform.position.x, transform.position.y), convertedSpeed);
        

    }


    void Patrol() 
    {
        
         distanceToTurning = Vector2.Distance(waypoints[currentindex].position, transform.position);
         Moving = Vector2.MoveTowards(transform.position, new Vector2 (waypoints[currentindex].position.x, transform.position.y), convertedSpeed);
         

         if(distanceToTurning< 0.7f)
         {

            if (waypoints.Count == 0) return;


             currentindex = (currentindex + 1) % waypoints.Count;

         }

    }




    void SpriteChangesInAction()
    {
        direction = Mathf.Sign(Moving.x - transform.position.x);

        if (direction == 1)
        {
            transform.localScale = new Vector2(-1, transform.localScale.y);
            
            

        }

        else if(direction == -1)
        {
            transform.localScale =new Vector2(1, transform.localScale.y);
            
        }
    }

    IEnumerator WaitAfterFollow()
    {
        isWaiting= true;
        enemyRb.velocity= Vector2.zero;
        yield return new WaitForSeconds(timeToWait);
        currentState = State.Patrolling;
        isWaiting = false;

    }



    public void LoadData(GameData data)
    {


        if (data.dataForEnemies.ContainsKey(name))
        {
            

            transform.position = data.dataForEnemies[name].Item1;
            isAlive = data.dataForEnemies[name].Item2;
            if (!isAlive)
            {
                gameObject.SetActive(false);
            }
            
        }



    }


    public void SaveData(ref GameData data)
    {
       if (data.dataForEnemies.ContainsKey(name))
        {
            data.dataForEnemies.Remove(name);
        }

        Tuple<Vector3, bool> tuple =  Tuple.Create(transform.position, isAlive);
        data.dataForEnemies.Add(name,tuple);

    }

   

}
