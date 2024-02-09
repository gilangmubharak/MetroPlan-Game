using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class GridBuilding : MonoBehaviour
{
    public static GridBuilding gridBuilding;
    private static Dictionary<TileType, TileBase> tileBases = new Dictionary<TileType, TileBase>();

    public GridLayout gridLayout;
    public Tilemap buildingsTilemap;
    public Tilemap placementIndicatorTilemap;

    private BoundsInt prevBuildingArea;
    public Building buildingToBuildInstance;
    public GameObject buildingToBuild;

    public Tile freeTile;
    public Tile occupiedTile;
    public Tile buildingTile;
    public enum TileType { FreeIndicator, OccupiedIndicator, Empty, Building}



    public void Awake(){
        gridBuilding = this;
        tileBases.Add(TileType.Empty, null);
        tileBases.Add(TileType.FreeIndicator, freeTile);
        tileBases.Add(TileType.OccupiedIndicator, occupiedTile);
        tileBases.Add(TileType.Building, buildingTile);
    }

    void Start()
    {
        
    }


    void Update()
    {
        // Check for clicks on tiles
        DetectClickedTile();
    }



    public void InitializeBuilding(GameObject building)
    {
        buildingToBuildInstance = Instantiate(building, CameraController.cameraController.GetComponent<Camera>().transform.position, Quaternion.identity).GetComponent<Building>();
        BuildingIndicators(CameraController.cameraController.GetComponent<Camera>().transform.position, buildingToBuildInstance);
    }



    private static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
    {
        TileBase[] array = new TileBase[area.size.x * area.size.y * area.size.z];
        int counter = 0;

        foreach(var v in area.allPositionsWithin)
        {
            Vector3Int pos = new Vector3Int(v.x, v.y, 0);
            array[counter] = tilemap.GetTile(pos);
            counter++;
        }

        return array;
    }


    private static void SetTilesBlock(BoundsInt area, TileType type, Tilemap tilemap)
    {
        int size = area.size.x * area.size.y * area.size.z;
        TileBase[] tileArray = new TileBase[size];
        FillTiles(tileArray, type);
        tilemap.SetTilesBlock(area, tileArray);
    }

    private static void FillTiles(TileBase[] arr, TileType type)
    {
        for(int i = 0; i < arr.Length; i++)
        {
            arr[i] = tileBases[type];
        }
    }

    private void ClearArea()
    {
        TileBase[] toClear = new TileBase[prevBuildingArea.size.x * prevBuildingArea.size.y * prevBuildingArea.size.z];
        FillTiles(toClear, TileType.Empty);
        placementIndicatorTilemap.SetTilesBlock(prevBuildingArea, toClear);
    }



    public void ClearBuildingArea(BoundsInt area)
    {
        TileBase[] toClear = new TileBase[area.size.x * area.size.y * area.size.z];
        FillTiles(toClear, TileType.Empty);
        buildingsTilemap.SetTilesBlock(area, toClear);
    }




    public void BuildingIndicators(Vector3 position, Building buildingInstance = null)
    {

        ClearArea();

        Vector3 centerPos = new Vector3(position.x, position.y - buildingInstance.area.size.y/2, position.z);

        buildingInstance.area.position = gridLayout.WorldToCell(centerPos);

        BoundsInt buildingArea = buildingInstance.area;

        TileBase[] baseArray = GetTilesBlock(buildingArea, buildingsTilemap);

        int size = baseArray.Length;
        TileBase[] tileArray = new TileBase[size];




        bool canBuild = true;

        if(ResourcesManager.resourcesManager.freeMoney < buildingInstance.price){
            canBuild = false;
        }

        for(int i = 0; i < baseArray.Length; i++){
            if(baseArray[i] != null){
                canBuild = false;
                break;
            }
        }

        for(int i = 0; i < baseArray.Length; i++){
            if(canBuild){
                tileArray[i] = tileBases[TileType.FreeIndicator];
                buildingInstance.spriteRenderer.color = new Color(1,1f,1f,0.7f);
            }else{
                FillTiles(tileArray, TileType.OccupiedIndicator);
                buildingInstance.spriteRenderer.color = new Color(255f,0f,0f,0.7f);
            }
        }

        placementIndicatorTilemap.SetTilesBlock(buildingArea, tileArray);
        prevBuildingArea = buildingArea;
    }


    public bool CanTakeArea(BoundsInt area)
    {
        TileBase[] baseArray = GetTilesBlock(area, buildingsTilemap);
        foreach(var b in baseArray)
        {
            if(b != null){
                return false;
            }
        }

        return true;
    }

    public void TakeArea(BoundsInt area){
        // Reset placement indicator tiles to null
        SetTilesBlock(area, TileType.Empty, placementIndicatorTilemap);

        // Set buildingsTilemap to building tile
        SetTilesBlock(area, TileType.Building, buildingsTilemap);
    }


    public void DetectClickedTile(){
        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var noZ = new Vector3(pos.x, pos.y);
        Vector3Int mouseCell = buildingsTilemap.WorldToCell(noZ);

        if(Input.GetMouseButtonUp(0))
        {
            Tile clickedTile = (Tile)buildingsTilemap.GetTile(mouseCell);

            if(clickedTile == null){
                UIManager.uiManager.showDeleteButton = false;
                BuildingsManager.buildingManager.currentBuilding = null;
            }else if( clickedTile.sprite.name == "water_isometric" || clickedTile.sprite.name == "trees" || clickedTile.sprite.name.Contains("road")){
                UIManager.uiManager.showDeleteButton = false;
                BuildingsManager.buildingManager.currentBuilding = null;
            }
        }
    }

    public void TryToPlaceBuilding(){
        if(buildingToBuildInstance.CanBePlaced()){
            buildingToBuildInstance.Place();
            buildingToBuildInstance = null;
        } else
        {
            AudioController.audioController.CantPlaceBuildingPlay();
        }
    }

    public void TryCreateBuildingPrototype(){
        if(buildingToBuildInstance == null){
            InitializeBuilding(buildingToBuild);
        }else{
            if(buildingToBuildInstance.buildingName != buildingToBuild.GetComponent<Building>().buildingName){
                CancelPlacement();
                TryCreateBuildingPrototype();
            }
        }
    }

    public void CancelPlacement(){
        ClearArea();
        Destroy(buildingToBuildInstance.gameObject);
        buildingToBuildInstance = null;
    }
}
