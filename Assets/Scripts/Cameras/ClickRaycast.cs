using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickRaycast : MonoBehaviour
{
    
    public GemBehavior currentGem;

    public Tuple<Vector2Int, Vector2Int> currentPair;


    void Start()
    {
        currentPair = new Tuple<Vector2Int, Vector2Int> (
            Vector2Int.zero, 
            Vector2Int.zero
        );
    }

    // Update is called once per frame
    void Update()
    {
       /* if (Input.GetMouseButtonDown(0))
        {
           
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);


            RaycastHit2D cast = Physics2D.Raycast(worldPoint, Vector2.zero);

            if (cast.collider != null) {
                Debug.Log("Hit");
            }
            
        } */
    }

    public void SwapGem()
    {

    }
}


