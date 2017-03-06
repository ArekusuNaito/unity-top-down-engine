using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[Tiled2Unity.CustomTiledImporter]
public class ImportChestTile : Tiled2Unity.ICustomTiledImporter
{
 
    public void HandleCustomProperties(GameObject gameObject,IDictionary<string, string> customProperties)
    {
        if (customProperties.ContainsKey("add"))
        {
            SpriteRenderer spriteRenderer =gameObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sortingOrder = 4;

			gameObject.AddComponent<Chest>();
			//You can use the customProperties here for further editing.
        }
    }
 
    public void CustomizePrefab(GameObject prefab)
    {
        // Do nothing
    }
}