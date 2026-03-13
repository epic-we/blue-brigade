using UnityEngine;

public class DeactivateObject : MonoBehaviour
{
    [SerializeField] private GameObject otherObject;

  
    public void Deactivate() => gameObject.SetActive(false);

    
    public void ActivateOther()
    {
        if (otherObject != null)
            otherObject.SetActive(true);
    }

    
    public void DeactivateOther()
    {
        if (otherObject != null)
            otherObject.SetActive(false);
    }
}
