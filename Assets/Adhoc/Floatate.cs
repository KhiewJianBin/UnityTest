using UnityEngine;

public class Floatate : MonoBehaviour
{
    public float bobSpeed = 3.0f; //Bob speed
    public float bobHeight = 0.5f; //Bob height
    public float bobOffset = 0.0f;

    public float PrimaryRot = 80.0f; //First axies degrees per second
    public float SecondaryRot = 40.0f; //Second axies degrees per second
    public float TertiaryRot = 20.0f; //Third axies degrees per second

    float OriginalposY;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    public void Awake()
    {
        if (bobSpeed < 0)
        {
            Debug.LogWarning("Negative object bobSpeed value! May result in undesired behavior. Continuing anyway.",
                gameObject);
        }

        if (bobHeight < 0)
        {
            Debug.LogWarning("Negative object bobHeight value! May result in undesired behavior. Continuing anyway.",
                gameObject);
        }

        OriginalposY = transform.position.y; // Store the original PosY
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    public void Update()
    {
        transform.Rotate(new Vector3(0, PrimaryRot, 0) * Time.deltaTime, Space.World);
        transform.Rotate(new Vector3(SecondaryRot, 0, 0) * Time.deltaTime, Space.Self);
        transform.Rotate(new Vector3(0, 0, TertiaryRot) * Time.deltaTime, Space.Self);

        Vector3 position = transform.position;
        position.y = OriginalposY + (((Mathf.Cos((Time.time + bobOffset) * bobSpeed) + 1) / 2) * bobHeight);
        transform.position = position;
    }
}