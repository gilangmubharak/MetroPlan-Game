using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager uiManager;
    public GameObject house_small;
    public GameObject house_big;

    public GameObject deleteButton;
    public bool showDeleteButton;

    public GameObject placeButton;
    public GameObject cancelPlacementButton;

    public GameObject BuildingInfoPanel;
    public TextMeshProUGUI buildingNameText;
    public TextMeshProUGUI buildingAttributesText;
    public Image buildingImage;


    public TextMeshProUGUI eletricProductionText;
    public TextMeshProUGUI electricityConsumptionText;
    public TextMeshProUGUI taxIncomeText;
    public TextMeshProUGUI freeMoneyText;
    public TextMeshProUGUI turnNumberText;
    public TextMeshProUGUI populationText;

    public GameObject buySolarPanelsButton;
    public TextMeshProUGUI buySolarPanelsButtonText;

    //public GameObject levelDescription;
    public bool showLevelDescription;


    public GameObject fixBuildingButton;
    public TextMeshProUGUI fixBuildingButtonText;


    

    public GameObject levelCompletedPanel;
    public GameObject gameOverPanel;
    
    public GameObject levelInfoPanel;
    public TextMeshProUGUI levelDescriptionText;
    public TextMeshProUGUI levelName;
    public GameObject retryPanel;


    public GameObject backToMenuButton;
    public GameObject confirmBackToMenuPanel;

    //public AudioController audioController;

  


    void Awake()
    {
        uiManager = this;
    }

    // Start is called before the first frame update
    void Start()
    {        
        if (showLevelDescription)
        {
        //    levelDescriptionText.SetText(LevelManager.Instance.GetLevelDescription());
        }

        //levelInfoPanel= GameObject.Find("Level info panel");
        if (levelInfoPanel != null)
        {
            levelInfoPanel.SetActive(true);
            levelDescriptionText.SetText(LevelManager.Instance.GetLevelDescription());
            levelName.SetText(LevelManager.Instance.GetLevelName() + " TARGET");
        }
        else
            Debug.LogWarning("Level info panel not configured");

        
    }
    // Update is called once per frame
    void Update()
    {
        if(LevelRestrictor.levelRestrictor.canRemoveBuildings == false){
            showDeleteButton = false;
        }
        
        deleteButton.SetActive(showDeleteButton);

        if(showDeleteButton){


            if(BuildingsManager.buildingManager.currentBuilding.isDestroyed){
                if(ResourcesManager.resourcesManager.freeMoney >= BuildingsManager.buildingManager.currentBuilding.priceForRemovingDestoyedBuilding){
                    deleteButton.GetComponent<Button>().interactable = true;
                }else{
                    deleteButton.GetComponent<Button>().interactable = false;
                }
            }else{
                if(ResourcesManager.resourcesManager.freeMoney >= BuildingsManager.buildingManager.currentBuilding.priceForRemovingFunctionBuilding){
                    deleteButton.GetComponent<Button>().interactable = true;
                }else{
                    deleteButton.GetComponent<Button>().interactable = false;
                }
            }
        }



        placeButton.SetActive(ShowPlaceButton());
        cancelPlacementButton.SetActive(ShowPlaceButton());
        SetBuildingInfoPanel();
        SetFixBuildingButton();
        SetBuySolarPanelsButton();

        eletricProductionText.SetText("Eletric production: "+ResourcesManager.resourcesManager.GetEletricProduction().ToString()+" kw ");
        electricityConsumptionText.SetText("Eletric consumption: "+ResourcesManager.resourcesManager.GetEletricConsumption().ToString()+" kw ");

        freeMoneyText.SetText("Money: "+ResourcesManager.resourcesManager.freeMoney.ToString()+"$");
        taxIncomeText.SetText("Tax income: "+ResourcesManager.resourcesManager.GetTaxIncome().ToString()+"$/turn ");
        turnNumberText.SetText("Turn: "+TurnManager.turnManager.currentTurnNumber.ToString());
        populationText.SetText("Population: "+ResourcesManager.resourcesManager.GetTotalPopulation().ToString());

    }


    public void SetBuildingToBuild(GameObject building){
        GridBuilding.gridBuilding.buildingToBuild = building;
        GridBuilding.gridBuilding.TryCreateBuildingPrototype();
    }


    public void DeleteButtonPressed(){
        BuildingsManager.buildingManager.currentBuilding.DestroyBuilding();
        showDeleteButton = false;
    }

    public void PlaceButtonPressed(){
        GridBuilding.gridBuilding.TryToPlaceBuilding();
    }

    public void CancelPlacementButtonPressed(){
        GridBuilding.gridBuilding.CancelPlacement();
        AudioController.audioController.CancelButtonPlay();
    }

    public bool ShowPlaceButton(){
        if(GridBuilding.gridBuilding.buildingToBuildInstance != null){
            return true;
        }else{
            return false;
        }
    }

    public void SetBuildingInfoPanel(){
        Building building = null;
        
        if(GridBuilding.gridBuilding.buildingToBuildInstance != null){
            building = GridBuilding.gridBuilding.buildingToBuildInstance;
        }else{
            if(BuildingsManager.buildingManager.currentBuilding != null){
                building = BuildingsManager.buildingManager.currentBuilding;
            }else{
                BuildingInfoPanel.SetActive(false);
            }
        }

        if(building != null){
            buildingNameText.SetText(building.buildingName);
            buildingImage.sprite = building.spriteRenderer.sprite;


            buildingAttributesText.SetText("");
            
            if(building.constructionFinished == false && building.placed){
                buildingAttributesText.SetText("Building is in construction.\n\n");
                buildingAttributesText.SetText(buildingAttributesText.text+"Remaining turns to build: "+building.remainingTurnsToFinishConstruction+"\n\n");
                BuildingInfoPanel.SetActive(true);
                return;
            }

            if(building.isDestroyed){
                buildingAttributesText.SetText("Building was destroyed because it was not maintained.\n\n");
                buildingAttributesText.SetText(buildingAttributesText.text+"Price to remove rubbish: "+building.priceForRemovingDestoyedBuilding+"$\n\n");
                BuildingInfoPanel.SetActive(true);
                return;
            }else{
                buildingAttributesText.SetText(buildingAttributesText.text+"Building HP: "+building.currentHP+"/"+building.maxHP+"\n\n");
                buildingAttributesText.SetText(buildingAttributesText.text+"Price to remove building: "+building.priceForRemovingFunctionBuilding+"$\n\n");
            }


            if(building.placed == false){
                buildingAttributesText.SetText("Price: "+building.price+"$\n\n");
                buildingAttributesText.SetText(buildingAttributesText.text+"Turns to build: "+building.remainingTurnsToFinishConstruction+"\n\n");
            }

            if(building.hasEletricity == false && building.placed){
                buildingAttributesText.SetText(buildingAttributesText.text+"HAS NO ELETRICITY!\n\n");
            }

            buildingAttributesText.SetText(buildingAttributesText.text+"Population: "+building.population+"\n\n");
            buildingAttributesText.SetText(buildingAttributesText.text+"Tax income: "+building.taxIncome+"\n\n");

            if(building.hasSolarPanels){
                buildingAttributesText.SetText(buildingAttributesText.text+"Solar panel production: "+building.solarPanelEletricityProduction+" kw\n\n");
            }

            buildingAttributesText.SetText(buildingAttributesText.text+"Eletricity production: "+building.electricityProduction+"\n\n");
            buildingAttributesText.SetText(buildingAttributesText.text+"Eletricity consumption: "+building.electricityConsumption+"\n\n");





            BuildingInfoPanel.SetActive(true);
        }
    }

    public void NextTurnButtonPressed(){
        TurnManager.turnManager.NextTurn();
        LevelManager.Instance.UpdateKeys();


        AudioController.audioController.NextTurnPlay();

        // if (LevelManager.Instance.IsLevelFinished())
        // {
        //     if (!LevelManager.Instance.IsLastLevel())
        //         levelCompletedPanel.SetActive(true);
        //     else
        //         gameOverPanel.SetActive(true);
        // }
        // else
        // {
        //     if (LevelManager.Instance.IsLevelFailed())
        //         retryPanel.SetActive(true);
                
        // }

        if (LevelManager.Instance.IsLevelFinished())
        {
            if (!LevelManager.Instance.IsLastLevel())
            {     
               levelCompletedPanel.SetActive(true);
                AudioController.audioController.LevelBackgroundStop();
                AudioController.audioController.NextLevelPlay();
            }
            else
                gameOverPanel.SetActive(true);
        }
        else
        {
            if (LevelManager.Instance.IsLevelFailed())
            { 
                retryPanel.SetActive(true);
                AudioController.audioController.LevelBackgroundStop();
                AudioController.audioController.LevelFailedPlay();
            }

        }


    }

    public void BuySolarPanelsButtonPressed(){
        Building building = null;
        
        if(BuildingsManager.buildingManager.currentBuilding != null){
            building = BuildingsManager.buildingManager.currentBuilding;
        }else{
            return;
        }

        building.BuildSolarPanels();
    }

    public void SetBuySolarPanelsButton(){
        Building building = null;


        if (BuildingsManager.buildingManager.currentBuilding != null)
        {
            building = BuildingsManager.buildingManager.currentBuilding;
        }
        else
        {
            buySolarPanelsButton.SetActive(false);
            return;
        }

        if(BuildingsManager.buildingManager.currentBuilding.isDestroyed || !LevelRestrictor.levelRestrictor.canBuiltSolarPanels){
            buySolarPanelsButton.SetActive(false);
            return;
        }

        if(building.currentHP != building.maxHP){
            buySolarPanelsButton.SetActive(false);
            return;
        }

        if (building.hasSolarPanels || !building.canHaveSolarPanels || !building.constructionFinished)
        {
            buySolarPanelsButton.SetActive(false);
            return;
        }


        buySolarPanelsButtonText.SetText("Buy solar panels (" + building.solarPanelPrice.ToString() + "$)");
        buySolarPanelsButton.SetActive(true);

        if (ResourcesManager.resourcesManager.freeMoney >= building.solarPanelPrice)
        {
            buySolarPanelsButton.GetComponent<Button>().interactable = true;
        }
        else
        {
            buySolarPanelsButton.GetComponent<Button>().interactable = false;
        }

    }
    public void NextLevelPressed()
    {
        LevelManager.Instance.NextLevel();
    }

    public void ExitButtonPressed()
    {
        Debug.Log("ExitButtonPressed");
        LevelManager.Instance.ExitGame();

    }
    public void RetryPressed()
    {
        LevelManager.Instance.ReloadLevel();
    }


    public void SetFixBuildingButton(){
    
        if(BuildingsManager.buildingManager.currentBuilding == null){
            fixBuildingButton.SetActive(false);
            return;
        }

        if(BuildingsManager.buildingManager.currentBuilding.isDestroyed || !LevelRestrictor.levelRestrictor.canFixBuildings){
            fixBuildingButton.SetActive(false);
            return;
        }

        // Check if building is damaged
        if(BuildingsManager.buildingManager.currentBuilding.currentHP < BuildingsManager.buildingManager.currentBuilding.maxHP){
            
            fixBuildingButtonText.SetText("Fix building ("+BuildingsManager.buildingManager.currentBuilding.CalculatePriceToFix()+"$)");
            fixBuildingButton.SetActive(true);

            if(ResourcesManager.resourcesManager.freeMoney >= BuildingsManager.buildingManager.currentBuilding.CalculatePriceToFix()){
                fixBuildingButton.GetComponent<Button>().interactable = true;
            }else{
                fixBuildingButton.GetComponent<Button>().interactable = false;
            }
        }else{
            fixBuildingButton.SetActive(false);
        }
    }

    public void fixBuildingButtonPressed(){
        if(BuildingsManager.buildingManager.currentBuilding){
            BuildingsManager.buildingManager.currentBuilding.FixBuilding();
            AudioController.audioController.FixBuildingPlay();
        }
    }


    public void GoBackToMenuButtonPressed(){
        confirmBackToMenuPanel.SetActive(true);
        backToMenuButton.SetActive(false);
    }

    public void ConfirmBackToMenuButtonPressed(){
        SceneManager.LoadScene("Main");
    }

    public void CancelBackToMenuButtonPressed(){
        confirmBackToMenuPanel.SetActive(false);
        backToMenuButton.SetActive(true);
    }


    public bool MouseOverUI(){
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
