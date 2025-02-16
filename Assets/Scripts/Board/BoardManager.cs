using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
    #region Class Definition

    private BoardData boardData;

    private GemBehavior[,] gemsView;
    private int[,] currentBoard;

    //
    public Tilemap baseBoardTiles;
    public GameObject GemPrefab;

    public Transform gridTransform;
    public Transform baseTransform;

    List<Vector2Int> matchedPosition;

    public BoardModel boardModel;




    #endregion


    private void Awake()
    {

       // boardModel = new BoardModel(1);
      //  board.PrintGemsArray(board.gemArray, board.boardWidth, board.boardHeight);


     //   Debug.Log(board.gemArray.ToString());

        boardData = new BoardData();
        boardData.initialBoardData();
        //boardData.boardData = board.gemArray;
        currentBoard = boardData.GetBoardData();

        

     //   InitialBaseSprite();
        InitialGem();
        ReCenterGrid();

        
    }

    #region InitView

    public void InitialBaseSprite()
    {
        Sprite baseSprite = Resources.Load<Sprite>("Sprites/UI/6");

        for (int i = 0; i < boardData.numberOfRows; i++) {
            for (int j = 0; j < boardData.numberOfCols; j++) {
                Tile tile = new Tile();
                tile.sprite = baseSprite;
                tile.name = $"[{i} , {j}";

                baseBoardTiles.SetTile(new Vector3Int(i, j, 0), tile);

               
            }
        }

       
    }


    public void InitialGem()
    {
        for (int i = 0; i < boardModel.boardWidth; i++)
        {
            for (int j = 0; j < boardModel.boardHeight; j++)
            {

               

                GemData data = new GemData(boardModel.gemArray[i, j], i, j);
                GemBehavior gem = GemPrefab.GetComponent<GemBehavior>();
                gem.SetGemData(data);
                gem.InitialGem();
                gem.gameObject.name = $"[{i} , {j}, Value: {boardModel.gemArray[i,j]}]";
                GameObject ga =  Instantiate(GemPrefab, gridTransform);
                
                gem.transform.localRotation = Quaternion.identity;
                gem.transform.localScale = Vector3.one;
                gem.CreateBaseSprite(ga.transform);

            //    gemsView[i, j] = gem;

            }
        }

                ReCenterGrid();


        //  CenterGrid.Instance.ReCenterGrid(gridTransform,boardData.numberOfRows, boardData.numberOfCols, GemConfig.ROW_PADDING);
     //   CenterGrid.Instance.ReCenterGrid(CenterGrid.Instance.baseContainer,boardData.numberOfRows, boardData.numberOfCols, GemConfig.ROW_PADDING);
    }

    public void ReCenterGrid()
    {   



        float totalHeight =  (boardData.numberOfRows - 1) * GemConfig.TILE_WIDTH;
        float totalWidth = (boardData.numberOfCols - 1) * GemConfig.TILE_HEIGHT;

        Vector3 offset = new Vector3(totalWidth / 4 , totalHeight / 4 + 30f , 0);

        gridTransform.position -= offset;
        baseTransform.position -= offset;

     
    }

    #endregion


    #region Logic Handling Matching


    public void CheckMatch(int row, int col, int gemId)
    {
        
    }

    public void CheckRight(int row, int col, int gemId)
    {

    }

    public void CheckLeft(int row, int col, int gemId)
    {

    }

    public void CheckUpper(int row, int col, int gemId)
    {

    }

    public void CheckDown(int row, int col, int gemId)
    {

    }

    #endregion



}
