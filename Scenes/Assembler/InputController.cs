using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public GameObject playerAssembly;
    public GameObject aiAssembly;

    private manager playerManager;
    private manager aiManager;
    private Plane colliderPlane;

    // Start is called before the first frame update
    void Start()
    {
        playerManager = playerAssembly.GetComponent<manager>();
        aiManager = aiAssembly.GetComponent<manager>();
        colliderPlane = new Plane((GameObject.Find("Player Camera").transform.position - playerAssembly.transform.position), playerAssembly.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = GameObject.Find("Player Camera").GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("Input Ray Hit");

                var selectedPart = hit.transform.gameObject;
                playerAssembly.GetComponent<manager>().CheckPart(selectedPart);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            aiManager.AIIncreaseCurrentPart();
        }

        if (Input.GetMouseButton(0))
        {
            playerManager.targeting = true;
        }
        else
        {
            playerManager.targeting = false;
        }
    
        if (playerManager.targeting)
        {
            //Mouse Specific Movement
            var ray = GameObject.Find("Player Camera").GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            if (colliderPlane.Raycast(ray, out float distance))
            {
                playerManager.target.x = ray.GetPoint(distance).x;
                playerManager.target.y = ray.GetPoint(distance).y;
                playerManager.target.z = ray.GetPoint(distance).z;

            }
        }
    }
}
