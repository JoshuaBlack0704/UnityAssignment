using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


public class MainMenuEvents : MonoBehaviour
{

    public Button enterAssembler;
    public Button exit;
    public AudioSource clickSound;
    
    // Start is called before the first frame update
    void Start()
    {
        clickSound = GameObject.Find("Clicker").GetComponent<AudioSource>();
        var root = GetComponent<UIDocument>().rootVisualElement;
        enterAssembler = root.Q<Button>("Assembler");
        enterAssembler.clicked += EnterAssembler;
        exit = root.Q<Button>("Exit");
        exit.clicked += Exit;
    }
    
    void EnterAssembler()
    {
        StartCoroutine(Click("Scenes/Assembler/Assembler"));
    }
    
    
    void Exit()
    {
        StartCoroutine(Click());
    }

    IEnumerator Click(string name)
    {
        clickSound.PlayOneShot(clickSound.clip, 1);
        yield return new WaitForSeconds(clickSound.clip.length);
        SceneManager.LoadScene(name);
    }
    IEnumerator Click()
    {
        clickSound.PlayOneShot(clickSound.clip, 1);
        yield return new WaitForSeconds(clickSound.clip.length);
        Application.Quit();
    }
    
}
