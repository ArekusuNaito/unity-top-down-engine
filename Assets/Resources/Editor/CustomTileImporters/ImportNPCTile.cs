using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[Tiled2Unity.CustomTiledImporter]
public class ImportNPCTile : Tiled2Unity.ICustomTiledImporter
{
 
    public void HandleCustomProperties(GameObject gameObject,IDictionary<string, string> customProperties)
    {
        if (customProperties.ContainsKey("add") && customProperties["add"] == "NPC")
        {
            SpriteRenderer spriteRenderer =gameObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sortingOrder = 4;

			NPC npc = gameObject.AddComponent<NPC>();
            npc.conversationKey = customProperties["conversation"];
			//You can use the customProperties here for further editing.
        }
    }
 
    public void CustomizePrefab(GameObject prefab)
    {
        // Do nothing
    }
}