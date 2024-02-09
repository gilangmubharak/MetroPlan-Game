using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController cameraController;
    public bool allowMovingCamera = true;
    public int moveCameraOffset = 30;
    public float moveSpeed = 0.01f;
    Camera camera;
    Canvas canvas;
    float w;
    float h;

    public GameObject cameraBounds;

    void Awake()
    {
        cameraController = this;
        camera = this.GetComponent<Camera>();
        canvas = FindObjectOfType<Canvas>(); 
        h = canvas.GetComponent<RectTransform>().rect.height;
        w = canvas.GetComponent<RectTransform>().rect.width;
    }

    // Update is called once per frame
    void Update()
    {
        if(allowMovingCamera){
            MoveCamera();
        }
    }



    public void MoveCamera(){


        // Determine camera offset
        var cameraWidth = camera.orthographicSize * 2 * (float)Screen.width / Screen.height;
        var cameraHeight = camera.orthographicSize * 2;

        
        // Move right
        if((w - Input.mousePosition.x) <= moveCameraOffset){

            if((camera.transform.position.x+cameraWidth/2) < cameraBounds.transform.position.x+cameraBounds.GetComponent<SpriteRenderer>().bounds.size.x/2){
                camera.transform.position += new Vector3(moveSpeed,0,0);
            }
        }

        // Move left
        if((w - Input.mousePosition.x) >= (w-moveCameraOffset)){

            if((camera.transform.position.x - cameraWidth/2) > cameraBounds.transform.position.x-cameraBounds.GetComponent<SpriteRenderer>().bounds.size.x/2){
                camera.transform.position -= new Vector3(moveSpeed,0,0);
            }
        }

        // Move top
        if((h - Input.mousePosition.y) <= moveCameraOffset){
            if((camera.transform.position.y+cameraHeight/2) < cameraBounds.transform.position.y+cameraBounds.GetComponent<SpriteRenderer>().bounds.size.y/2){
                camera.transform.position += new Vector3(0,moveSpeed,0);
            }
        }

        // Move bottom
        if((h - Input.mousePosition.y) >= (h-moveCameraOffset)){
            if((camera.transform.position.y-cameraHeight/2) > cameraBounds.transform.position.y-cameraBounds.GetComponent<SpriteRenderer>().bounds.size.y/2){
                camera.transform.position -= new Vector3(0,moveSpeed,0);
            }
        }

    }
}
