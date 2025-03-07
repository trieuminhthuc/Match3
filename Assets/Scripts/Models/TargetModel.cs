using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetModel {
    public int id;
    public string target_path;
    public int quantity;

    public TargetModel(int id, int quantity)
    {
        this.id = id;
        this.quantity = quantity;
        target_path = ($"Sprites/Candy/{id}");
    }



}
