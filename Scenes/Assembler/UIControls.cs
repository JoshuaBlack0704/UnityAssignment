using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;



public class UIControls : MonoBehaviour
{
    private VisualElement SceneMenu;
    private VisualElement Controls;
    private Button enterSceneMenu;
    private Button resume;
    private Button toMainMenu;
    private Button exit;
    private Button next;
    private Button restart;
    private AudioSource audioPlayer;
    public AudioClip clickSound;
    
    // Start is called before the first frame update
    void Start()
    {
        Controls = GameObject.Find("Controls").GetComponent<UIDocument>().rootVisualElement;
        SceneMenu = GameObject.Find("MainMenu").GetComponent<UIDocument>().rootVisualElement;
        SceneMenu.style.display = DisplayStyle.None;
        enterSceneMenu = Controls.Q<Button>("EnterSceneMenu");
        resume = SceneMenu.Q<Button>("Resume");
        toMainMenu = SceneMenu.Q<Button>("BackToMainMenu");
        exit = SceneMenu.Q<Button>("Exit");
        next = Controls.Q<Button>("Next");
        enterSceneMenu.clickable.clicked += EnterSceneMenu;
        resume.clickable.clicked += Resume;
        toMainMenu.clickable.clicked += ToMainMenu;
        exit.clickable.clicked += Exit;
        next.clickable.clicked += Next;
        next.clickable.clicked += GameObject.Find("AIAssembly").GetComponent<manager>().AIIncreaseCurrentPart;
        audioPlayer = GetComponent<AudioSource>();
        restart = SceneMenu.Q<Button>("Restart");
        restart.clickable.clicked += Restart;
    }

    void EnterSceneMenu()
    {
        SceneMenu.style.display = DisplayStyle.Flex;
        Controls.style.display = DisplayStyle.None;
        audioPlayer.PlayOneShot(clickSound);
        StartCoroutine(AnimateAvatar());
    }

    void Resume()
    {
        SceneMenu.style.display = DisplayStyle.None;
        Controls.style.display = DisplayStyle.Flex;
        audioPlayer.PlayOneShot(clickSound);
        StopCoroutine(AnimateAvatar());
    }

    IEnumerator AnimateAvatar()
    {
        VisualElement avatar = SceneMenu.Q<VisualElement>("SceneMenuAvatar");
        avatar.ToggleInClassList("Animation1");
        avatar.ToggleInClassList("Animation2");
        avatar.ToggleInClassList("Animation3");

        while (true)
        {
            avatar.ToggleInClassList("Animation1");
            yield return new WaitForSeconds(1);
            avatar.ToggleInClassList("Animation1");
            avatar.ToggleInClassList("Animation2");
            yield return new WaitForSeconds(1);
            avatar.ToggleInClassList("Animation2");
            avatar.ToggleInClassList("Animation1");
            yield return new WaitForSeconds(1);
            avatar.ToggleInClassList("Animation1");
            avatar.ToggleInClassList("Animation3");
            yield return new WaitForSeconds(1);
            avatar.ToggleInClassList("Animation3");
        }
    }
    
    void ToMainMenu()
    {
        StartCoroutine(Click(0));
    }

    void Restart()
    {
        StartCoroutine(Click(1));
    }
    
    void Exit()
    {
        StartCoroutine(Click(-1));
    }

    void Next()
    {
        GetComponent<AudioSource>().PlayOneShot(clickSound);
    }
    
    IEnumerator Click(int newScene)
    {
        audioPlayer.PlayOneShot(clickSound);
        yield return new WaitForSeconds(clickSound.length);
        if (newScene == -1)
        {
            Application.Quit();
        }
        SceneManager.LoadScene(newScene);
    }

    private void Update()
    {
    }
}
