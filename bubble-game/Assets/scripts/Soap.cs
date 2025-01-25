using Unity.VisualScripting;
using UnityEngine;

public class Soap : MonoBehaviour
{

    [SerializeField] Vector2 jumpDirection;

    bool murHorizontal;


    private void Start()
    {
        if (jumpDirection == Vector2.zero)
            throw new System.Exception("Bah non mon reuf faut mettre une direction de saut");
        
        
        if (jumpDirection.y == 0)
            murHorizontal = false;
        else
            murHorizontal = true;
    }


    public Vector2 getJumpDirection()
    {
        return this.jumpDirection;
    }


    public bool isWallHorizontal()
    {
        return this.murHorizontal;
    }
}
