using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    private List<Torch> nearbyTorches = new List<Torch>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    // Public method to allow easy testing and interaction triggering
    public void Interact()
    {
        // ToArray() prevents modification errors if the list changes
        foreach (var torch in nearbyTorches.ToArray())
        {
            if (torch != null)
            {
                torch.LightUp();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Torch torch))
        {
            if (!nearbyTorches.Contains(torch))
            {
                nearbyTorches.Add(torch);
                Debug.Log("Nearby Torch Added: " + torch);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Torch torch))
        {
            nearbyTorches.Remove(torch);
            Debug.Log("Nearby Torch Removed: " + torch);
        }
    }
}
