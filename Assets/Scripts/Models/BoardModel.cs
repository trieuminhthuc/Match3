using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;

public class BoardModel {

    #region Class Defination
    public int boardWidth;
    public int boardHeight;
    public int[,] gemArray;
    public int level;
    public string filePath;
    #endregion


    #region Initialize func

    public BoardModel(int boardWidth, int boardHeight, int[,] gemArray) {

        this.boardWidth = boardWidth;
        this.boardHeight = boardHeight;
        this.gemArray = gemArray;
        
    }

    public BoardModel() {
    }

    public void GetBoardDataFromTmx()
    {
        LoadTXM(filePath);
        PrintGemsArray(gemArray, boardWidth, boardHeight);
    }

    public void GetBoardDataFromJson()
    {

    }
    #endregion


    #region XML handling
    public void LoadTXM(string path)
    {

       // TextAsset textAsset = Resources.Load<TextAsset>("levels/level"+level);
   

        string textContent = File.ReadAllText(path);


        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(textContent);


        //Reading the map node property
        XmlNode mapNode = xmlDoc.SelectSingleNode("map");

        if (mapNode != null) {
            //Setting board width, height, initialize the gem array
            boardWidth = int.Parse(mapNode.Attributes["width"].Value);
            boardHeight = int.Parse(mapNode.Attributes["height"].Value);
            gemArray = new int[boardWidth, boardHeight];
        }
        else
        {
            Debug.Log("Failed loading TMX file, maybe the path is incorrect");
        }

        //reading the array value in tmx and initilize
        //reading the layer first
        XmlNode layerNode = xmlDoc.SelectSingleNode("map/layer");
        if (layerNode != null)
        {
            string layerName = layerNode.Attributes["name"].Value;

            //reading the layer data CSV
            XmlNode dataNode = layerNode.SelectSingleNode("data");
            if (dataNode != null)
            {
                string csvData = dataNode.InnerText.Trim();
                gemArray = parseCSV(csvData, boardWidth, boardHeight);
            }
            else
            {
                Debug.Log("There is something wrong while reading the tile layer in xml");
            }

        }
        else
        {
            Debug.Log("There is something wrong while reading the  layer in xml");
        }
    }

    private int[,] parseCSV(string csv, int width, int height)
    {
        string[] ids = csv.Split(',');
        int[,] gems = new int[width, height];

        for (int i = 0; i < height; i++)
        {
            for(int j = 0; j < width; j++)
            {
                gems[i, j] = int.Parse(ids[i*width + j]);
            }
        }

        return gems;
    }


    public void PrintGemsArray(int[,] arr, int width, int height)
    {
        Debug.Log("Tile Map:");
        for (int y = 0; y < height; y++)
        {
            string row = "";
            for (int x = 0; x < width; x++)
            {
                row += arr[x, y] + " ";
            }
            Debug.Log(row);
        }
    }


    #endregion


    public void SetBoardSize(int w, int h)
    {
        this.boardWidth = w;
        this.boardHeight = h;
    }

    public void SetBoardData(int[,] data)
    {
        gemArray = data;
    }

}
