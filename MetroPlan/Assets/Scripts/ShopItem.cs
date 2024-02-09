using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{

    public GameObject representedBuilding;


    void Start()
    {
        if(LevelRestrictor.levelRestrictor.CanBuildBuilding(representedBuilding) == false){
            GetComponent<Button>().interactable = false;
        }else{
            GetComponent<Button>().interactable = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
