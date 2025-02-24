using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public InputSystem3DInteraction inputSystem;
    public LevelManager[] levels;
    int levelIndex = 0;
    public Animator animator;
    private void Start()
    {
        Debug.Log(levelIndex);
        inputSystem.levelManager = levels[levelIndex];
    }

    public void GoNextLevel()
    {
        if (levelIndex < levels.Length)
        {
            levelIndex++;
            if(levelIndex >= levels.Length)
            {
                Application.Quit();
                Debug.Log("quit");
                return;
            }
            inputSystem.levelManager = levels[levelIndex];
            animator.Play("camMove" + levelIndex.ToString());
        }
    }

    public void Reset()
    {
        levels[levelIndex].Restart();
    }

    //public void GoBack()
    //{
    //    levels[levelIndex].GoBackByOne();
    //}
}
