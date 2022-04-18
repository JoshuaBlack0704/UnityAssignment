using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ViewPortManager : MonoBehaviour
{
    private Camera playerCamera;
    private Camera aiCamera;
    
    
    // Start is called before the first frame update
    void Start()
    {
        playerCamera = GameObject.Find("Player Camera").GetComponent<Camera>();
        aiCamera = GameObject.Find("AICamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        float pixelWidth = Screen.width / 2;
        Vector3 mousePos = Input.mousePosition;

        if (mousePos.x < pixelWidth)
        {
            
            //Ai Focus
            Rect rect = new Rect();
            rect.center = new Vector2(.25f, .5f);
            rect.height = (float)math.lerp(aiCamera.rect.height, 1, 1.0f * Time.deltaTime);
            rect.width = (float) math.lerp(aiCamera.rect.width, .5, 1.0f * Time.deltaTime);
            
            aiCamera.rect = new Rect(rect);

            rect.center = new Vector2(.75f, 0.5f);
            rect.height = (float)math.lerp(playerCamera.rect.height, .75, 1.0f * Time.deltaTime);
            rect.width = (float) math.lerp(playerCamera.rect.width, .3, 1.0f * Time.deltaTime);

            playerCamera.rect = new Rect(rect);
        }
        else
        {
            //Player Focus
            Rect rect = new Rect();
            rect.center = new Vector2(.25f, 0.5f);
            rect.height = (float)math.lerp(aiCamera.rect.height, .75, 1.0f * Time.deltaTime);
            rect.width = (float) math.lerp(aiCamera.rect.width, .35, 1.0f * Time.deltaTime);
            
            aiCamera.rect = new Rect(rect);

            rect.center = new Vector2(.75f, 0.5f);
            rect.height = (float)math.lerp(playerCamera.rect.height, 1, 1.0f * Time.deltaTime);
            rect.width = (float) math.lerp(playerCamera.rect.width, .5, 1.0f * Time.deltaTime);

            playerCamera.rect = new Rect(rect);
        }
    }
}
