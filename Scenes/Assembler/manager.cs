using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class manager : MonoBehaviour
{
    
    public float waitRadius;
    public float waitSize;
    public AudioClip click;
    public AudioClip error;
    public AudioClip finalFanfare;
    public AudioClip congratsMessage;
    public AudioClip tutorial;
    public bool isAI;
    public int currentPart = 0;
    public bool selected = false;
    public List<AudioClip> stepsAudioClips;
    public GameObject phantom;


    private Camera playerCamera;
    private List<GameObject> parts = new List<GameObject>();
    private List<Vector3> partPositions = new List<Vector3>();
    private List<Quaternion> partRotations = new List<Quaternion>();
    private List<Vector3> partScales = new List<Vector3>();

    //Audio States
    private bool doClick = false;
    private bool doError = false;
    private bool doFinished = false;
    private bool doStep = false;
    
    // Start is called before the first frame update
    void Start()
    {
        playerCamera = GameObject.Find("Player Camera").GetComponent<Camera>();
        foreach (Transform child in transform)
        {
            partPositions.Add(child.position);
            partRotations.Add(child.rotation);
            partScales.Add(child.localScale);
            parts.Add(child.gameObject);
            
        }

        StartCoroutine(AudioManager());
        StartCoroutine(CommandWaitingParts());
        if (!isAI)
        {
            StartCoroutine(CommandSelectedPart());
            StartCoroutine(CommandPlacedParts());
        }
        else
        {
            StartCoroutine(CommandAISelectedParts());
        }


    }

    IEnumerator CommandWaitingParts()
    {
        float angleIncrement = 6.28f / parts.Count;
        Vector3 targetScale = new Vector3(waitSize, waitSize, waitSize);
        while (true)
        {
            targetScale.x = waitSize;
            targetScale.y = waitSize;
            targetScale.z = waitSize;
            for (int x = currentPart; x < parts.Count; x++)
            {
                GameObject part = parts[x];
                float targetRadius = waitRadius;
                if (selected)
                {
                    targetRadius = waitRadius * 10;
                }
                part.transform.Rotate(Vector3.right, 45.0f * Time.deltaTime);
                Vector3 targetPos = new Vector3(
                    transform.position.x + math.cos(angleIncrement * x) * targetRadius,
                    transform.position.y + math.sin(angleIncrement * x) * targetRadius,
                    transform.position.z);
                if (isAI)
                {
                    targetPos.x *= -1;
                }
                
                part.transform.position = Vector3.Lerp(part.transform.position, targetPos, 5.0f * Time.deltaTime);
                
                part.transform.localScale = Vector3.Lerp(part.transform.localScale, targetScale, 3.0f * Time.deltaTime);

            }

            yield return null;
        }
    }

    IEnumerator CommandSelectedPart()
    {
        Plane colliderPlane = new Plane((Vector3.zero - transform.position), transform.position.z);
        while (currentPart != parts.Count)
        {
            if (selected)
            {
                int index = currentPart;
                bool placed = false;
                currentPart++;
                GameObject part = parts[index];
                Vector2 targetPos = new Vector2(partPositions[index].x, partPositions[index].y);
                Vector2 partPos = new Vector2(part.transform.position.x, part.transform.position.y);
                GameObject phantomClone = Instantiate(phantom, partPositions[index], partRotations[index]);
                
                while (Input.GetMouseButton(0) && selected)
                {
                    phantomClone.transform.Rotate(Vector3.up, 45.0f * Time.deltaTime);
                    phantomClone.transform.localScale = new Vector3(math.sin(Time.frameCount/100.0f),math.sin(Time.frameCount/100.0f),math.sin(Time.frameCount/100.0f)) / 5;
                    part.transform.localScale =
                        Vector3.Lerp(part.transform.localScale, partScales[index], 3.0f * Time.deltaTime);
                    var ray = playerCamera.ScreenPointToRay(Input.mousePosition);
                    if (colliderPlane.Raycast(ray, out float distance))
                    {
                        part.transform.position = ray.GetPoint(distance);
                        partPos.x = ray.GetPoint(distance).x;
                        partPos.y = ray.GetPoint(distance).y;
                    }
                    
                    float dist = (targetPos - partPos).magnitude;
                    Debug.Log(dist);
                    if (dist < .1)
                    {
                        part.transform.position = partPositions[index];
                        part.transform.rotation = partRotations[index];
                        part.transform.localScale = partScales[index];
                        selected = false;
                        placed = true;
                        doClick = true;
                        doStep = true;
                    }

                    yield return null;
                }

                if (!placed)
                {
                    selected = false;
                    currentPart--;
                }
                
                Destroy(phantomClone);
                
            }
            
            
            yield return null;
        }

        doStep = true;
        doFinished = true;
    }

    IEnumerator CommandPlacedParts()
    {
        Vector3 direction = (transform.position - Vector3.zero).normalized;
        while (true)
        {
            for (int i = 0; i < currentPart; i++)
            {
                if (!selected)
                {
                    GameObject part = parts[i];
                    if (currentPart != parts.Count)
                    {
                        part.transform.position = new Vector3(math.lerp(part.transform.position.x, partPositions[i].x, 5.0f * Time.deltaTime), math.lerp(part.transform.position.y, partPositions[i].y, 5.0f * Time.deltaTime), math.lerp(part.transform.position.z, partPositions[i].z + 30, 5.0f * Time.deltaTime));
                    }
                    else
                    {
                        part.transform.position = new Vector3(math.lerp(part.transform.position.x, partPositions[i].x, 5.0f * Time.deltaTime), math.lerp(part.transform.position.y, partPositions[i].y, 10.0f * Time.deltaTime), math.lerp(part.transform.position.z, partPositions[i].z, 5.0f * Time.deltaTime));
                    }
                    
                }
                else
                {
                    if (i != currentPart-1)
                    {
                        if (currentPart == parts.Count)
                        {
                            GameObject part = parts[i];
                            part.transform.position = new Vector3(part.transform.position.x, math.lerp(part.transform.position.y, partPositions[i].y + 5, 5.0f * Time.deltaTime), math.lerp(part.transform.position.z, partPositions[i].z, 5.0f * Time.deltaTime));
                        }
                        else
                        {
                            GameObject part = parts[i];
                            part.transform.position = new Vector3(part.transform.position.x, part.transform.position.y, math.lerp(part.transform.position.z, partPositions[i].z, 5.0f * Time.deltaTime));
                        }
                        
                    }
                }
            }
            yield return null;
        }
    }

    IEnumerator CommandAISelectedParts()
    {
        while (true)
        {
            for (int i = 0; i < currentPart; i++)
            {
                GameObject part = parts[i];

                part.transform.position = Vector3.Lerp(part.transform.position, partPositions[i], 3 * Time.deltaTime);
                part.transform.rotation = Quaternion.Lerp(part.transform.rotation, partRotations[i], 3.0f * Time.deltaTime);
                part.transform.localScale = Vector3.Lerp(part.transform.localScale, partScales[i], 3.0f * Time.deltaTime);
            }

            yield return null;
        }
    }

    IEnumerator AudioManager()
    {
        var audioPlayer = GetComponent<AudioSource>();
        var avatar = GameObject.Find("Controls").GetComponent<UIDocument>().rootVisualElement
            .Q<Button>("EnterSceneMenu");
        
        
        if (!isAI)
        {
            avatar.ToggleInClassList("MenuButton");
            audioPlayer.PlayOneShot(tutorial);
            yield return new WaitForSeconds(tutorial.length);
            avatar.ToggleInClassList("MenuButton");
        }
        while (true)
        {
            if (doClick)
            {
                audioPlayer.PlayOneShot(click, 2);
                yield return new WaitForSeconds(click.length);
                doClick = false;
            }
            if (doError)
            {
                audioPlayer.PlayOneShot(error);
                yield return new WaitForSeconds(error.length);
                doError = false;
            }
            if (doStep)
            {
                avatar.ToggleInClassList("MenuButton");
                if (isAI)
                {
                    if (stepsAudioClips[currentPart - 1] != null)
                    {
                        audioPlayer.PlayOneShot(stepsAudioClips[currentPart-1]);
                        yield return new WaitForSeconds(stepsAudioClips[currentPart - 1].length);
                    }
                }
                else
                {
                    if (stepsAudioClips[currentPart-1] != null)
                    {
                        audioPlayer.PlayOneShot(stepsAudioClips[currentPart-1]);
                        yield return new WaitForSeconds(stepsAudioClips[currentPart-1].length);
                    }
                }
                avatar.ToggleInClassList("MenuButton");
                doStep = false;
            }   
            if (doFinished)
            {
                audioPlayer.PlayOneShot(finalFanfare);
                yield return new WaitForSeconds(finalFanfare.length);
                audioPlayer.PlayOneShot(congratsMessage);
                doFinished = false;
            }
               
            yield return null;
        }
    }

    public void AIIncreaseCurrentPart()
    {
        if (currentPart < parts.Count)
        {
            doStep = true;
            currentPart++;
        }
    }
    private void Update()
    {
        if (!isAI)
        {
            if (Input.GetMouseButtonDown(0))
            {
                var ray = GameObject.Find("Player Camera").GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    Debug.Log("Input Ray Hit");

                    var selectedPart = hit.transform.gameObject;
                    if (currentPart != parts.Count && selectedPart == parts[currentPart])
                    {
                        doClick = true;
                        selected = true;
                    }
                    else
                    {
                        doError = true;
                    }
                }
            }
        }
        
    }
}
