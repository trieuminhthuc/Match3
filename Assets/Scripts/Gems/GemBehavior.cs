using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemBehavior : MonoBehaviour
{
    #region Class Definition
    
  
    private string spriteName;
    public SpriteRenderer spriteRenderer;

    public float rowPadding;
    public float colPadding;

    public GemData gemData;


  

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

     

    }

    #endregion

    #region Initial Gem (Sprite, position, ......)

    public void InitialGem()
    {
        if (gemData == null) return;

        Sprite sprite = Resources.Load<Sprite>($"Sprites/Candy/{gemData.GemId}");
        spriteRenderer.sprite = sprite;



        int r, c;
      
        r = gemData.GetColIdx();
        c = gemData.GetRowIdx();

        
        Vector3 position = new Vector3 (r * GemConfig.TILE_WIDTH, c * GemConfig.TILE_HEIGHT);

        transform.localPosition = position;

        
        



        spriteRenderer.sortingOrder = c + 10;

      

    }


    

    public void CreateBaseSprite(Transform parent)
    {
        GameObject underlyingSprite = new GameObject("UnderlyingSprite");
       
        underlyingSprite.transform.SetParent(parent);
        underlyingSprite.transform.localPosition = Vector3.zero;
        SpriteRenderer underlyingSpriteRenderer = underlyingSprite.AddComponent<SpriteRenderer>();
       
        Sprite underlyingSpriteImage = Resources.Load<Sprite>("Sprites/UI/BaseSprite");
        underlyingSpriteRenderer.sprite = underlyingSpriteImage;


        underlyingSpriteRenderer.sortingOrder = spriteRenderer.sortingOrder - 1;
        underlyingSprite.transform.SetParent(GameObject.FindGameObjectWithTag("BaseContainer").transform);
    }

    public void SetGemData(GemData gemData)
    {
        this.gemData = gemData;
    }

    #endregion


    #region Gem Logic

    private void Update()
    {
        
    }

  
    

    #endregion

}


