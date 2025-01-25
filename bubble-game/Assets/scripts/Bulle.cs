using UnityEngine;
using UnityEngine.InputSystem;

public class Bulle : MonoBehaviour
{

    [SerializeField]
    private string murTag;

    [SerializeField]
    private string savonTag;




    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag(murTag))
        {
            OnHitWall();
        }
        else if (collision.gameObject.CompareTag(savonTag))
        {
            OnHitSoap();
        }


    }


    private void OnHitWall()
    {
        Debug.Log("OnHitWall");
    }

    private void OnHitSoap()
    {
        Debug.Log("OnHitSoap");
    }

}