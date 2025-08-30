using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class TilemapObjectPlacer : MonoBehaviour
{
    public Tilemap tilemap; // Przypisz swoją Tilemapę w Inspectorze
    public TileBase burrowTile; // Kafelek nory
    public TileBase rockTile; // Kafelek kamienia
    public TileBase rockTile2; // Kafelek kamienia
    public GameObject burrowColliderPrefab; // Prefab do kolizji nory (okrągły)
    public GameObject rockColliderPrefab; // Prefab do kolizji kamienia (np. kwadratowy)
    public GameObject rock2ColliderPrefab; // Prefab do kolizji kamienia (np. kwadratowy)

    private List<Vector3> burrowPositions = new List<Vector3>();

    public void Generate()
    {
        if (tilemap == null)
        {
            Debug.LogError("Tilemap nie jest przypisana!");
            return;
        }

        FindAndPlaceColliders();
        Debug.Log(burrowPositions.Count);
    }

    private void FindAndPlaceColliders()
    {
        BoundsInt bounds = tilemap.cellBounds;

        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            TileBase tile = tilemap.GetTile(pos);
            if (tile == null) continue;

            Vector3 worldPos = tilemap.CellToWorld(pos) + new Vector3(0.5f, 0.5f, 0); // Centrowanie

            if (tile == burrowTile)
            {
                burrowPositions.Add(worldPos);
                // PlaceCollider(worldPos, burrowColliderPrefab);
            }
            else if (tile == rockTile)
            {
                PlaceCollider(worldPos, rockColliderPrefab);
            }
            else if (tile == rockTile2)
            {
                PlaceCollider(worldPos, rock2ColliderPrefab);
            }
        }
        Debug.Log(burrowPositions.Count);
    }

    private void PlaceCollider(Vector3 position, GameObject prefab)
    {
        if (prefab != null)
        {
            Instantiate(prefab, position, Quaternion.identity, transform);
        }
        else
        {
            Debug.LogWarning("Prefab dla collidera nie jest ustawiony!");
        }
    }

    public List<Vector3> GetBurrowPositions()
    {
        Debug.Log(burrowPositions.Count);
        return new List<Vector3>(burrowPositions);
    }
}
