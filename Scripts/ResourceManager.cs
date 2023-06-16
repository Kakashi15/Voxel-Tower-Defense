using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public int humanBlood;
    public float collectionSpeed;
    public float routineCount;
    public float stopCount;
    List<Coroutine> resourceQueue;

    void Start()
    {
        resourceQueue = new List<Coroutine>();
    }

    // Update is called once per frame
    void Update()
    {

    }


}
