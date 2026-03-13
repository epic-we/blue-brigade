using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class DetectNpcs : MonoBehaviour
{

    [SerializeField] private KeyCode censorKey = KeyCode.Space;  

    [SerializeField] private LayerMask npcLayer;

    [SerializeField] private float detectionCooldown = 0.1f;
    private float justDetected = 0f;
    private List<GameObject> npcsInArea;

    [SerializeField] private float beatUpRadius = 2f;
    private GameObject selectedGuy = null;

     private LevelManager levelManager;

    private bool canInteract = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        npcsInArea = new List<GameObject>();

        levelManager = FindAnyObjectByType<LevelManager>();
    }

    private void Update()
    {
        if((Input.GetKeyDown(censorKey) || Input.GetMouseButtonDown(0)) && selectedGuy != null && !levelManager.revolution && canInteract)
        {
            if(!selectedGuy.GetComponent<CivilianFault>().menuButton)
                npcsInArea.Remove(selectedGuy);
                
            selectedGuy.GetComponent<CivilianFault>().Censor();
        }
    }

    private void FixedUpdate()
    {
        if (Time.time - justDetected > detectionCooldown)
        {
            if(selectedGuy != null)
            {
                selectedGuy.GetComponent<CivilianFault>().spriteRenderer.material.SetFloat("_Thickness", 0f);
            }
            justDetected = Time.time;
            RemoveFarObjects(beatUpRadius + 5);
            selectedGuy = GetClosestObject();

            if (selectedGuy != null)
            {
                selectedGuy.GetComponent<CivilianFault>().spriteRenderer.material.SetFloat("_Thickness", 80f);
            }
        }

    }

    public void RemoveAllObject()
    {
        npcsInArea.Clear();
    }


    void RemoveFarObjects(float radius)
    {
        Vector2 playerPosition = transform.position;
        npcsInArea.RemoveAll(obj => obj == null || Vector2.Distance(playerPosition, obj.transform.position) > radius);
    }


    GameObject GetClosestObject()
    {
        if (npcsInArea == null || npcsInArea.Count == 0)
        {
            return null;
        }

        GameObject nearest = null;
        float minDistance = beatUpRadius; // Only consider objects within this range
        Vector2 playerPosition = transform.position;

        foreach (GameObject obj in npcsInArea)
        {
            if (obj == null) continue;

            if (obj.GetComponent<CivilianFault>().censored)
                continue;

                float distance = Vector2.Distance(playerPosition, obj.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = obj;
            }
        }

        return nearest; // Returns null if no object is within range
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        int x = 1 << collision.gameObject.layer;

        if (x == npcLayer.value)
        {
            if(!collision.gameObject.GetComponent<CivilianFault>().censored) { npcsInArea.Add(collision.gameObject); }
            
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, beatUpRadius);
    }

    public void CanInteract(bool interact)
    {
        canInteract = interact;
    }
}
