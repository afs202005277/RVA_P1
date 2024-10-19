using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableTowerController : TowerController
{
    private bool isMovable = true; // Initially, the tower is movable
    private Transform originalParent;
    void Start()
    {
        Initialize();
        originalParent = transform.parent;
    }

    public void LockPosition()
    {
        isMovable = false; // Lock the tower in place
        transform.SetParent(null); // Unparent from the Image Target
    }

    public void UnlockPosition()
    {
        isMovable = true; // Allow movement
        transform.SetParent(originalParent); // Reparent to the Image Target
        // Optionally, reset the position to match the Image Target's position
        transform.localPosition = Vector3.zero; // Reset local position if needed
    }

    private void FollowParent()
    {
        // Follow the position of the original parent (Image Target)
        if (originalParent != null)
        {
            transform.position = originalParent.position; // Move tower to the parent's position
            transform.rotation = originalParent.rotation; // Match rotation if needed
        }
    }
}
