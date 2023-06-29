using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class HittingByPlayer : MonoBehaviour
{

    [SerializeField] float secondsToBeDestroyed;
    [SerializeField] float toPush = 100f;
    private List<GameObject> processedEnemies = new List<GameObject>();
    bool isCoroutineStarted = false;
    Collider2D enemyCollider;
    Transform playerTransform;
    float direction;
  
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GetComponentInParent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
        direction = Mathf.Sign(playerTransform.localScale.x);
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && collision is CircleCollider2D )
        {
            
            processedEnemies.Add(collision.gameObject);
            enemyCollider = collision;
            StartCoroutine(GivingDmg(collision));
        }



    }
    
    IEnumerator GivingDmg(Collider2D collision)
    {

        collision.gameObject.GetComponent<SpriteRenderer>().color = new Color32(255, 100, 100, 255);
        if (collision.gameObject.GetComponent<EnemyMovement>() != null && !collision.gameObject.GetComponent<EnemyMovement>().isPushed)
        {
            collision.gameObject.GetComponent<EnemyMovement>().isPushed = true;
        }
        yield return new WaitForSeconds(0.2f);
       
        if (collision.gameObject.GetComponent<EnemyMovement>() != null)
        {
            collision.gameObject.GetComponent<EnemyMovement>().isAlive = false;
            collision.gameObject.GetComponent<EnemyMovement>().isPushed = false;
        }
        collision.gameObject.SetActive(false);

        processedEnemies.Remove(collision.gameObject);
    }

}
