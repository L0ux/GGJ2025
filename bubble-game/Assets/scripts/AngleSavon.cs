using System;
using UnityEngine;
using static Utils;



[ExecuteInEditMode]
public class AngleSavon : MonoBehaviour
{


    [SerializeField] Soap bloc1;
    [SerializeField] CoinRectangle angleDu1;

    [SerializeField] Soap bloc2;
    [SerializeField] CoinRectangle angleDu2;


    [ExecuteInEditMode]
    private void OnValidate()
    {

        BoxCollider2D myBoxCollider = GetComponent<BoxCollider2D>();
        if(bloc1 != null && bloc2 != null)
        {

            /****BLOC1******/
            foreach (CoinRectangle coin in Enum.GetValues(typeof(CoinRectangle)))
            {
                if (myBoxCollider.OverlapPoint(bloc1.getCoin(coin)))
                {
                    Debug.Log("Angle du bloc 1 (" + bloc1.gameObject + ") affecté à " + coin);
                    angleDu1 = coin;
                }
                    
            }

            /****BLOC2******/
            foreach (CoinRectangle coin in Enum.GetValues(typeof(CoinRectangle)))
            {
                if (myBoxCollider.OverlapPoint(bloc2.getCoin(coin)))
                {
                    Debug.Log("Angle du bloc 2 (" + bloc2.gameObject + ") affecté à " + coin);
                    angleDu2 = coin;
                }

            }
        }
            
    }

    private void Update()
    {
    }


    public void changerDeBord(Bulle bulle)
    {
        Debug.Log("On utilise un angle");
        Soap currentBloc = bulle.blocSavonSurLequelOnEstAttache.GetComponent<Soap>();
        Soap newBlocToAttach = null;
        Vector2 whereToSpawn = Vector2.negativeInfinity;

        if (currentBloc == bloc1)
        {
            newBlocToAttach = bloc2;
            whereToSpawn = bloc2.getCoin(angleDu2);
        }
        if (currentBloc == bloc2)
        {
            newBlocToAttach = bloc1;
            whereToSpawn = bloc1.getCoin(angleDu1);
        }

        if (newBlocToAttach == null || whereToSpawn == Vector2.negativeInfinity)
            throw new System.Exception("AZEAZE");

        bulle.stickTo(newBlocToAttach);

        /*Besoin de mettre un offset ?? */
        bulle.transform.position = whereToSpawn + ( newBlocToAttach.getJumpDirection() *0.3f) +(currentBloc.getJumpDirection() * 0.005f);

    }

}
