using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitController : Selectable
{
    NavMeshAgent navMeshAgent;
    public LayerMask groundLayer;
    public Vector3 TargetPosition;
    public float detectionRadius = 10f;
    public bool collectionReset = false;
    ResourceManager resourceManager;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        resourceManager = FindObjectOfType<ResourceManager>();
        anim = GetComponent<Animator>();
        navMeshAgent = GetComponentInChildren<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 origin = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        Ray ray2 = new Ray(origin, transform.forward);
        RaycastHit hit2;
        Debug.DrawLine(origin, new Vector3(origin.x, origin.y, origin.z + detectionRadius), Color.blue);
        if (Physics.Raycast(ray2, out hit2, detectionRadius))
        {
            if (hit2.collider.gameObject.tag.Equals("Human") && navMeshAgent.remainingDistance < detectionRadius)
            {
                navMeshAgent.isStopped = true;
                anim.Play("Idle");
                if (!collectionReset)
                {
                    collectionReset = true;
                    StartCoroutine(Collector());
                    hit2.transform.GetComponent<HumanResourceHolder>().Collect();
                }
                Debug.DrawRay(origin, new Vector3(origin.x, origin.y, origin.z + detectionRadius), Color.red);
            }
        }

        if (Input.GetMouseButton(1) && IsSelected())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, groundLayer))
            {
                navMeshAgent.isStopped = false;
                collectionReset = false;
                StopCoroutine(Collector());
                TargetPosition = hit.point;
            }
            navMeshAgent.SetDestination(TargetPosition);
        }
        if (navMeshAgent.isStopped)
            anim.Play("Idle");

        if (!navMeshAgent.isStopped)
            anim.Play("Walk");

        navMeshAgent.updateRotation = true;
    }

    IEnumerator Collector()
    {
        while (true)
        {
            resourceManager.humanBlood++;
            yield return new WaitForSeconds(resourceManager.collectionSpeed);
        }
    }
}