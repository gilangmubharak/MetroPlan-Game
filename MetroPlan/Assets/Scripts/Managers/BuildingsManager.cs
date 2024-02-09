using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingsManager : MonoBehaviour
{
    public static BuildingsManager buildingManager;
    public Building currentBuilding;
    public List<Building> buildings;


    void Awake()
    {
        buildingManager = this;
    }


    void Start()
    {
        Debug.Log("BuildingManager Start");
        foreach(Building b in buildings)
        {
            b.InitialPlace();
        }
        Debug.LogFormat("BuildingManager Start: number of buildings: {0}", BuildingsManager.buildingManager.buildings.Count);

    }
    // Update is called once per frame
    void Update()
    {
        CheckEletricityAvailability();   
    }


    public void CheckEletricityAvailability(){
        int freeEletricity = ResourcesManager.resourcesManager.GetEletricProduction();

        for(int i = 0; i < BuildingsManager.buildingManager.buildings.Count; i++){

            if(BuildingsManager.buildingManager.buildings[i].constructionFinished == false){
                continue;
            }

            int consumtionOfCurBuilding;
            if(BuildingsManager.buildingManager.buildings[i].hasSolarPanels){
                consumtionOfCurBuilding = BuildingsManager.buildingManager.buildings[i].GetConsumptionWithSolarPanels();
            }else{
                consumtionOfCurBuilding = BuildingsManager.buildingManager.buildings[i].electricityConsumption;
            }

            if(consumtionOfCurBuilding < 0){
                consumtionOfCurBuilding = 0;
            }


            if(freeEletricity >= consumtionOfCurBuilding){
                freeEletricity -= consumtionOfCurBuilding;
                BuildingsManager.buildingManager.buildings[i].hasEletricity = true;
            }else{
                BuildingsManager.buildingManager.buildings[i].hasEletricity = false;
            }
        }
    }
}
