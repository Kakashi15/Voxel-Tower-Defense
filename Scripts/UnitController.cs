using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class UnitController : Selectable
{
    NavMeshAgent navMeshAgent;
    public LayerMask groundLayer;
    public Vector3 TargetPosition;
    public float detectionRadius = 10f;
    public bool collectionReset = false;
    public bool harvesting = false;
    ResourceManager resourceManager;
    Animator anim;
    ObjectDrag objectDrag;

    float clicked = 0;
    float clicktime = 0;
    float clickdelay = 0.5f;

    bool DoubleClick()
    {
        if (Input.GetMouseButtonDown(1))
        {
            clicked++;
            if (clicked == 1) clicktime = Time.time;
        }
        if (clicked > 1 && Time.time - clicktime < clickdelay)
        {
            clicked = 0;
            clicktime = 0;
            return true;
        }
        else if (clicked > 2 || Time.time - clicktime > 1) clicked = 0;
        return false;
    }


    // Start is called before the first frame update
    void Start()
    {
        resourceManager = FindObjectOfType<ResourceManager>();
        anim = GetComponent<Animator>();
        navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        objectDrag = GetComponent<ObjectDrag>();
    }

    // Update is called once per frame
    void Update()
    {
        if (objectDrag.isPlacing)
            return;

        Vector3 origin = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        Ray ray2 = new Ray(origin, transform.forward);
        RaycastHit hit2;
        Debug.DrawLine(origin, new Vector3(origin.x, origin.y, origin.z + detectionRadius), Color.blue);
        if (Physics.Raycast(ray2, out hit2, detectionRadius))
        {
            if (hit2.collider.gameObject.tag.Equals("Human") && navMeshAgent.remainingDistance < detectionRadius)
            {
                navMeshAgent.isStopped = true;
                harvesting = true;
                if (!collectionReset)
                {
                    collectionReset = true;
                    StartCoroutine(Collector());
                }
                Debug.DrawRay(origin, new Vector3(origin.x, origin.y, origin.z + detectionRadius), Color.red);
            }
        }
        //else
        //    harvesting = false;

        if (Input.GetMouseButton(1) && IsSelected() && !harvesting)
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

        if (DoubleClick())
        {
            harvesting = false;
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
        }
        if (navMeshAgent)
        {
            if (navMeshAgent.isStopped && !harvesting)
                anim.Play("Idle");

            if (!navMeshAgent.isStopped && !harvesting)
                anim.Play("Walk");

            navMeshAgent.updateRotation = true;
        }

    }

    public void OnUpdatePosition(GameObject host, Vector3 pos, Vector3 rotation)
    {
        anim.Play("Harvest");
        transform.LookAt(pos);
        navMeshAgent.isStopped = true;
        transform.position = host.transform.position + pos;
        transform.eulerAngles = rotation;
        harvesting = true;
    }
    IEnumerator Collector()
    {
        while (true)
        {
            resourceManager.humanBlood++;
            yield return new WaitForSeconds(resourceManager.collectionSpeed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9)
        {

        }
    }
}