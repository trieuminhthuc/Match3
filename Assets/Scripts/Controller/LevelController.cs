using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public static LevelController Instance;

    public LevelModel levelModel;
    [SerializeField]
    public GameObject GemPreFab;

    public List<GemBehavior> viewGems;
    public int[,] viewBoard;


    private Vector3 boardOffSet;
        

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        viewGems = new List<GemBehavior>(); 
        viewGems.Clear();


        

        InitLevelModel();

        float x = viewBoard.GetLength(1) * GemConfig.TILE_WIDTH;
        float y = viewBoard.GetLength(0) * GemConfig.TILE_HEIGHT;

        boardOffSet = new Vector3 (x / 2 - GemConfig.TILE_WIDTH / 2, 0, 0);
        Debug.Log($"board off set x: {boardOffSet.x}, y: {boardOffSet.y}");

        InitLevelView();

    }

    #region Initialize function
    public void InitLevelModel()
    {
        int level = PlayerPrefs.GetInt("currentLevel", 1);
        levelModel = new LevelModel(level);
        viewBoard = levelModel.GetCurrentBoardModel().gemArray;
    }

    public void InitLevelView()
    {
        //initialize board first

        InitializeBoardView(viewBoard);


       
    }

    public void InitializeBoardView(int[,] board)
    {
        for (int row = 0; row < board.GetLength(0); row++) {
            for (int col = 0; col < board.GetLength(1); col++) {
                
                //calculate position
                float x = col * GemConfig.TILE_WIDTH;
                float y = -370 + (row * GemConfig.TILE_HEIGHT) ;
                Vector3 pos = new Vector3(x,   y, 1);

                GameObject gemGO = Instantiate(GemPreFab, pos, Quaternion.identity);
                gemGO.transform.position -= boardOffSet;
                gemGO.name = $"[{row}, col {col}], value: {board[row, col]}";
                GemBehavior gem = gemGO.GetComponent<GemBehavior>();
                GemData data = new GemData(board[row, col], row, col);
                gem.SetGemData(data);
                gem.InitialGem();
                viewGems.Add(gem);
            }
        }
    }


    public void InitialUIProperty()
    {

    }

    #endregion

}
