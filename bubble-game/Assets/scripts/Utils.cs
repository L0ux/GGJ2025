using System.IO;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public static class Utils 
{

    public enum CoinRectangle
    {
        BAS_GAUCHE =0,
        BAS_DROITE = 1,
        HAUT_GAUCHE =2,
        HAUT_DROIT =3 ,
    }

    public enum Direction
    {
       HAUT,
       GAUCHE,
       BAS, 
       DROITE
    }


    public static Vector2 getVectorDIrection(Direction direction)
    {
        switch (direction )
        {
            case Direction.DROITE:
                return new Vector2(1, 0);
            case Direction.GAUCHE:
                return new Vector2(-1, 0);
            case Direction.HAUT:
                return new Vector2(0, 1);
            case Direction.BAS:
                return new Vector2(0, -1);
        }

        throw new System.Exception("HM");
    }


    

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


    public static float calculateRealAngle(Vector3 from, Vector3 end)
    {
        float angle = Vector3.Angle(from, end);
        if ((Vector3.Cross(from, end).z < 0))
            angle = -angle;
        return angle;
    }


}
