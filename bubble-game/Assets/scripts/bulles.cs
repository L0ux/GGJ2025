using UnityEngine;
using UnityEngine.InputSystem;

public class bulles : MonoBehaviour
{

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
        rb.AddForce(new Vector2(2f,23f), ForceMode2D.Impulse); 
    }
}
