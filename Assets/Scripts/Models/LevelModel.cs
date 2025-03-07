using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using System.IO;
using System.Linq;

public class LevelModel {
    public int level;

    //board model property
    public BoardModel boardModel;
    public List<TargetModel> targets;


    public int moves;


    #region Initialize level data
    public LevelModel(int level)
    {
        targets = new List<TargetModel>();

        string path = Application.dataPath + "/levels/level" + level + ".tmx";
        LoadTXM(path);

    }
    #endregion


    #region XML handling
    public void LoadTXM(string path)
    {
        boardModel = new BoardModel();




        string textContent = File.ReadAllText(path);


        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(textContent);


        //Reading the map node property
        XmlNode mapNode = xmlDoc.SelectSingleNode("map");

        if (mapNode != null)
        {

            //Setting board width, height, initialize the gem array
            int boardWidth = int.Parse(mapNode.Attributes["width"].Value);
            int boardHeight = int.Parse(mapNode.Attributes["height"].Value);

            boardModel.SetBoardSize(boardWidth, boardHeight);



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
                boardModel.SetBoardData(parseCSV(csvData, boardModel.boardWidth, boardModel.boardHeight));
            }
            else
            {
                Debug.Log("There is something wrong while reading the tile layer in xml");
            }


            //reding other properties
            XmlNode propertiesNode = mapNode.SelectSingleNode("properties");

            List<string> targetValues = new List<string>();
            List<string> targetQuantity = new List<string>();

            foreach (XmlNode property in propertiesNode.SelectNodes("property"))
            {
                string propName = property.Attributes["name"].Value;
                string propValue = property.Attributes["value"].Value;



                switch (property.Attributes["name"].Value)
                {
                    case "Move": moves = int.Parse(property.Attributes["value"].Value); break;
                    case "targets":
                        targetValues = property.Attributes["value"].Value.Split(',').ToList(); ;
                        break;
                    case "quantity":
                        targetQuantity = property.Attributes["value"].Value.Split(",").ToList();
                        break;
                    default: break;
                }
            }

            for (int i = 0; i < targetValues.Count; i++)
            {
                TargetModel targetModel = new TargetModel();
                targetModel.id = int.Parse(targetValues[i]);
                targetModel.number = int.Parse(targetQuantity[i]);
                targets.Add(targetModel);
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
            for (int j = 0; j < width; j++)
            {
                gems[i, j] = int.Parse(ids[i * width + j]);
            }
        }

        return gems;
    }

    #endregion


    #region Getting and Setting
    public BoardModel GetCurrentBoardModel()
    {
        return boardModel;
    }

    public void SetCurrentBoardModel(BoardModel boardModel)
    {
        this.boardModel = boardModel;
    }

    public List<TargetModel> GetCurrentTarget()
    {
        return targets;
    }

    public void SetCurrentTargets(List<TargetModel> targets)
    {
        this.targets = targets;
    }
    #endregion




}
