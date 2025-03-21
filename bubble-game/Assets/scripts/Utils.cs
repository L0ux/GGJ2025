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


    public static float calculateRealAngle(Vector3 from, Vector3 end)
    {
        float angle = Vector3.Angle(from, end);
        if ((Vector3.Cross(from, end).z < 0))
            angle = -angle;
        return angle;
    }


    public static Vector2 vectorClamp(Vector2 vector, float minMaxValue)
    {
        return new Vector2(Mathf.Clamp(vector.x, -minMaxValue, minMaxValue),
            Mathf.Clamp(vector.y, -minMaxValue, minMaxValue));
    }


}
