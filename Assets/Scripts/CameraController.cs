using UnityEngine;

public class CameraController : MonoBehaviour {

    public float smooth = 1.5f;         // The relative speed at which the camera will catch up.


    private Transform player;           // Reference to the player's transform.
    private Vector3 relCameraPos;       // The relative position of the camera from the player.
    private float relCameraPosMag;      // The distance of the camera from the player.
    private Vector3 newPos;             // The camera's position is trying to reach.
    public LayerMask layerMask;

    void Awake() {
        // Setting up the reference.
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Setting the relative position as the initial relative position of the camera in the scene.
        relCameraPos = transform.position - player.position;
        relCameraPosMag = relCameraPos.magnitude - 0.5f;
    }

    void FixedUpdate() {
        // The standard position of the camera is the relative position of the camera from the player.
        Vector3 standardPos = player.position + relCameraPos;

        // The abovePos is directly above the player at the same distance as the standard position.
        Vector3 abovePos = player.position + Vector3.up * relCameraPosMag;

        // An array of 5 points to check if the camera can see the player.
        Vector3[] checkPoints = new Vector3[5];

        // The first is the standard position of the camera.
        checkPoints[0] = standardPos;

        // The next three are 25%, 50% and 75% of the distance between the standard position and abovePos.
        checkPoints[1] = Vector3.Lerp(standardPos, abovePos, 0.25f);
        checkPoints[2] = Vector3.Lerp(standardPos, abovePos, 0.5f);
        checkPoints[3] = Vector3.Lerp(standardPos, abovePos, 0.75f);

        // The last is the abovePos.
        checkPoints[4] = abovePos;

        // Run through the check points...
        for (int i = 0; i < checkPoints.Length; i++) {
            // ... if the camera can see the player...
            if (ViewingPosCheck(checkPoints[i]))
                // ... break from the loop.
                break;
        }

        // Lerp the camera's position between its current position and its new position.
        transform.position = Vector3.Lerp(transform.position, newPos, smooth * Time.deltaTime);
    }


    bool ViewingPosCheck(Vector3 checkPos) {
        RaycastHit hit;

        // If a raycast from the check position to the player hits something...
        if (Physics.Raycast(checkPos, player.position - checkPos, out hit, relCameraPosMag, layerMask))
            // ... if it is not the player...
            if (hit.transform != player)
                // This position isn't appropriate.
                return false;

        // If we haven't hit anything or we've hit the player, this is an appropriate position.
        newPos = checkPos;
        return true;
    }

}
