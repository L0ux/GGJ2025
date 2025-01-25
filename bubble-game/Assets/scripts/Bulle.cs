using UnityEngine;
using UnityEngine.InputSystem;

public class Bulle : MonoBehaviour
{

    [SerializeField]
    private string murTag;

    [SerializeField]
    private string savonTag;

    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnJump()
    {
        rb.AddForce(new Vector2(2f, 23f), ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag(murTag))
        {
            OnHitWall();
        }else if(collision.gameObject.CompareTag(savonTag))
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
