using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAnimPlayer : MonoBehaviour
{
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        anim.Play(Random.Range(1, 13).ToString());
    }

    // Update is called once per frame
    void Update()
    {

    }
}
