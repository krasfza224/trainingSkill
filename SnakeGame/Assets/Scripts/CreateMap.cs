using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class CreateMap : MonoBehaviour
{

    public Camera mainCamera;
    public GameObject sizeMapContent;
    public GameObject block;
    GameObject head;
    public Material headMaterial, tailMaterial;
    GameObject food;
    bool isAlive;
    List<GameObject> tail;

    GameObject showInPutSizeMap;
    public int scoreCount;
    public TMP_Text result;
    public TMP_Text scorePointText;

    //public GameObject sizeMapContent;
    public int maxWidth;
    public int maxHight;

    public Color color1;
    public Color color2;

    GameObject mapObject;
    SpriteRenderer mapRenderer;


    public TMP_InputField sizeMapX;
    public TMP_InputField sizeMapY;

  


    public int xSize;
    public int ySize;

    Vector2 dir;

    public Button restartButton;
    public Button createSizeButton;
 

    private void Start()
    {
        //timeBetweenMovement = 0.5f;
        ////drawMap();
        //dir = Vector2.right;
        //setMap();
        //createPlayer();
        //spawnFood();
        //block.SetActive(false);
        //isAlive = true;


        //StartCoroutine(DrawMap());

        mainCamera.orthographicSize = 10;

        restartButton.onClick.AddListener(restart);
        createSizeButton.onClick.AddListener(() =>
        {
            calculateSizeMap();
        });
    }
    private void calculateSizeMap()
    {
        sizeMapContent.SetActive(false);
        int SX = Convert.ToInt32(sizeMapX.text);
        int SY = Convert.ToInt32(sizeMapX.text);
        getSizeMap(SX, SY);

    }


    private void getSizeMap(int x, int y)
    {
        xSize = x;
        ySize = y;
        startGame();
    }



    private void startGame()
    {
        timeBetweenMovement = 0.5f;
        //drawMap();
        dir = Vector2.right;
        setMap();
        createPlayer();
        spawnFood();
        block.SetActive(false);
        isAlive = true;

    }

    private Vector2 getRandomPos()
    {
        return new Vector2(Random.Range(-xSize / 2 + 1, xSize / 2), Random.Range(-ySize / 2 + 1, ySize / 2));
    }

    private bool containedInSnake(Vector2 spawnPos)
    {
        bool isInHead = spawnPos.x == head.transform.position.x && spawnPos.y == head.transform.position.y;
        bool isIntail = false;

        foreach(var item in tail)
        {
            if(item.transform.position.x == spawnPos.x && item.transform.position.y == spawnPos.y)
            {
                isIntail = true;
            }
        }
        return isInHead || isIntail;
    }


    private void createPlayer()
    {
        head = Instantiate(block) as GameObject;
        head.GetComponent<MeshRenderer>().material = headMaterial;
        head.SetActive(true);
        tail = new List<GameObject>();

    }

    private void spawnFood()
    {
        Vector2 spawnPos = getRandomPos();
        while (containedInSnake(spawnPos))
        {
            spawnPos = getRandomPos();
        }

        food = Instantiate(block);
        food.transform.position = new Vector3(spawnPos.x, spawnPos.y, 0);
        food.SetActive(true);
    }
         

    private void setMap()
    {
        for(int x =0;x <= xSize; x++)
        {
            GameObject borderBottom = Instantiate(block) as GameObject;
            borderBottom.GetComponent<Transform>().position = new Vector3(x - xSize / 2, -ySize/2, 0);
            borderBottom.SetActive(true);

            GameObject borderTop = Instantiate(block) as GameObject;
            borderTop.GetComponent<Transform>().position = new Vector3(x - xSize / 2, ySize - ySize/2, 0);
            borderTop.SetActive(true);
        }

        for (int y = 0; y <= ySize; y++)
        {
            GameObject borderRight = Instantiate(block) as GameObject;
            borderRight.GetComponent<Transform>().position = new Vector3( -xSize / 2, y-(ySize/2), 0);
            borderRight.SetActive(true);

            GameObject borderLeft = Instantiate(block) as GameObject;
            borderLeft.GetComponent<Transform>().position = new Vector3(xSize - (xSize / 2), y - (ySize/2), 0);
            borderLeft.SetActive(true);
        }

       // startGame();
    }

    float passedTime, timeBetweenMovement;
    public GameObject gameOverUI;

    private void gameOver()
    {
        isAlive = false;
        gameOverUI.SetActive(true);
        result.text = scoreCount.ToString();

    }


    public void restart()
    {
        SceneManager.LoadScene(0);
      
    }

     void Update()
    {
        if (Input.GetKey(KeyCode.DownArrow))
        {
            dir = Vector2.down;
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            dir = Vector2.up;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            dir = Vector2.left;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            dir = Vector2.right;
        }

        if (xSize == 0 && ySize == 0)
        {
          

        }
        else
        {
            passedTime += Time.deltaTime;
            if (timeBetweenMovement < passedTime && isAlive)
            {
                passedTime = 0;
                Vector3 newPosition = head.GetComponent<Transform>().position + new Vector3(dir.x, dir.y, 0);

                if (newPosition.x >= xSize / 2)
                {
                    newPosition.x = -xSize / 2 + 1;
                }
                else if (newPosition.x <= -xSize / 2)
                {
                    newPosition.x = (xSize / 2) - 1;
                }
                else if (newPosition.y >= ySize / 2)
                {
                    newPosition.y = -ySize / 2 + 1;
                }
                else if (newPosition.y <= -ySize / 2)
                {
                    newPosition.y = (ySize / 2) - 1;
                }


                //Check if head eat tail mean 
                foreach (var item in tail)
                {
                    if (item.transform.position == newPosition)
                    {
                        gameOver();

                    }
                }

                if (newPosition.x == food.transform.position.x && newPosition.y == food.transform.position.y) // check growing snake after 
                {
                    GameObject newTile = Instantiate(block);
                    newTile.SetActive(true);
                    newTile.transform.position = food.transform.position;
                    DestroyImmediate(food);
                    head.GetComponent<MeshRenderer>().material = tailMaterial;
                    tail.Add(head);
                    head = newTile;
                    head.GetComponent<MeshRenderer>().material = headMaterial;

                    scorePointText.text = "Point: " + tail.Count;
                    scoreCount += 1;
                    spawnFood();
                    AddSpeedMovement();

                }

                if (tail.Count == 0)
                {
                    head.transform.position = newPosition;
                }
                else
                {
                    head.GetComponent<MeshRenderer>().material = tailMaterial;
                    tail.Add(head);
                    head = tail[0];
                    head.GetComponent<MeshRenderer>().material = headMaterial;
                    tail.RemoveAt(0);
                    head.transform.position = newPosition;
                }
            }
        }


    }

    private void AddSpeedMovement()
    {
       if(tail.Count > 5)
        {
            timeBetweenMovement = 0.4f;
        }
       
       if (tail.Count > 10)
        {
            timeBetweenMovement = 0.35f;
        }

        if (tail.Count > 15)
        {
            timeBetweenMovement = 0.2f;
        }

        if (tail.Count > 20)
        {
            timeBetweenMovement = 0.1f;
        }

        if (tail.Count > 35)
        {
            timeBetweenMovement = 0.07f;
        }
    }


    IEnumerator DrawMap()
    {
        mapObject = new GameObject("Map");
        mapRenderer = mapObject.AddComponent<SpriteRenderer>();

        Texture2D txt = new Texture2D(maxWidth, maxHight);
        for (int x = 0; x < maxWidth; x++)
        {
            for (int y = 0; y < maxHight; y++)
            {
                if (x % 2 != 0)
                {
                    if (y % 2 != 0)
                    {
                        txt.SetPixel(x, y, color1);
                    }
                    else
                    {
                        txt.SetPixel(x, y, color2);
                    }
                }
                else
                {
                    if (y % 2 != 0)
                    {
                        txt.SetPixel(x, y, color2);
                    }
                    else
                    {
                        txt.SetPixel(x, y, color1);
                    }
                }
                
            }
            txt.filterMode = FilterMode.Point;
            txt.Apply();
            Rect rect = new Rect(0, 0, maxWidth, maxHight);
            Sprite sprite = Sprite.Create(txt, rect, Vector2.one * 0.5f, 1, 0, SpriteMeshType.FullRect);
            mapRenderer.sprite = sprite;
            yield return new WaitForSeconds(1f);
        }
      
    }

    //public void drawMap()
    //{
    //    mapObject = new GameObject("Map");
    //    mapRenderer = mapObject.AddComponent<SpriteRenderer>();

    //    Texture2D txt = new Texture2D(maxWidth, maxHight);
    //    for (int x = 0; x < maxWidth; x++)
    //    {
    //        for (int y = 0; y < maxHight; y++)
    //        {
    //            if (x % 2 != 0)
    //            {
    //                if (y % 2 != 0)
    //                {
    //                    txt.SetPixel(x, y, color1);
    //                }
    //                else
    //                {
    //                    txt.SetPixel(x, y, color2);
    //                }
    //            }
    //            else
    //            {
    //                if (y % 2 != 0)
    //                {
    //                    txt.SetPixel(x, y, color2);
    //                }
    //                else
    //                {
    //                    txt.SetPixel(x, y, color1);
    //                }
    //            }
    //        }
    //        txt.filterMode = FilterMode.Point;
    //        txt.Apply();
    //        Rect rect = new Rect(0, 0, maxWidth, maxHight);
    //        Sprite sprite = Sprite.Create(txt, rect, Vector2.one * 0.5f,1,0,SpriteMeshType.FullRect);
    //        mapRenderer.sprite = sprite;
    //    }
    //}
}
