using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragging : MonoBehaviour
{
    private Vector3 offset;
    private Vector3 lastRaycastHit;
    public bool isDragged = false;


    private void Awake(){
        try
        {
            transform.position = SnapCoordinateToGrid(GetMouseWorldPosition());
        }
        catch(System.Exception e)
        {
            Debug.LogWarningFormat("Exception: {0}", e);
        }
    }
    private void Start()
    {
        if(GetComponent<Building>().builtByPlayer){
            transform.position = SnapCoordinateToGrid(CameraController.cameraController.GetComponent<Camera>().transform.position);
        }else{
            transform.position = SnapCoordinateToGrid(transform.position);
            GridBuilding.gridBuilding.BuildingIndicators(transform.position, GetComponent<Building>());
            GridBuilding.gridBuilding.buildingToBuildInstance = null;
            GetComponent<Building>().InitPreplacedBuilding();
        }
    }


    private void OnMouseDown(){
        if(!this.GetComponent<Building>().placed){
            offset = transform.position - GetMouseWorldPosition();
            isDragged = true;
        }
    }

    private void OnMouseUp(){
        if(!this.GetComponent<Building>().placed){
            transform.position = SnapCoordinateToGrid(GetMouseWorldPosition());
            isDragged = false;
        }
    }


    private void OnMouseDrag(){
        if(!this.GetComponent<Building>().placed){
            transform.position = SnapCoordinateToGrid(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            GridBuilding.gridBuilding.BuildingIndicators(SnapCoordinateToGrid(GetMouseWorldPosition()), GetComponent<Building>());
        }
    }



    public Vector3 GetMouseWorldPosition(){
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if(hit){
            this.lastRaycastHit = hit.point;
            return lastRaycastHit;
        }else{
            return lastRaycastHit;
        }
    }


    public Vector3 SnapCoordinateToGrid(Vector3 position){
        Vector3Int cellPos = GridBuilding.gridBuilding.gridLayout.WorldToCell(position);
        position = GridBuilding.gridBuilding.gridLayout.GetComponent<Grid>().GetCellCenterWorld(cellPos);

        if(!GetComponent<Building>().hasClearCenter){
            position -= new Vector3(0,0.5f,0);
        }

        return position;
    }
}
