using TMPro;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    [SerializeField]
    private GameObject[] charactersToSpawn;

    [SerializeField]
    private TextMeshProUGUI pressKeyText;

    private PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = FindAnyObjectByType<PlayerMovement>();

        playerMovement.StartMoving(false);
        playerMovement.CanPlaySound = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            foreach (GameObject character in charactersToSpawn)
            {
                character.SetActive(true);
            }


            playerMovement.DelayStartMovementFromTitle();
            gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        float alpha = Mathf.Abs(Mathf.Sin(Time.time));

        pressKeyText.color = new Color(pressKeyText.color.r, pressKeyText.color.g, pressKeyText.color.b, alpha);
    }
}
