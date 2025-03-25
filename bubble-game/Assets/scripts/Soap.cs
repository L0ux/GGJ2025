using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using static Utils;

public class Soap : MonoBehaviour
{

   /* [SerializeField] public Vector2 jumpDirection;*/
    [SerializeField] public Direction directionSaut;

    public Vector2 getJumpDirection()
    {
        return Utils.getVectorDIrection(this.directionSaut);
    }
}
