using DG.Tweening;

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public static LevelController Instance;

    public LevelModel levelModel;
    [SerializeField]
    public GameObject GemPreFab;

    public GemBehavior[,] viewGems;
    public int[,] viewBoard;
    public GemBehavior currentSelectedGem;
    public GemBehavior currentDestinationGem;

    private Vector3 boardOffSet;


    List<GemBehavior> matchedGems = new List<GemBehavior>();
    List<GemBehavior> matchTmp = new List<GemBehavior>();
    List<GemBehavior> gemToMove = new List<GemBehavior>();
    
    public TextMeshProUGUI txt_move;


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        currentSelectedGem = null;
        currentDestinationGem = null;

        matchedGems.Clear();  
        matchTmp.Clear();
        

        

        InitLevelModel();

        float x = viewBoard.GetLength(1) * GemConfig.TILE_WIDTH;
        float y = viewBoard.GetLength(0) * GemConfig.TILE_HEIGHT;

        //viewGems = new List<GemBehavior>();
      //  viewGems.Clear();

        viewGems = new GemBehavior[viewBoard.GetLength(0), viewBoard.GetLength(1)];

        boardOffSet = new Vector3 (x / 2 - GemConfig.TILE_WIDTH / 2, 0, 0);
        Debug.Log($"board off set x: {boardOffSet.x}, y: {boardOffSet.y}");

        InitLevelView();
        InitialUIProperty();

    }



    public void test()
    {
     
    }






    #region Initialize function
    public void InitLevelModel()
    {
        int level = PlayerPrefs.GetInt(GemConfig.CURRENT_SELECT_LEVEL, 1);

        if(level <= 0)
        {
            level = 1;
            PlayerPrefs.SetInt(GemConfig.PLAYER_LEVEL_KEY, level);
            PlayerPrefs.SetInt(GemConfig.CURRENT_SELECT_LEVEL, level);
            PlayerPrefs.Save();
        }

        //level = 2;
        levelModel = new LevelModel(level);
        Debug.Log("target list: " + levelModel.targets.Count);
        viewBoard = levelModel.GetCurrentBoardModel().gemArray;
    }

    public void InitLevelView()
    {
   
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
                viewGems[row, col] = gem;
            }
        }
    }


    public void InitialUIProperty()
    {
        txt_move.text = levelModel.moves.ToString();

        if (UIManager.instance == null)
        {
            Debug.LogError("UIManager instance is NULL!");
            return;
        }
        UIManager.instance.LoadTarget(levelModel.targets);
    }

    #endregion


    #region Swaping Logic Handling
 

   
    public void SwapBack()
    {
        Vector3 startPos = currentSelectedGem.transform.position;
        Vector3 endPos = currentDestinationGem.transform.position;

        Vector2Int startIdx = new Vector2Int(currentSelectedGem.gemData.GetRowIdx(), currentSelectedGem.gemData.GetColIdx());
        Vector2Int endIdx = new Vector2Int(currentDestinationGem.gemData.GetRowIdx(), currentDestinationGem.gemData.GetColIdx());

        //swaping the game object first
        currentSelectedGem.transform.DOMove(endPos, 0.5f).OnComplete(() => currentSelectedGem.transform.DOKill());
        currentDestinationGem.transform.DOMove(startPos, 0.5f).OnComplete(() => currentDestinationGem.transform.DOKill());

        // swaping the row col idx of the game object
        currentSelectedGem.gemData.SetPositionInGrid(endIdx);
        currentDestinationGem.gemData.SetPositionInGrid(startIdx);

        //swaping the value of the board model
        viewBoard[endIdx.x, endIdx.y] = currentSelectedGem.gemData.GemId;
        viewBoard[startIdx.x, startIdx.y] = currentDestinationGem.gemData.GemId;

        //swaping the value of the view gem
        viewGems[endIdx.x, endIdx.y] = currentSelectedGem;
        viewGems[startIdx.x, startIdx.y] = currentDestinationGem;



        //Killing the tween 
        currentSelectedGem.OnDeselected();
        currentSelectedGem = null;
        currentDestinationGem = null;
    }

    public void SwapGemAnimation()
    {
        PrintBoardToConsole();

        Vector3 startPos = currentSelectedGem.transform.position;
        Vector3 endPos = currentDestinationGem.transform.position;

        Vector2Int startIdx = new Vector2Int(currentSelectedGem.gemData.GetRowIdx(), currentSelectedGem.gemData.GetColIdx());
        Vector2Int endIdx = new Vector2Int(currentDestinationGem.gemData.GetRowIdx(), currentDestinationGem.gemData.GetColIdx());

        //swaping the game object first
        currentSelectedGem.transform.DOMove(endPos, 0.5f).OnComplete(() => currentSelectedGem.transform.DOKill());
        currentDestinationGem.transform.DOMove(startPos, 0.5f).OnComplete(() => currentDestinationGem.transform.DOKill());

        // swaping the row col idx of the game object
        currentSelectedGem.gemData.SetPositionInGrid(endIdx);
        currentDestinationGem.gemData.SetPositionInGrid(startIdx);
        
        //swaping the value of the board model
        viewBoard[endIdx.x, endIdx.y] = currentSelectedGem.gemData.GemId;
        viewBoard[startIdx.x, startIdx.y] = currentDestinationGem.gemData.GemId;

        //swaping the value of the view gem
        viewGems[endIdx.x, endIdx.y] = currentSelectedGem;
        viewGems[startIdx.x, startIdx.y] = currentDestinationGem;



        //Killing the tween 
        currentSelectedGem.OnDeselected();
        //  currentSelectedGem = null;
        // currentDestinationGem = null;


        Debug.Log("after swap: ==================== ");
        PrintBoardToConsole();

        StartCoroutine(AfterSwapSequence());


       

    }

    public void PrintBoardToConsole()
    {
        string boardString = "Current Board:\n";

        for (int row = 0; row < viewBoard.GetLength(0); row++)
        {
            for (int col = 0; col < viewBoard.GetLength(1); col++)
            {
                boardString += viewBoard[row, col] + " ";
            }
            boardString += "\n"; // New line after each row
        }

        Debug.Log(boardString);
    }





    #endregion


    #region Board moving, tiles effect function
    public IEnumerator AfterSwapSequence()
    {
        yield return new WaitForSeconds(0.5f); // wait for swap animation

        if (CheckMatch())
        {
            yield return new WaitForSeconds(0.15f); // delay before destroy
            RemoveMatchedGem();

            yield return new WaitForSeconds(0.2f); // 
            DropTileOnBoardModel();
            SpawnGem();
            MoveAllGem();
            yield return new WaitForSeconds(0.5f);

            if (CheckMatch())
            {
                StartCoroutine(AfterSwapSequence());
            }
            else
            {
                levelModel.moves--;
                UIManager.instance.UpdateUI(levelModel.moves, levelModel.targets);

                CheckGameOver();

            }
        }
        else
        {   
           
            SwapBack();
        }


    }


  
    #endregion


    #region Checking the board function


    public bool CheckAdjacent(Vector2Int posA, Vector2Int posB)
    {
        return (Mathf.Abs(posA.x - posB.x) == 1 && posA.y == posB.y) ||
              (Mathf.Abs(posA.y - posB.y) == 1 && posA.x == posB.x);
    }

    public bool CheckMatch()
    {
        bool rs = false;

        matchedGems.Clear();

        for (int i = 0; i < viewBoard.GetLength(0); i++)
        {
            for (int j = 0; j < viewBoard.GetLength(1); j++)
            {
                if (viewBoard[i, j] != 0) // Ignore empty spaces
                {
                    // Check horizontal matches
                    List<GemBehavior> horizontalMatch = new List<GemBehavior>();
                    CheckHorizontalMatch(i, j, viewBoard[i, j], horizontalMatch);

                    if (horizontalMatch.Count >= 3)
                    {

                        matchedGems.AddRange(horizontalMatch);
                        
                    }

                    // Check vertical matches
                    List<GemBehavior> verticalMatch = new List<GemBehavior>();
                    CheckVerticalMatch(i, j, viewBoard[i, j], verticalMatch);

                    if (verticalMatch.Count >= 3)
                    {
                        matchedGems.AddRange(verticalMatch);
                    }

                   
                }
            }
        }

        // Remove matched gems
        if (matchedGems.Count > 0)
        {

            Debug.Log("matchedgemms: " + matchedGems.Count);
            rs = true;
        }
        return rs;
    }

    public void RemoveMatchedGem()
    {

        HashSet<GemBehavior> uniqueGems = new HashSet<GemBehavior>(matchedGems);
        matchedGems.Clear();
        matchedGems.AddRange(uniqueGems);

        foreach (GemBehavior gem in matchedGems)
        {

            for (int i = 0; i < levelModel.targets.Count; i++) {
                if (levelModel.targets[i].id == gem.gemData.GemId)
                {
                    levelModel.targets[i].quantity--;
                    Debug.Log("tru tru");
                }
            }

            viewBoard[gem.gemData.GetRowIdx(), gem.gemData.GetColIdx()] = 0; // Mark as empty
            viewGems[gem.gemData.GetRowIdx(), gem.gemData.GetColIdx()] = null;
             gem.OnGemDestroy();
        }

        currentDestinationGem = null;
        currentSelectedGem = null;
        matchedGems.Clear();
    }


    public void CheckMatchRecursive(int row, int col, int id, List<GemBehavior> matches, int xDir, int yDir)
    {
 

        if (row < 0 || row >= viewBoard.GetLength(0) || col < 0 || col >= viewBoard.GetLength(1)) return;

        int value = viewBoard[row, col];
        Debug.Log("======Value: " + value);

        GemBehavior gem = viewGems[row, col];

        if (value == 0 || matchTmp.Contains(gem) || value != id)
        {
            Debug.Log("value is different");
            return;
        }

        matchTmp.Add(gem);
        Debug.Log($"=========Add id: {gem.gemData.GemId}");

        CheckMatchRecursive(row + xDir, col + yDir, value, matchTmp, xDir, yDir);
    }


    private void CheckVerticalMatch(int row, int col, int id, List<GemBehavior> matches)
    {
        if (row < 0 || row >= viewBoard.GetLength(0)) return;

        int value = viewBoard[row, col];
        GemBehavior gem = viewGems[row, col];

        if (value == 0 || matches.Any(g => g.gemData.GetRowIdx() == row && g.gemData.GetColIdx() == col) || value != id)
            return;


        matches.Add(gem);

        // Check up and down
        CheckVerticalMatch(row - 1, col, id, matches); // Up
        CheckVerticalMatch(row + 1, col, id, matches); // Down
    }

    private void CheckHorizontalMatch(int row, int col, int id, List<GemBehavior> matches)
    {
        if (col < 0 || col >= viewBoard.GetLength(1)) return;

        int value = viewBoard[row, col];
        GemBehavior gem = viewGems[row, col];

        if (value == 0 || matches.Any(g => g.gemData.GetRowIdx() == row && g.gemData.GetColIdx() == col) || value != id)
            return;

        matches.Add(gem);

        // Check left and right
        CheckHorizontalMatch(row, col - 1, id, matches); // Left
        CheckHorizontalMatch(row, col + 1, id, matches); // Right
    }

    #endregion

    #region Dropping tiles

    public void CheckGameOver()
    {
        bool rs = true;
        foreach(TargetModel target in levelModel.targets)
        {
            if(target.quantity > 0)
            {
                rs = false;
                break;
            }
        }

        if (rs) {
            Debug.Log("WIN");
            UIManager.instance.OnWinPanelOn();
        }

        if(levelModel.moves <= 0 && !rs)
        {
         
        }

    }


    public void DropTileOnBoardModel()
    {
        for(int col = 0; col < viewBoard.GetLength(1); col++)
        {
            for(int row = 0; row < viewBoard.GetLength(0); row++)
            {
                if(viewBoard[row, col] == 0 && row != (viewBoard.GetLength(0) - 1))
                {
                    for(int r = row + 1; r < viewBoard.GetLength(0); r++)
                    {
                        if (viewBoard[r, col] != 0)
                        {
                            Debug.Log($"found  at : [{r}, {col}");
                            viewBoard[row, col] = viewBoard[r, col];
                            viewBoard[r, col] = 0;


                            if (viewGems[r, col] == null)
                            {
                                Debug.LogError($"NULL VIEW GEM at {r}, {col}");
                                Debug.LogError($"But the value of the board is : {viewBoard[r, col]}");
                            }

                            viewGems[row, col] = viewGems[r, col];
                            viewGems[r, col] = null;

                          
                            if (viewGems[row, col] != null)
                            {
                                viewGems[row, col].gemData.SetPositionInGrid(new Vector2Int(row, col));
                                gemToMove.Add(viewGems[row, col]); // Add to movement list
                            }

                            break;

                        }
                    }
                }
            }
        }
    }

    public void SpawnGem()
    {
        for(int col = 0;col < viewGems.GetLength(1); col++)
        {
            

            for(int row = 0;row < viewGems.GetLength(0); row++)
            {
                if (viewBoard[row, col] == 0)
                {

                    int randomIdx = Random.Range(1, GemConfig.MAX_GEM_TYPE);
                    viewBoard[row, col] = randomIdx;


                    float x = col * GemConfig.TILE_WIDTH;
                    float y = (row * GemConfig.TILE_HEIGHT);
                    Vector3 pos = new Vector3(x, y, 1);

                    pos -= boardOffSet;




                    GameObject newGem = Instantiate(GemPreFab, pos, Quaternion.identity);
                    GemBehavior gem = newGem.GetComponent<GemBehavior>();

                    newGem.name = $"row {row}, col {col}";

                    GemData data = new GemData(viewBoard[row, col], row, col);
                    gem.SetGemData(data);
            


                    gem.gemData.SetPositionInGrid(new Vector2Int(row, col));
                    gem.InitialGem();
                    viewGems[row, col] = gem;
                    gemToMove.Add(gem);
                }
            }

         
        }
    }

    public void MoveAllGem()
    {
        foreach (GemBehavior gem in gemToMove)
        {   
            if(gem != null)
            {
                float x = gem.gemData.GetColIdx() * GemConfig.TILE_WIDTH;
                float y = -370 + (gem.gemData.GetRowIdx() * GemConfig.TILE_HEIGHT);
                Vector3 pos = new Vector3(x, y, 1);

                gem.gameObject.name = $"[{gem.gemData.GetRowIdx()}, col {gem.gemData.GetColIdx()}], value: {viewBoard[gem.gemData.GetRowIdx(), gem.gemData.GetColIdx()]}";

                pos -= boardOffSet;

                gem.transform.DOMove(pos, 0.5f);
            }
           

        }
        gemToMove.Clear();
    }
    #endregion


}
