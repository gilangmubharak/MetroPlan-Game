using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public struct SustKeys
{
    public int population;
    public int energy;
    public int poverty;    
    public int clean;
    

}
    

public class LevelManager : MonoBehaviour
{
        
    public int TestNumberOfTurnsToPassLevel = 5;
    private int TestNumberOfTurnsToPassLevelCounter = 0;
    public bool TEST_MODE;
    LevelInfo levelInfo;
    public Button[] lvlButtons;
   
    public static LevelManager Instance { get; private set; }
    

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    SustKeys sKeys;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("LevelManager.Start");

        levelInfo = GetLevelInfo();
        
        
        sKeys.energy = 0;
        sKeys.poverty = 0;
        sKeys.population= 0;
        sKeys.clean = 0;
        //Debug.LogFormat("GetTotalPopulation: number of buildings: {0}", BuildingsManager.buildingManager.buildings.Count);

         int levelAt = PlayerPrefs.GetInt("levelAt", 2); 
   
          for (int i = 0; i < lvlButtons.Length; i++)
          {
              if (i + 2 > levelAt)
                  lvlButtons[i].interactable = false;
         }
       



      
    }

    LevelInfo GetLevelInfo()
    {
        if (levelInfo==null)
        {
            levelInfo = GetComponent<LevelInfo>();
            if (levelInfo == null)
            {
                Debug.LogWarning("Cannot find LevelInfo, please add LevelInfo component to the LevelManager");
                return null;
            }
            else
            {
                return levelInfo;
            }
        }
        else
        {
            return levelInfo;
        }
    }

    public string GetLevelName()
    {
        return GetLevelInfo().levelName;
    }

    
    
    // Update is called once per frame
    void Update()
    {
        
    }
    /*

    void PlaceBuilding(Building b, int x, int y)
    {
        if (IsTileEmpty(currentLevel, x, y))
        {
            boards[currentLevel].SetTile(x, y, b);
            SetKeys(b);
        }
    }
    */
    public void SetKeys(Building b)
    {
        SustKeys bKeys = b.GetKeys();
        sKeys.energy += bKeys.energy;
        sKeys.population += bKeys.population;
        sKeys.poverty += bKeys.poverty;
        sKeys.clean += bKeys.clean;
        string log= String.Format("Updating KPIs (population={0}, energy={1}, poverty={2}, clean={3}", sKeys.population, sKeys.energy, sKeys.poverty, sKeys.clean);
        Debug.Log(log);
    }

    

    public bool IsLevelFailed()
    {
        if (TurnManager.turnManager.currentTurnNumber > GetLevelInfo().maxTurns)
            return true;
        else
            return false;

    }
    public bool IsLevelFinished()
    {
        
        if (TEST_MODE)
        {
            if (++TestNumberOfTurnsToPassLevelCounter == TestNumberOfTurnsToPassLevel)
            {
                TestNumberOfTurnsToPassLevelCounter = 0;
                return true;
            }

        }
        else
        {
            bool levelFinished = true;
            levelFinished &= GetLevelInfo().targetClean <= sKeys.clean;
            levelFinished &= GetLevelInfo().targetPopulation <= sKeys.population;
            levelFinished &= GetLevelInfo().targetEnergy <= sKeys.energy;
            levelFinished &= GetLevelInfo().targetPoverty <= sKeys.poverty;
            return levelFinished;
        }
        return false;
    }

    public bool IsLastLevel()
    {
        return GetLevelInfo().isLastLevel;
    }

    public void UpdateKeys()
    {
        this.sKeys.population= ResourcesManager.resourcesManager.GetTotalPopulation();
        this.sKeys.energy= ResourcesManager.resourcesManager.GetEletricProduction();
        this.sKeys.clean= ResourcesManager.resourcesManager.GetEletricConsumption();
        this.sKeys.poverty= ResourcesManager.resourcesManager.GetTaxIncome();
    }

    public bool NextLevel()
    {
        
        if (GetLevelInfo().isLastLevel)
        {
            Debug.Log("Max levels reached. Endgame");
            return false;
        }
        else
        {
            Debug.Log("NextLevel: Current level= " + GetLevelInfo().levelName + " Loading " + GetLevelInfo().nextLevel);
            SceneManager.LoadScene(GetLevelInfo().nextLevel);
            levelInfo = null;
            return true;
        }

    }
    public bool ReloadLevel()
    {
        
        Debug.Log("Reload level: Current level= " + GetLevelInfo().levelName  + " Loading " + SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        levelInfo = null;
        return true;
        

    }

    public void ExitGame()
    {
        Debug.Log("Exit game");
        //Application.Quit();
        SceneManager.LoadScene("Main");
    }

    /// <summary>
    /// Call this method from UI to show level description and 
    /// objetives
    /// </summary>
    /// <returns>Level description</returns>
    public string GetLevelDescription()
    {
        //Debug.Log("Current level= " + currentLevel);
        return GetLevelInfo().levelDescription.Replace('|', '\n');
    }
   
}
