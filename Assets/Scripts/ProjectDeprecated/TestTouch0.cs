using UnityEngine;

public class TestTouch0 : MonoBehaviour
{
    /*private InputManager inputManager;
    private Camera cameraMain;

    private void Awake()
    {
        inputManager = InputManager.Instance;
    }
    private void OnEnable()
    {
        InputManager.StartingTouch += Move;
    }
    private void OnDisable()
    {
        InputManager.EndingTouch -= Move;
    }

    public void Move(Vector2 screenPos, float time)
    {
        Vector3 screenCoordinates = new Vector3(screenPos.x, screenPos.y, cameraMain.nearClipPlane);
        Vector3 worldCoordinates = cameraMain.ScreenToWorldPoint(screenCoordinates);
        worldCoordinates.z = 0;
        transform.position = worldCoordinates;
    }*/
}
