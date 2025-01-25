using UnityEngine.InputSystem;
using UnityEngine;
using Unity.VisualScripting;
using static UnityEditor.Searcher.SearcherWindow.Alignment;
using UnityEngine.Rendering;

public class MoveInTheAir : MonoBehaviour
{
    [SerializeField] float customGravity = 2; // Accélération gravité personnalisée
    [SerializeField] float horizontalMovementSpeed = 5f;

    [SerializeField] float maxJumpForce = 30;

    [SerializeField] float maxChargeTimeJump;
    [SerializeField] float decelarationHorizontal;
    [SerializeField] Vector2 TMP_JUMP_DIRECTION;

    float TMP_jump_force = 30;

    Rigidbody2D rb;
    float horizontalMoveDirection;
    bool accrochedAuSavon;


    float _timeChargingJump = 0f;
    




    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        stick();

    }
   
    // Update is called once per frame
    void Update()
    {
        /***************MOUVEMENT HORIZONTAL************/

        /*On lit l'input*/
        horizontalMoveDirection = Input.GetAxis("Horizontal");

        rb.AddForceX(horizontalMoveDirection * horizontalMovementSpeed);

        /*Freinage*/
        /* ?? peut être mettre le freinage uniquement si on n'appuie pas sur la touche ?? */
        rb.linearVelocityX = Mathf.MoveTowards(rb.linearVelocityX, 0, decelarationHorizontal * Time.fixedDeltaTime);

        /*?? Ajouter une vitesse max ??*/


        /***************GESTION DU SAUT************/

        if(Input.GetKey(KeyCode.Space) && accrochedAuSavon)
        {
            _timeChargingJump += Time.deltaTime;
            Debug.Log("On charge le saut charge Time : " + _timeChargingJump);
        }
        else 
        {
            if (_timeChargingJump > 0 && accrochedAuSavon)
            {
                _timeChargingJump = Mathf.Max(_timeChargingJump, maxChargeTimeJump);
                
                /*On récup le pourcentage de temps appuyé,
                 * 0 si 0secondes 
                 * 1 si appuyé maxChargeTimeJump*/
                float jumpPercent = Mathf.Lerp(0, maxChargeTimeJump, _timeChargingJump);
                doAJump(TMP_JUMP_DIRECTION, jumpPercent * maxJumpForce);
                
            }
            _timeChargingJump = 0;
        }


    }

    void FixedUpdate()
    {
        if(!accrochedAuSavon)
            rb.linearVelocityY += customGravity * Time.fixedDeltaTime;

    }


    void doAJump(Vector2 jumpDirection, float jumpForce)
    {
        unstick();
        rb.AddForce(jumpDirection * jumpForce, ForceMode2D.Impulse);
        Debug.Log("On saute avec une force de " + jumpForce);
    }






    /*Appelé par la barre d'espace*//*
    public void OnJump()
    {
        if (!accrochedAuSavon)
            return;
        
        
        doAJump(TMP_JUMP_DIRECTION);
    }*/


    void stick()
    {
        accrochedAuSavon = true;
    }


    void unstick()
    {
        accrochedAuSavon = false;
    }
}