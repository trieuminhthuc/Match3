using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;


public class GemBehavior : MonoBehaviour, IPointerDownHandler
{
    #region Class Definition
    
  
    private string spriteName;
    public SpriteRenderer spriteRenderer;

    public float rowPadding;
    public float colPadding;

    public GemData gemData;
    public bool isSelected;

    public GameObject particlePrefab;


    private Sequence selectedAnimation;
    private ParticleSystem destroyParticle;


    public Sprite sprite;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        destroyParticle = particlePrefab.GetComponent<ParticleSystem>();
        destroyParticle.Stop();


    }

    #endregion

    #region Initial Gem (Sprite, position, ......)

    public void InitialGem()
    {
        if (gemData == null) return;

        sprite = Resources.Load<Sprite>($"Sprites/Candy/{gemData.GemId}");
        spriteRenderer.sprite = sprite;

        destroyParticle.textureSheetAnimation.SetSprite(0, sprite);
     


        int r, c;
      
        r = gemData.GetColIdx();
        c = gemData.GetRowIdx();

        
        
        
        



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


    #region Selecting logic

    public void OnSelected()
    {

        selectedAnimation = null;
        selectedAnimation.Kill();

        selectedAnimation = DOTween.Sequence();

        selectedAnimation.Append(transform.DOScale(0.85f, 0.3f));
        selectedAnimation.Append(transform.DOScale(1f, 0.3f));
        selectedAnimation.SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
        selectedAnimation.Play();

    }

    public void OnDeselected()
    {
        Debug.Log("DESELECTED");
        selectedAnimation.Kill();
    }

   

    public void OnPointerDown(PointerEventData eventData)
    {

        GemBehavior gem = LevelController.Instance.currentSelectedGem;
        if(gem == null)
        {
           
            LevelController.Instance.currentSelectedGem = this;
            OnSelected();
            return;
        }

        if(gem == this)
        {
          

            LevelController.Instance.currentSelectedGem = null;
            OnDeselected();
            return;
        }

        if(gem != this && gem != null)
        {
            LevelController.Instance.currentDestinationGem = this;    


            if (LevelController.Instance.CheckAdjacent(gem.gemData.GetPositionInGrid(), this.gemData.GetPositionInGrid()))
            {
                LevelController.Instance.SwapGemAnimation();
            }

            //Debug swap
            return;
        }

       
    }

    public void OnGemDestroy()
    {
        particlePrefab.GetComponent<ParticleSystem>().textureSheetAnimation.SetSprite(0, sprite);
        Instantiate(particlePrefab, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    #endregion

}


