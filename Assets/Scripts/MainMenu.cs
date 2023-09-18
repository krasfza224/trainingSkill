using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuContent;
    public GameObject setmapContent;
    public GameObject resultContent;
    public GameObject gameOverContent;
    public Button startButton; // start > setmap > game
    public Button resultButton;  // menu > lastResult
    public Button resultToMenuButton; // result > menu
    public Button setMapToMenuButton; // setmap to menu
    public Button backToMenuButton; // game > menu
  

    private void Start()
    {
        mainMenuContent.SetActive(true);
        startButton.onClick.AddListener(MenuToStart);
        resultButton.onClick.AddListener(MenuToResult);
        resultToMenuButton.onClick.AddListener(ResultToMenu);
        setMapToMenuButton.onClick.AddListener(SetMapToMenu);
        backToMenuButton.onClick.AddListener(BackToMenu);

    }


    public void MenuToStart()
    {
        setmapContent.SetActive(true);
        resultContent.SetActive(false);
        mainMenuContent.SetActive(false);
        gameOverContent.SetActive(false);
    }

    public void MenuToResult()
    {
        setmapContent.SetActive(false);
        resultContent.SetActive(true);
        mainMenuContent.SetActive(false);
        gameOverContent.SetActive(false);
    }


    public void ResultToMenu()
    {
        setmapContent.SetActive(false);
        resultContent.SetActive(false);
        mainMenuContent.SetActive(true);
        gameOverContent.SetActive(false);
    }

    public void SetMapToMenu()
    {
        setmapContent.SetActive(false);
        resultContent.SetActive(false);
        mainMenuContent.SetActive(true);
        gameOverContent.SetActive(false);
    }
    public void BackToMenu()
    {
        setmapContent.SetActive(false);
        resultContent.SetActive(false);
        mainMenuContent.SetActive(true);
        gameOverContent.SetActive(false);
    }

}
