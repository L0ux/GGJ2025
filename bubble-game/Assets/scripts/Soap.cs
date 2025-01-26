using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using static Utils;

public class Soap : MonoBehaviour
{

   /* [SerializeField] public Vector2 jumpDirection;*/
    [SerializeField] public Direction directionSaut;


    bool murHorizontal;
    Vector2 bottomLeft;
    Vector2 bottomRight;
    Vector2 topLeft;
    Vector2 topRight;


    private void Start()
    {
        if (getJumpDirection() == Vector2.zero)
            throw new System.Exception("Bah non mon reuf faut mettre une direction de saut");
        gameObject.gameObject.tag = "soap";
       

        
        if (getJumpDirection().x == 0)
            murHorizontal = true;
        else
            murHorizontal = false;


        BoxCollider2D boxCollider =  this.GetComponent<BoxCollider2D>();
        Vector2 size = boxCollider.size * boxCollider.transform.localScale; // Taille de la boîte
        Vector2 offset = boxCollider.offset; // Décalage de la boîte

        // Calcul des coins du BoxCollider
        bottomLeft = (Vector2)boxCollider.transform.position + offset - size / 2;
        bottomRight = bottomLeft + new Vector2(size.x, 0);
        topLeft = bottomLeft + new Vector2(0, size.y);
        topRight = bottomLeft + size;
    }

    public Vector2 getCoin(CoinRectangle coinWanted)
    {
        switch (coinWanted) {
            case CoinRectangle.BAS_GAUCHE:
                return bottomLeft;
            case CoinRectangle.BAS_DROITE:
                return bottomRight;
            case CoinRectangle.HAUT_DROIT:
                return topRight;
            case CoinRectangle.HAUT_GAUCHE:
                return topLeft;
        }
        throw new System.Exception("Heu pourquoi");
    }

    public Vector2 getJumpDirection()
    {
        switch (this.directionSaut)
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


    public bool isWallHorizontal()
    {
        return this.murHorizontal;
    }



}
