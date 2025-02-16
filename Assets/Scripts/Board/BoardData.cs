using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardData
{
    #region Class defination
    public int numberOfRows;
    public int numberOfCols;
    private List<GemData> gemDatas = new List<GemData>();



    public int[,] boardData =
        {
            {1, 1, 1, 1, 1, 2, 3 },
            {1, 2, 2, 2, 2, 2, 3 },
            {1, 3, 3, 3, 3, 2, 3 },
            {4, 4, 4, 4, 4, 2, 3 },
            {1, 1, 1, 1, 1, 2, 3 },
            {4, 4, 4, 4, 4, 2, 3 },
            {4, 4, 4, 4, 4, 2, 3 },
           
        };

    #endregion

    #region InitBoard Data

    public void initialBoardData()
    {

        numberOfRows = boardData.GetLength(0);
        numberOfCols = boardData.GetLength(1);


     


        for (int row = 0; row < numberOfRows; row++)
        {
            for (int col = 0; col < numberOfCols; col++)
            {
                GemData gemData = new GemData(boardData[row, col], row, col);
                gemDatas.Add(gemData);
            }
        }

     

       
    }


   

    #endregion

    #region Get Methods


    public List<GemData> GetGemDatas() { 
        return gemDatas;
    }

    public int[,] GetBoardData()
    {
        return boardData;
    }

    #endregion

}
