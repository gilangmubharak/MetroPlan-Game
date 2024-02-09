using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelRestrictor : MonoBehaviour
{


    public static LevelRestrictor levelRestrictor;

    public bool canBuiltSolarPanels = true;
    public bool canFixBuildings = true;
    public bool canRemoveBuildings = true;
    public List<GameObject> prohibitedBuildings;


    void Awake()
    {
        levelRestrictor = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public bool CanBuildBuilding(GameObject building){
        for(int i = 0; i < prohibitedBuildings.Count; i++){
            if(prohibitedBuildings[i].GetComponent<Building>().buildingName == building.GetComponent<Building>().buildingName){
                return false;
            }
        }
        return true;
    }
}
