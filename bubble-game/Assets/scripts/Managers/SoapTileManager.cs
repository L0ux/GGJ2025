using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SoapTileManager : MonoBehaviour
{
    public Tilemap tilemap;       // Reference to the Tilemap


    [Serializable]
    public struct coupleTilePrefab
    {
        public TileBase tile;   // The tile type where we want to spawn prefabs
        public GameObject prefab;
    }

    [SerializeField]
    public coupleTilePrefab[] rules;


    void Start()
    {
        SpawnPrefabsOnTiles();
    }

    void SpawnPrefabsOnTiles()
    {
        BoundsInt bounds = tilemap.cellBounds;

        // Loop through all tiles in the tilemap
        foreach (Vector3Int position in bounds.allPositionsWithin)
        {
            TileBase currentTile = tilemap.GetTile(position);

           
            foreach(coupleTilePrefab couple in rules)
            {
                if(currentTile == couple.tile)
                {
                    Vector3 worldPos = tilemap.GetCellCenterWorld(position);
                    GameObject newSoap = Instantiate(couple.prefab, worldPos, Quaternion.identity);
                    newSoap.transform.SetParent(tilemap.gameObject.transform);
                }
            }
        }
    }
}