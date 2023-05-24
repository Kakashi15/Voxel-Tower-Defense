using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UnitHolder : MonoBehaviour
{
    public GameObject unit;
    public int amount;
    UnitManager uManager;
    TextMeshProUGUI textValue;

    void Start()
    {
        textValue = GetComponentInChildren<TextMeshProUGUI>();
        uManager = FindObjectOfType<UnitManager>();
    }

    // Update is called once per frame
    void Update()
    {
        textValue.SetText(amount.ToString());
    }

    public void SpawnUnit()
    {
        // Called by UI button click to spawn the unit at the designated spawn point

        GameObject unitInstance = Instantiate(unit, uManager.spawnPoint.position, Quaternion.identity);
        unitInstance.GetComponent<Unit>().StartPlacing();
    }
}
