using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject block, wall, player1, player2;

    public TargetFollow cameraFollow;

    public Vector2 margin;
    public Vector2 size;

    public List<Block> blockList;

    public List<Transform> player1List;
    public List<Transform> player2List;

    [SerializeField]
    private int inputSelect = 1;

    public int InputSelect
    {
        get
        {
            return inputSelect;
        }
        set
        {
            inputSelect = value;
            inputSelect = inputSelect % 2;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start()
    {
        CreateBlocks();
    }


    void CreateBlocks()
    {
        for (int i = 0; i < size.y; i++)
        {
            for (int j = 0; j < size.x; j++)
            {
                GameObject g = Instantiate(block, transform.position + new Vector3(margin.x * j, 0, margin.y * i), Quaternion.identity);
                g.transform.parent = transform;
                if (i == 1 && (j + 1) % 2 == 0)
                {
                    GameObject p1 = Instantiate(player1, transform.position + new Vector3(margin.x * j, 1, margin.y * i), Quaternion.identity);
                    player1List.Add(p1.transform);
                }
                else if (i == (size.y - 2) && (j + 1) % 2 == 0)
                {
                    GameObject p2 = Instantiate(player2, transform.position + new Vector3(margin.x * j, 1, margin.y * i), Quaternion.identity);
                    player2List.Add(p2.transform);
                }
                else if (i > 1 && i < (size.y - 2) && (j) % 2 == 0)
                    CreateWalls(i, j);
            }
        }
    }

    void CreateWalls(int i, int j)
    {
        if (Random.Range(0, 100) < 40)
        {
            blockList.Add(new Block(j,i));
            GameObject g = Instantiate(wall, transform.position + new Vector3(margin.x * j, 1, margin.y * i), Quaternion.identity);
            g.transform.parent = transform;
        }
    }

    public void SetCameraTarget(int select,Transform _tr) {
        switch (select)
        {
            case 0:
                player1List.Remove(_tr);
                break;
            case 1:
                player2List.Remove(_tr);
                break;
        }

        switch (inputSelect)
        {
            case 0:
                cameraFollow.target = player1List[Random.Range(0, player1List.Count)];
                break;
            case 1:
                cameraFollow.target = player2List[Random.Range(0, player2List.Count)];
                break;
        }

        Destroy(_tr.gameObject);
    }

}

[System.Serializable]
public class Block
{
    public int x = 0;
    public int y = 0;

    public Block(int _x, int _y)
    {
        x = _x;
        y = _y;
    }
}
