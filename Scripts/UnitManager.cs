using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UnitManager : MonoBehaviour
{
    public EnemyUnit[] units;
    public Transform spawnPoint;
    public Transform[] endPathPosition;
    public LayerMask enemyMask;
    public Transform endPosTransform;

    public float enemySpawnTime = 3;

    private void Start()
    {
        // Set the initial spawn point to (0, 0, 0) or any other desired position
        //StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(enemySpawnTime);
            Instantiate(units[Random.Range(0, units.Length)], endPathPosition[Random.Range(0, endPathPosition.Length)].position, Quaternion.identity);
        }
    }

}
