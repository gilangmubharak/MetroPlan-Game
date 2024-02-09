using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesManager : MonoBehaviour
{
    public static ResourcesManager resourcesManager;
    public int freeMoney = 0;

    void Awake()
    {
        resourcesManager = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public int GetTaxIncome(){
        int income = 0;
        for(int i = 0; i < BuildingsManager.buildingManager.buildings.Count; i++){

            // If does not have eletricity, you will get no tax money
            if(BuildingsManager.buildingManager.buildings[i].hasEletricity){
                if(BuildingsManager.buildingManager.buildings[i].constructionFinished == false){
                    continue;
                }
                income += BuildingsManager.buildingManager.buildings[i].taxIncome;
            }
        }
        return income;
    }


    public int GetEletricProduction(){
        int production = 0;
        for(int i = 0; i < BuildingsManager.buildingManager.buildings.Count; i++){
            if(BuildingsManager.buildingManager.buildings[i].constructionFinished == false){
                continue;
            }
            production += BuildingsManager.buildingManager.buildings[i].electricityProduction;
        }
        return production;
    }

    public int GetEletricConsumption(){
        int consumption = 0;
        for(int i = 0; i < BuildingsManager.buildingManager.buildings.Count; i++){

            if(BuildingsManager.buildingManager.buildings[i].constructionFinished == false){
                continue;
            }
            
            if(BuildingsManager.buildingManager.buildings[i].hasSolarPanels){

                if(BuildingsManager.buildingManager.buildings[i].constructionFinished == false){
                    continue;
                }

                if(BuildingsManager.buildingManager.buildings[i].GetConsumptionWithSolarPanels() > 0){
                    consumption += BuildingsManager.buildingManager.buildings[i].GetConsumptionWithSolarPanels();
                }
            }else{
                consumption += BuildingsManager.buildingManager.buildings[i].electricityConsumption;
            }
        }
        return consumption;
    }

    public int GetTotalPopulation(){
        int population = 0;
        
        for(int i = 0; i < BuildingsManager.buildingManager.buildings.Count; i++){
            if(BuildingsManager.buildingManager.buildings[i].constructionFinished == false){
                continue;
            }
            population += BuildingsManager.buildingManager.buildings[i].population;
        }
        return population;
    }
}
