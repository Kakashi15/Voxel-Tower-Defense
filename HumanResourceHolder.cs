using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanResourceHolder : MonoBehaviour
{
    public int amount;
    public List<GameObject> humans;
    ResourceManager resourceManager;
    bool isCollecting;
    void Start()
    {
        humans = new List<GameObject>();
        amount = transform.childCount;
        for (int i = 1; i < transform.childCount; i++)
        {
            humans.Add(transform.GetChild(i).gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Collect()
    {
        StartCoroutine(CollectUpdate());
    }


    public void StopCollect()
    {
        StopCoroutine(CollectUpdate());
    }

    IEnumerator CollectUpdate()
    {
        amount--;
        yield return new WaitForSeconds(resourceManager.collectionSpeed);
    }
}
