using UnityEngine;
using UnityEngine.UI;

public enum SliceDir
{
    UP,
    DOWN,
    LEFT,
    RIGHT
}

public enum Level
{
    LV1,
    LV2,
    LV3
}

[System.Serializable]
public class Grid4
{
    public Vector3[] matrixPos = new Vector3[16];
    public GameObject[] obj = new GameObject[16];

}
public class LevelManager : MonoBehaviour
{
    public Grid4[,] grid = new Grid4[4, 4];
    public Level level;
    public GameManager gameManager;

    public GameObject cubeTest;
    public GameObject cubeBun;
    public GameObject cubeMeat;
    public GameObject cubeCheddar;
    public GameObject cubeLettuce;


    public Transform baseGrid;
    public float distance = 1.5f;
    public float height = 0.5f;
    Vector2 prevMove, lastMove;

    Vector2 lastColumn;

    private void Start()
    {
        GridInit();
        switch (level)
        {
            case Level.LV1:
                FoodInitLv1();
                break;

            case Level.LV2:
                FoodInitLv2();
                break;

            case Level.LV3:
                FoodInitLv3();
                break;

        }
    }
    public void GridInit()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                grid[i, j] = new Grid4();
                for (int k = 0; k < 16; k++)
                {
                    grid[i, j].matrixPos[k] = new Vector3(baseGrid.transform.position.x + distance * i, baseGrid.transform.position.y + height * k, baseGrid.transform.position.z + distance * j);
                    //inizializza matrix 
                }
            }
        }
    }
    public void GridObjDestroy()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {

                for (int k = 0; k < 16; k++)
                {

                    Destroy(grid[i, j].obj[k]);
                }
            }

        }
    }
    public void FirstLayerInIt()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                Instantiate(cubeTest, grid[i, j].matrixPos[0], Quaternion.identity);
            }
        }
        //mostra layer base
    }

    //volevo farlo meglio ma ho avuto pochissimo tempo a disposizione
    public void FoodInitLv1()
    {
        grid[1, 0].obj[0] = Instantiate(cubeBun, grid[1, 0].matrixPos[0], Quaternion.identity);
        grid[1, 1].obj[0] = Instantiate(cubeBun, grid[1, 1].matrixPos[0], Quaternion.identity);
        grid[0, 1].obj[0] = Instantiate(cubeMeat, grid[0, 1].matrixPos[0], Quaternion.identity);
        grid[2, 1].obj[0] = Instantiate(cubeLettuce, grid[2, 1].matrixPos[0], Quaternion.identity);
        grid[1, 2].obj[0] = Instantiate(cubeCheddar, grid[1, 2].matrixPos[0], Quaternion.identity);
    }
    public void FoodInitLv2()
    {
        grid[2, 2].obj[0] = Instantiate(cubeBun, grid[2, 2].matrixPos[0], Quaternion.identity);
        grid[1, 2].obj[0] = Instantiate(cubeBun, grid[1, 2].matrixPos[0], Quaternion.identity);
        grid[0, 2].obj[0] = Instantiate(cubeMeat, grid[0, 2].matrixPos[0], Quaternion.identity);
        grid[2, 1].obj[0] = Instantiate(cubeMeat, grid[2, 1].matrixPos[0], Quaternion.identity);
        grid[1, 3].obj[0] = Instantiate(cubeLettuce, grid[1, 3].matrixPos[0], Quaternion.identity);
        grid[1, 1].obj[0] = Instantiate(cubeCheddar, grid[1, 1].matrixPos[0], Quaternion.identity);
    }
    public void FoodInitLv3()
    {
        grid[3, 2].obj[0] = Instantiate(cubeBun, grid[3, 2].matrixPos[0], Quaternion.identity);
        grid[2, 2].obj[0] = Instantiate(cubeBun, grid[2, 2].matrixPos[0], Quaternion.identity);
        grid[1, 0].obj[0] = Instantiate(cubeMeat, grid[1, 0].matrixPos[0], Quaternion.identity);
        grid[1, 1].obj[0] = Instantiate(cubeMeat, grid[1, 1].matrixPos[0], Quaternion.identity);
        grid[2, 3].obj[0] = Instantiate(cubeMeat, grid[2, 3].matrixPos[0], Quaternion.identity);
        grid[0, 1].obj[0] = Instantiate(cubeLettuce, grid[0, 1].matrixPos[0], Quaternion.identity);
        grid[3, 3].obj[0] = Instantiate(cubeLettuce, grid[3, 3].matrixPos[0], Quaternion.identity);
        grid[2, 1].obj[0] = Instantiate(cubeCheddar, grid[2, 1].matrixPos[0], Quaternion.identity);
        grid[1, 2].obj[0] = Instantiate(cubeCheddar, grid[1, 2].matrixPos[0], Quaternion.identity);
    }

    public bool IsLastColumn()
    {
        int remaining = 0;
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (grid[i, j].obj[0] != null)
                {
                    lastColumn = new Vector2(i, j);
                    remaining++;
                }
            }
        }
        if (remaining > 1)
        {
            //Debug.Log("più di una colonna rimasta");
            return false;
        }
        else
        {
            //Debug.Log("rimasta solo una colonna");
            return true;
        }

    }

    public void SliceMovement(SliceDir dir, string id)
    {
        Vector2 gridPos = new Vector2(0, 0);
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                foreach (GameObject a in grid[i, j].obj)
                {
                    if (a != null && a.GetComponent<SliceRef>().id == id)
                    {

                        gridPos = new Vector2(i, j);
                        break;
                    }
                }
            }
        }
        switch (dir)
        {
            case SliceDir.UP:

                Passage(0, 1, gridPos);
                break;

            case SliceDir.DOWN:
                Passage(0, -1, gridPos);
                break;

            case SliceDir.LEFT:
                Passage(-1, 0, gridPos);
                break;

            case SliceDir.RIGHT:
                Passage(1, 0, gridPos);
                break;
        }
        if (IsLastColumn())
        {
            CheckHamburger();

        }
    }

    public void Passage(int gridX, int gridY, Vector2 gridPos)
    {
        if (grid[(int)gridPos.x + gridX, (int)gridPos.y + gridY].obj[0] != null)
        {

            for (int i = 15; i >= 0; i--)
            {

                if (grid[(int)gridPos.x, (int)gridPos.y].obj[i] != null)
                {

                    //tp
                    for (int j = 0; j < 16; j++)
                    {
                        if (grid[(int)gridPos.x + gridX, (int)gridPos.y + gridY].obj[j] == null)
                        {

                            
                            grid[(int)gridPos.x + gridX, (int)gridPos.y + gridY].obj[j] = Instantiate(grid[(int)gridPos.x, (int)gridPos.y].obj[i]);


                            
                            grid[(int)gridPos.x + gridX, (int)gridPos.y + gridY].obj[j].transform.position = grid[(int)gridPos.x + gridX, (int)gridPos.y + gridY].matrixPos[j];
                            prevMove = new Vector2(grid[(int)gridPos.x, (int)gridPos.y].matrixPos[i].x, grid[(int)gridPos.x, (int)gridPos.y].matrixPos[i].y);
                            lastMove = new Vector2(grid[(int)gridPos.x + gridX, (int)gridPos.y + gridY].matrixPos[i].x, grid[(int)gridPos.x + gridX, (int)gridPos.y + gridY].matrixPos[i].y);
                            Destroy(grid[(int)gridPos.x, (int)gridPos.y].obj[i]);

                            grid[(int)gridPos.x, (int)gridPos.y].obj[i] = null;
                            grid[(int)gridPos.x + gridX, (int)gridPos.y + gridY].obj[j].GetComponent<SliceRef>().id = grid[(int)gridPos.x + gridX, (int)gridPos.y + gridY].obj[j].GetComponent<SliceRef>().id;

                            break;
                        }
                    }
                    //return;

                }
            }
            //controllo board
        }
        else
        {
            //Debug.Log("niente adiacente in sta direzione");
        }
    }

    public void CheckHamburger()
    {
        //controllo obj array più alto e il più basso se sono bun
        for (int i = 15; i >= 0; i--)
        {
            if (grid[(int)lastColumn.x, (int)lastColumn.y].obj[i] != null)
            {
                if (grid[(int)lastColumn.x, (int)lastColumn.y].obj[i].GetComponent<SliceRef>().foodType == FoodType.BUN)
                {

                    //Debug.Log("fetta in alto pane");
                    if (grid[(int)lastColumn.x, (int)lastColumn.y].obj[0] != null)
                    {
                        if (grid[(int)lastColumn.x, (int)lastColumn.y].obj[0].GetComponent<SliceRef>().foodType == FoodType.BUN)
                        {
                            //Debug.Log("fetta in basso panwe");
                            //win
                            gameManager.GoNextLevel();
                        }
                        else
                        {
                            
                        }
                    }

                }
                else
                {
                    //Debug.Log("no pane");
                }
                return;
            }
        }
    }

    public void Restart()
    {
        GridObjDestroy();
        //prima destroy tutto 
        Debug.Log(level);
        //GridInit();
        switch (level)
        {
            case Level.LV1:
                FoodInitLv1();
                break;

            case Level.LV2:
                FoodInitLv2();
                break;

            case Level.LV3:
                FoodInitLv3();
                break;

        }
    }

    //public void GoBackByOne()
    //{

    //    //salvo l'ultimo obj mosso, salvo dov'era e salvo dov'è
    //    //    grid[(int)gridPos.x + gridX, (int)gridPos.y + gridY].obj[j] = Instantiate(grid[(int)gridPos.x, (int)gridPos.y].obj[i]);
    //    //    grid[(int)gridPos.x + gridX, (int)gridPos.y + gridY].obj[j].transform.position = grid[(int)gridPos.x + gridX, (int)gridPos.y + gridY].matrixPos[j];
    //}

}

