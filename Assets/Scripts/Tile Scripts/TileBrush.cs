using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Tilemaps;
using UnityEngine;
[CreateAssetMenu(fileName = "Object Brush", menuName = "Brushes/Object Brush")]
[CustomGridBrush(false,true,false,"Object Brush")]
public class TileBrush : GameObjectBrush
{


    public override void Erase(GridLayout grid, GameObject targetObject, Vector3Int position)
    {
        
        if(targetObject.layer==31) return;
        Transform erased = GetObject(grid, targetObject.transform, new Vector3Int(position.x, position.y, 0));
        if(erased!=null)
            Undo.DestroyObjectImmediate(erased.gameObject);
    }
 
    static Transform GetObject(GridLayout grid, Transform parentObject, Vector3Int position)
    {
        int childCount = parentObject.childCount;

        Vector3 min = grid.LocalToWorld(grid.CellToLocalInterpolated(position));
        Vector3 max = grid.LocalToWorld(grid.CellToLocalInterpolated(position + Vector3Int.one));

        Bounds bounds = new Bounds((min + max) * 0.5f, max - min);


        for (int index = 0; index < childCount; index++)
        {
            Transform child = parentObject.GetChild(index);
            if (bounds.Contains(child.position)) 
                return child;
        }
  

        return null;
    }
}