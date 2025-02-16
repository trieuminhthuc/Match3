using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemData 
{
    public int GemId;

    private int rowIdx;
    private int colIdx;

    
    
    public GemData(int GemId, int row, int col)
    {
        this.GemId = GemId;
        this.rowIdx = row;
        this.colIdx = col;
    }

    public int GetRowIdx()
    {
        return rowIdx;
    }

    public int GetColIdx() 
    {
        return colIdx;
    }

    public void SetRowIdx(int rowIdx) 
    {
        this.rowIdx = rowIdx;
    }

    public void SetColIdx(int colIdx)
    {
        this.colIdx = colIdx;
    }






}
