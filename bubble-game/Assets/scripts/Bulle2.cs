using System.Collections;
using UnityEngine;


public class Bulle2 : MonoBehaviour
{




    /*Variable à ajuster*/
    [SerializeField] float flottementVersLeHaut = 2; // Accélération gravité Vers le haut


    [SerializeField] float moveSpeedOnTheGRound = 8f;
    [SerializeField] float moveSpeedOnTheAir;
    [SerializeField] float decelarationInTheAir;


    [SerializeField] float maxJumpForce = 30;
    [SerializeField] float maxChargeTimeJump;


    /*[SerializeField] float maxSpeedInTheAir;*/
    

    /*Affecté dans le start*/
    Rigidbody2D rb;
    Animator myAnimator;
    Animator barreChargementAnimator;
    SpriteRenderer mySpriteRenderer;

    Soap2 blocSavonSurLequelOnEstAttache;

    Vector2 directionSaut;

    bool inputAccepted = true;
    bool isDead = false;

    float _timeChargingJump = 0;

    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();

        myAnimator = this.GetComponent<Animator>();
        barreChargementAnimator = transform.Find("BarreChargement").GetComponent<Animator>();
        mySpriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (isDead)
            return;
        float moveInputHorizontal = 0;
        float moveInputVerical = 0;

        if (inputAccepted)
        {
            moveInputHorizontal = Input.GetAxisRaw("Horizontal");  // "Horizontal" : touches directionnelles (A, D ou flèches)
            moveInputVerical = Input.GetAxisRaw("Vertical");  // "Horizontal" : touches directionnelles (A, D ou flèches)
        }

        /*************DANS LES AIRS************************/
        if (!isAccrocheAuSavon())
        {

            /*A voir pour enlever*/
            myAnimator.SetBool("Soap", false);


            /* On pousse vers le haut*/
            rb.linearVelocityY += flottementVersLeHaut * Time.fixedDeltaTime;


            // Déplacement horizontal, conserve la vitesse verticale
            rb.linearVelocityX += moveInputHorizontal * moveSpeedOnTheAir *Time.fixedDeltaTime; 


            /*ON ralentit la vitesse*/
            rb.linearVelocityX = Mathf.MoveTowards(rb.linearVelocityX, 0, decelarationInTheAir * Time.fixedDeltaTime);
            

        }

        /*************SUR LE SOL*******************************/
        else
        {
            /*Si on en train de charger un saut */
            if (Input.GetKey(KeyCode.Space))
                chargerSaut();
            else
            {
                /*On saute si un saut a été chargé*/
                if (_timeChargingJump > 0)
                    sauter();
                else
                {
                    /*Sinon on gère le déplacement*/
                    /*Sur un mur horizontal*/
                    if (this.directionSaut.x == 0)
                        rb.linearVelocityX = moveInputHorizontal * moveSpeedOnTheGRound; // Déplacement horizontal, conserve la vitesse verticale

                    /*Sur un mur vertical*/
                    if (this.directionSaut.y == 0)
                        rb.linearVelocityY = moveInputVerical * moveSpeedOnTheGRound; // Déplacement horizontal, conserve la vitesse verticale

                    /*On se colle contre le bord de savon*/
                    rb.linearVelocity += -this.directionSaut *1;
                }

                _timeChargingJump = 0;
            }

        }
    }

    bool isAccrocheAuSavon()
    {
        return blocSavonSurLequelOnEstAttache != null;
    }



    void chargerSaut()
    {
        if (_timeChargingJump == 0)
        {
            barreChargementAnimator.SetTrigger("startChargement");
            myAnimator.SetTrigger("ChargeJump");
            rb.linearVelocity = Vector2.zero;
        }

        _timeChargingJump += Time.deltaTime;
    }

    void sauter()
    {
        barreChargementAnimator.SetTrigger("releaseButton");

        _timeChargingJump = Mathf.Min(_timeChargingJump, maxChargeTimeJump);


        /*On récup le pourcentage de temps appuyé*/
        float pourcentagePuissanceSaut = _timeChargingJump / maxChargeTimeJump;
        if (directionSaut == Vector2.zero)
            throw new System.Exception("Ah ben non on peut pas sauter à 0 ");

        myAnimator.SetTrigger("Jump");
        myAnimator.SetBool("Soap", false);
          myAnimator.SetTrigger("Jump");
        rb.AddForce(directionSaut * (pourcentagePuissanceSaut * maxJumpForce), ForceMode2D.Impulse);
        StartCoroutine(blocInput());
        Debug.Log("On saute avec une force de " + (pourcentagePuissanceSaut * maxJumpForce));
    }


    IEnumerator blocInput()
    {
        inputAccepted = false;
        yield return new WaitForSeconds(0.05f);
        inputAccepted = true;

    }

    private void stickTo(Soap2 blocSavon)
    {
        Debug.Log("On s'accroche au bloc " + blocSavon);
        this.directionSaut =blocSavon.getJumpDirection();
        blocSavonSurLequelOnEstAttache = blocSavon;

    }
    private void unStick()
    {
        Debug.Log("On se détache du bloc " + blocSavonSurLequelOnEstAttache);
        this.directionSaut = Vector2.zero;
        blocSavonSurLequelOnEstAttache = null;
        myAnimator.SetBool("Soap", false);
    }
    void pop()
    {
        isDead = true;
        rb.linearVelocity = Vector2.zero;
        myAnimator.SetTrigger("Explosion");
        StartCoroutine(waittheGameOver());
        return;
    }

    IEnumerator waittheGameOver()
    {
        yield return new WaitForSeconds(0.2f);

        GameManager.Instance.LooseGame();
        Destroy(this.gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "bordCollant")
        {
            Debug.Log("On est tjrs sur un bloc");

            Soap2 hehe = collision.gameObject.GetComponent<Soap2>();
            blocSavonSurLequelOnEstAttache = hehe;
            this.directionSaut = hehe.getJumpDirection();
            this.rotate(this.directionSaut);
            myAnimator.SetBool("Soap", true);
        }
    }

    void rotate(Vector2 normalVector)
    {

        float newAngle = Utils.calculateRealAngle(Vector2.up, normalVector);
        transform.rotation = Quaternion.Euler(0, 0, newAngle);


    }

        /*  private void OnTriggerEnter2D(Collider2D collision)
          {
              if(collision.gameObject.tag == "bordCollant")
              {
                  stickTo(collision.gameObject.GetComponent<Soap2>());
              }
          }
      */



        /*Vérif si il n'y a pas d'autres collisions*/
        private void OnTriggerExit2D(Collider2D collision)
       {
        if (collision.gameObject.tag == "bordCollant")
        {
            if (collision.gameObject.GetComponent<Soap2>() == blocSavonSurLequelOnEstAttache)
            {
                unStick();
            }
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("wall") && !isAccrocheAuSavon())
        {
            pop();
            return;
        }
    }


}
