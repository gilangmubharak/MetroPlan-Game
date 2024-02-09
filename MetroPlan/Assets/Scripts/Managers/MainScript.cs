using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScript : MonoBehaviour
{
    public string title;
    private string PrevScene;
    //private string scene1;
   // private string[] scene1 = new string[2];
    private List<string> scene1 = new List<string>();  //running history of scenes
 
    Scene scene;  
  public  void Start()
    {

           scene = SceneManager.GetActiveScene();
           PrevScene = PlayerPrefs.GetString("SceneNumber");
           PlayerPrefs.SetString("SceneNumber", SceneManager.GetActiveScene().name);
        //scene1.Add(scene.name);
        Debug.Log("Active Scene name is: " + scene.name + "\nActive Scene index: " + scene.buildIndex);
    }
    
    // Start is called before the first frame update
   public void ChangeSecneDynamic(string title)
    {  
       //scene1.Add(title);
      
       SceneManager.LoadScene(title);    
       
    
    }

    public void ChangeScene()
    { 
          Debug.Log("Active Scene name is: " + scene.name + "\nActive Scene index: " + scene.buildIndex);
        
       Debug.Log(" name is: " + title);
          //Debug.Log(" name is: " + title2);
       Debug.Log("Active Scene name is: " + scene.name + "\nActive Scene index: " + scene.buildIndex +"::" +scene1.Count + "::"+PrevScene );
       
      SceneManager.LoadScene(PrevScene);  
    
       
    }

   

   
}
