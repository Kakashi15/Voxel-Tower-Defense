using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    UnitManager uManager;
    public float speed;
    public Collider[] enemiesDetect;
    public bool attackState;
    public float detectionRadius = 1f;
    public float health;
    public Animator anim;
    public float AttackSpeedDelay;
    float timer = 0;


    public Vector3 endPos;

    public bool isPlaced = false;
    public bool isPlacing = false;
    public GameObject ground;
    public Camera mainCamera;
    public LayerMask groundLayer;

    void Start()
    {
        timer = AttackSpeedDelay;
        attackState = false;
        uManager = FindObjectOfType<UnitManager>();
        anim = GetComponent<Animator>();
        endPos = uManager.endPosTransform.position;
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, groundLayer))
        {
            endPos = new Vector3(hit.point.x, transform.localScale.y / 2, hit.point.z);
            //transform.position.y = transform.localScale.y / 2;
        }

        /*if (isPlacing)
        {
            
        }

        if (Input.GetMouseButtonDown(0))
        {
            //endPos.z = transform.position.z;
            StopPlacing();
        }

        RaycastHit hit2;
        //Debug.DrawRay(transform.position, transform.forward, Color.green);

        Vector3 starPos1 = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
        Debug.DrawRay(starPos1, transform.forward, Color.yellow);
        Vector3 starPos2 = new Vector3(transform.position.x + 0.3f, transform.position.y + 0.1f, transform.position.z);
        Debug.DrawRay(starPos2, transform.forward, Color.yellow);
        Vector3 starPos3 = new Vector3(transform.position.x - 0.3f, transform.position.y + 0.1f, transform.position.z);
        Debug.DrawRay(starPos3, transform.forward, Color.yellow);

        Vector3 starPos = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
        if (Physics.Raycast(starPos1, transform.forward, out hit2, detectionRadius, uManager.enemyMask)
            || Physics.Raycast(starPos2, transform.forward, out hit2, detectionRadius, uManager.enemyMask)
            || Physics.Raycast(starPos3, transform.forward, out hit2, detectionRadius, uManager.enemyMask))
        {
            if (hit2.collider.gameObject.GetComponent<EnemyUnit>() != null)
            {
                if (timer >= AttackSpeedDelay)
                {
                    hit2.collider.gameObject.GetComponent<EnemyUnit>().RecieveDamage(50);
                    timer = 0;
                }
                timer += Time.deltaTime;
            }
        }
        else
        {
            Debug.LogWarning("Moving");
            transform.position = Vector3.MoveTowards(transform.position, endPos, speed * Time.deltaTime);
            //transform.position = Vector3.Slerp(transform.position, uManager.endPathPosition.position, speed * Time.deltaTime);
        }*/

    }

    private void LateUpdate()
    {
        transform.LookAt(endPos);
    }

    public void ReceiveDamage(int damage)
    {
        health -= damage;
        anim.Play("Hit");
    }

    public void StartPlacing()
    {
        isPlacing = true;
    }

    public void StopPlacing()
    {
        isPlacing = false;
    }
}
