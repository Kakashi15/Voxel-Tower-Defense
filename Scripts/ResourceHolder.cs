using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceHolder : MonoBehaviour
{
    public Vector3 posUpdate;
    public Vector3 rotUpdate;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.tag.Equals("Collector"))
    //    {
    //        other.gameObject.GetComponent<UnitController>().OnUpdatePosition(posUpdate);
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Collector"))
        {
            other.gameObject.GetComponent<UnitController>().OnUpdatePosition(gameObject, posUpdate, rotUpdate);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Collector"))
        {
            other.gameObject.GetComponent<UnitController>().harvesting = false;
        }
    }
}
