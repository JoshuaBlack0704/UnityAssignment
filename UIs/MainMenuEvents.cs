using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


public class MainMenuEvents : MonoBehaviour
{

    public Button enterAssembler;
    public Button exit;
    
    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        enterAssembler = root.Q<Button>("Assembler");
        enterAssembler.clicked += EnterAssembler;
        exit = root.Q<Button>("Exit");
        exit.clicked += Exit;
    }

    void EnterAssembler()
    {
        SceneManager.LoadScene("Scenes/Assembler");
    }
    
    void Exit()
    {
        Application.Quit();
    }
}
