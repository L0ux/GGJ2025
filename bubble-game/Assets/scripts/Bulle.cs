using System.Collections;
using UnityEngine;
using GameUtils;


public class Bulle : MonoBehaviour
{




    /*Variable à ajuster*/
    [SerializeField] float flottementVersLeHaut = 2; // Accélération gravité Vers le haut


    [SerializeField] float moveSpeedOnTheGround = 8f;
    [SerializeField] float maxMoveSpeedOnTheGround = 6f;
    [SerializeField] float decelerationOnTheGround;


    [SerializeField] float moveSpeedOnTheAir;
    [SerializeField] float decelarationInTheAir;

    [SerializeField] float maxMooveSpeedInTheAir = 6f;

    


    [SerializeField] float maxJumpForce = 30;
    [SerializeField] float maxChargeTimeJump;


    [SerializeField] Collider2D zoneBarriere;
    [SerializeField] ParticleSystem particleOnJump;
    [SerializeField] ParticleSystem particleOnJump2;

    [SerializeField] ParticleSystem particleOnSlide;
    /*[SerializeField] float maxSpeedInTheAir;*/


    /*Affecté dans le start*/
    Rigidbody2D rb;
    Animator myAnimator;
    Animator barreChargementAnimator;
    SpriteRenderer mySpriteRenderer;

    Soap blocSavonSurLequelOnEstAttache;

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


        if (zoneBarriere == null)
            throw new System.Exception("Faut mettre la barriere");

        zoneBarriere.enabled = false;
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
            float newVelocityY = rb.linearVelocityY + (flottementVersLeHaut * Time.fixedDeltaTime);
            rb.linearVelocityY = Mathf.Min(maxMooveSpeedInTheAir, newVelocityY);


            // Déplacement horizontal, conserve la vitesse verticale
            rb.linearVelocityX = rb.linearVelocityX +  moveInputHorizontal * moveSpeedOnTheAir *Time.fixedDeltaTime;
          


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
                        rb.linearVelocityX += moveInputHorizontal * moveSpeedOnTheGround; // Déplacement horizontal, conserve la vitesse verticale

                    /*Sur un mur vertical*/
                    if (this.directionSaut.y == 0)
                        rb.linearVelocityY += moveInputVerical * moveSpeedOnTheGround; // Déplacement horizontal, conserve la vitesse verticale


                    rb.linearVelocity = Utils.vectorClamp(rb.linearVelocity, maxMoveSpeedOnTheGround);
                    rb.linearVelocity = Vector2.MoveTowards(rb.linearVelocity, Vector2.zero,decelerationOnTheGround); 
                       
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
           
            myAnimator.SetTrigger("ChargeJump");
            rb.linearVelocity = Vector2.zero;
        }

        _timeChargingJump += Time.deltaTime;
        barreChargementAnimator.SetFloat("chargement", _timeChargingJump / maxChargeTimeJump);
    }

    void sauter()
    {
        barreChargementAnimator.SetTrigger("releaseButton");
        barreChargementAnimator.SetFloat("chargement", 0);
        _timeChargingJump = Mathf.Min(_timeChargingJump, maxChargeTimeJump);

        particleOnJump.Play();
        particleOnJump2.Play();

        /*On récup le pourcentage de temps appuyé*/
        float pourcentagePuissanceSaut = _timeChargingJump / maxChargeTimeJump;

        if (directionSaut == Vector2.zero)
            throw new System.Exception("Ah ben non on peut pas sauter à 0 ");

        myAnimator.SetTrigger("Jump");
        myAnimator.SetBool("Soap", false);
        rb.AddForce(directionSaut * (pourcentagePuissanceSaut * maxJumpForce), ForceMode2D.Impulse);
        StartCoroutine(blocInput());
        Debug.Log("On saute avec une force de " + (pourcentagePuissanceSaut * maxJumpForce));
    }


    IEnumerator blocInput()
    {
        inputAccepted = false;
        yield return new WaitForSeconds(0.15f);
        inputAccepted = true;

    }

    private void stickTo(Soap blocSavon)
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
        zoneBarriere.enabled = false;
        particleOnSlide.Stop();

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

            Soap hehe = collision.gameObject.GetComponent<Soap>();
            blocSavonSurLequelOnEstAttache = hehe;
            this.directionSaut = hehe.getJumpDirection();
            this.rotate(this.directionSaut);
            myAnimator.SetBool("Soap", true);
            zoneBarriere.enabled = true;
            if (!particleOnSlide.isPlaying)
                particleOnSlide.Play();
        }
    }

    void rotate(Vector2 normalVector)
    {

        float newAngle = Utils.calculateRealAngle(Vector2.up, normalVector);
        transform.rotation = Quaternion.Euler(0, 0, newAngle);


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("fin"))
        {
            GameManager.Instance.WinRoom();
        }

        if (collision.gameObject.tag == "bordCollant" && !isAccrocheAuSavon() )
        {
            rb.linearVelocity = Vector2.zero;
        }
    }



        /*Vérif si il n'y a pas d'autres collisions*/
        private void OnTriggerExit2D(Collider2D collision)
       {
        if (collision.gameObject.tag == "bordCollant")
        {
            if (collision.gameObject.GetComponent<Soap>() == blocSavonSurLequelOnEstAttache)
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

    public Direction GetDirection(){
        if(rb.linearVelocityY > 1){
            return Direction.MOVING_UP;
        }
        if(rb.linearVelocityY < -1){
            return Direction.MOVING_DOWN;
        }
        return Direction.NEUTRAL;
    }


}
