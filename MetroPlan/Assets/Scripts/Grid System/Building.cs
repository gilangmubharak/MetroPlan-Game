using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    // General
    public string buildingName;
    public int price;
    public int minimalPopulationToBuild;
    public int taxIncome;

    public SpriteRenderer spriteRenderer;
    public bool builtByPlayer = true; // If already placed on start of the level than false
    
    //KPIs for level target
    public int population;
    public int poverty;
    public int energy;
    public int clean;

    public float maxHP;
    public float currentHP;
    public bool isDestroyed;
    public GameObject fireParticles;

    public int priceForRemovingFunctionBuilding;    // Should be lower than bellow
    public int priceForRemovingDestoyedBuilding;    // Eg.: destroyed by fire

    public bool constructionFinished;
    public int remainingTurnsToFinishConstruction;
    public Sprite inConstructionSprite;
    private Sprite finishedSprite; 
    public Sprite destroyedBuildingSprite;

    public bool placed {get; private set; }
    public GameObject destroyParticles;
    public BoundsInt area;
    public bool hasClearCenter = true;

    // Solar panels
    public bool canHaveSolarPanels = true;
    public bool hasSolarPanels;
    public int solarPanelPrice;
    public int solarPanelEletricityProduction;
    public GameObject noEletricityWarning;

    public Sprite solarPanels;


    // Eletricity
    public bool hasEletricity;
    public int electricityProduction;
    public int electricityConsumption;



    private void Awake(){

        if(spriteRenderer == null){
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        if(builtByPlayer){
            spriteRenderer.color = new Color(1f,1f,1f,0.7f);
        }

        finishedSprite = spriteRenderer.sprite;
    }

    void Start()
    {

    }

    void Update(){
        IndicatePowerAvailability();


        if(currentHP <= 0){
            TransformToRuins();
        }
    }


    public void InitPreplacedBuilding(){
        Place();
        remainingTurnsToFinishConstruction = 0;
        constructionFinished = true;
    }

    public bool CanBePlaced(){
        if
        (  GridBuilding.gridBuilding.CanTakeArea(area) &&
           ResourcesManager.resourcesManager.freeMoney >= price &&
           ResourcesManager.resourcesManager.GetTotalPopulation() >= minimalPopulationToBuild
        ){
            return true;
        }
        
        return false;
    }


    public void FinishConstruction(){
        if(isDestroyed == false){
            constructionFinished = true;
            spriteRenderer.sprite = finishedSprite;
        }
    }

    public void Place(){
        placed = true;
        GridBuilding.gridBuilding.TakeArea(area);
        spriteRenderer.color = new Color(1f,1f,1f,1f);
        BuildingsManager.buildingManager.buildings.Add(this);

        if(builtByPlayer){
            AudioController.audioController.PlaceBuildingPlay();
            spriteRenderer.sprite = inConstructionSprite;
            ResourcesManager.resourcesManager.freeMoney -= price;
        }
    }

    

    public void OnMouseOver(){
        if(Input.GetMouseButtonDown(0) && placed){
            if(UIManager.uiManager.MouseOverUI() == false){
                SelectBuilding();
            }
        }
    }

    public void SelectBuilding(){
        UIManager.uiManager.showDeleteButton = true;
        BuildingsManager.buildingManager.currentBuilding = this;
    }

    public void DestroyBuilding(){

        if(isDestroyed){
            ResourcesManager.resourcesManager.freeMoney -= priceForRemovingDestoyedBuilding;
        }else{
            ResourcesManager.resourcesManager.freeMoney -= priceForRemovingFunctionBuilding;
        }

        BuildingsManager.buildingManager.buildings.Remove(this);
        Instantiate(destroyParticles, transform.position, Quaternion.identity);
        GridBuilding.gridBuilding.ClearBuildingArea(area);
        Destroy(this.gameObject);
    }


    public void BuildSolarPanels(){

        ResourcesManager.resourcesManager.freeMoney -= solarPanelPrice;
        hasSolarPanels = true;
        spriteRenderer.sprite = solarPanels;

        if(solarPanelEletricityProduction > electricityConsumption){
            electricityProduction += solarPanelEletricityProduction-electricityConsumption;
        }
    }


    public int GetConsumptionWithSolarPanels(){
        return electricityConsumption - solarPanelEletricityProduction;
    }

    public void IndicatePowerAvailability(){

        if(!constructionFinished || isDestroyed){
            noEletricityWarning.SetActive(false);
            return;
        }

        if(electricityConsumption > 0){
            if(hasEletricity){
                noEletricityWarning.SetActive(false);
            }else{
                if(placed){
                    noEletricityWarning.SetActive(true);
                }else{
                    noEletricityWarning.SetActive(false);
                }
            }
        }
    }


    
    public SustKeys GetKeys()
    {
        SustKeys keys;
        keys.population = population;
        keys.energy= energy;
        keys.poverty= poverty;
        keys.clean= clean;

        return keys;
    }

    public void InitialPlace()
    {
        Debug.LogFormat("Placing building {0}", name);
        placed = true;
        GridBuilding.gridBuilding.TakeArea(area);
        spriteRenderer.color = new Color(1f, 1f, 1f, 1f);        
    }

    public void TransformToRuins(){
        constructionFinished = true;
        fireParticles.SetActive(true);
        spriteRenderer.sprite = destroyedBuildingSprite;
        isDestroyed = true;
        population = 0;
        electricityConsumption = 0;
        electricityProduction = 0;
        taxIncome = 0;
    }

    public int CalculatePriceToFix(){
        float priceToFix = 0;
        float remainingPercentage = (currentHP/maxHP) * 100;
        priceToFix = (price * (100-remainingPercentage)) / 100;

        if(priceToFix <= 0){
            priceToFix = 1;
        }

        return (int)priceToFix;
    }


    public void FixBuilding(){
        ResourcesManager.resourcesManager.freeMoney -= CalculatePriceToFix();
        currentHP = maxHP;
    }
}
