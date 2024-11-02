using UnityEngine;

public class MapEditCamMovement :MonoBehaviour
{
    
    private Vector2 curCameraMovementDir;
    private Vector2 curCameraLookDir;
    private float curCameraDepth;

    

    private void SetMovementDir(Vector2 newDir)
    {
        curCameraMovementDir = newDir;
    }

}