using UnityEngine;

public class VerticalMover : MonoBehaviour
{
    public Transform target;   // The object to move
    public float speed = 1.0f; // Meters per second

    private bool upHeld, downHeld;

    void Update()
    {
        float dir = (upHeld ? 1f : 0f) + (downHeld ? -1f : 0f);
        if (dir != 0f && target != null)
        {
            target.position += Vector3.up * (dir * speed * Time.deltaTime);
        }
    }

    // Hook to onPrimaryButtonPressed (for UP) or whichever button you prefer
    public void OnUp(string state)
    {
        upHeld = state == "Pressed";
    }

    // Hook to onSecondaryButtonPressed (for DOWN)
    public void OnDown(string state)
    {
        downHeld = state == "Pressed";
    }
}
