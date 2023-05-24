using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : MonoBehaviour
{
    public float health;
    UnitManager uManager;
    public float speed;
    public Collider[] playerDetect;
    public LayerMask playerMask;
    public float detectionRadius = 1f;
    public Animator anim;
    public float AttackSpeedDelay;


    float timer = 0;
    Vector3 endPos;

    // Start is called before the first frame update
    void Start()
    {
        timer = AttackSpeedDelay;
        anim = GetComponent<Animator>();
        uManager = FindObjectOfType<UnitManager>();
        endPos = uManager.spawnPoint.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }

        RaycastHit hit;
        Vector3 starPos1 = new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z);
        Debug.DrawRay(starPos1, transform.forward, Color.yellow);
        Vector3 starPos2 = new Vector3(transform.position.x + 0.3f, transform.position.y + 0.3f, transform.position.z);
        Debug.DrawRay(starPos2, transform.forward, Color.yellow);
        Vector3 starPos3 = new Vector3(transform.position.x - 0.3f, transform.position.y + 0.3f, transform.position.z);
        Debug.DrawRay(starPos3, transform.forward, Color.yellow);
        if (Physics.Raycast(starPos1, transform.forward, out hit, detectionRadius, playerMask)
            || Physics.Raycast(starPos2, transform.forward, out hit, detectionRadius, playerMask)
            || Physics.Raycast(starPos3, transform.forward, out hit, detectionRadius, playerMask))
        {
            Debug.Log("Enemy Collided");
            if (hit.collider.gameObject.GetComponent<Unit>() != null)
            {
                if (timer >= AttackSpeedDelay)
                {
                    hit.collider.gameObject.GetComponent<Unit>().ReceiveDamage(10);
                    timer = 0;
                }
                timer += Time.deltaTime;
            }
        }
        else
        {
            Debug.Log("Moving Enemy");
            transform.position = Vector3.MoveTowards(transform.position, endPos, speed * Time.deltaTime);
            //transform.position = Vector3.Slerp(transform.position, uManager.spawnPoint.position, speed * Time.deltaTime);
        }
    }

    private void LateUpdate()
    {
        transform.LookAt(uManager.spawnPoint.position);
    }
    public void RecieveDamage(int damage)
    {
        health -= damage;
        anim.Play("Hit");
    }
}
