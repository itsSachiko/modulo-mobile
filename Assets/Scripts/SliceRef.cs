using System;
using UnityEngine;
public enum FoodType
{
    MEAT,
    CHEDDAR,
    BUN,
    LETTUCE

}
public class SliceRef : MonoBehaviour, IInteractable
{
    public string id;

    public FoodType foodType;
    private void Awake()
    {
        id = Guid.NewGuid().ToString(); //che obj muovo
        //Debug.Log("ID: " + id);

    }
}
