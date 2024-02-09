using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{

    public static TurnManager turnManager;
    public int currentTurnNumber = 0;

    void Awake()
    {
        turnManager = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextTurn()
    {
        currentTurnNumber++;

        // Reset building indicators
        if (GridBuilding.gridBuilding.buildingToBuildInstance != null)
        {
            GridBuilding.gridBuilding.CancelPlacement();
        }

        if (!LevelManager.Instance.IsLevelFinished())
        {
            Debug.Log("Level not completed");
            for (var i = 0; i < BuildingsManager.buildingManager.buildings.Count; i++)
            {
                if(BuildingsManager.buildingManager.buildings[i].constructionFinished == false){

                    Debug.Log("Building");
                    BuildingsManager.buildingManager.buildings[i].remainingTurnsToFinishConstruction--;

                    if(BuildingsManager.buildingManager.buildings[i].remainingTurnsToFinishConstruction <= 0){
                        BuildingsManager.buildingManager.buildings[i].FinishConstruction();
                    }

                    continue;
                }
                
                ResourcesManager.resourcesManager.freeMoney += BuildingsManager.buildingManager.buildings[i].taxIncome;
                Debug.LogFormat("Tax Collection: new budget {0}", ResourcesManager.resourcesManager.freeMoney);
            }
        }
        else
        {
            //show message to player of level completed and give option to move on to next level
            Debug.Log("Level completed");
        }
    }
}