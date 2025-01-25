using System.IO;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public static class Utils 
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   public static Vector2[] getAngles(BoxCollider2D boxCollider )
    {

        Vector2 size = boxCollider.size * boxCollider.transform.localScale; // Taille de la boîte
        Vector2 offset = boxCollider.offset; // Décalage de la boîte

        // Calcul des coins du BoxCollider
        Vector2 bottomLeft = (Vector2) boxCollider.transform.position + offset - size / 2;
        Vector2 bottomRight = bottomLeft + new Vector2(size.x, 0);
        Vector2 topLeft = bottomLeft + new Vector2(0, size.y);
        Vector2 topRight = bottomLeft + size;
        Vector2[] tablo  = { bottomLeft, bottomRight, topLeft, topRight };
        return tablo;

    }
}
