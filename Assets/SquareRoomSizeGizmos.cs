using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareRoomSizeGizmos : MonoBehaviour
{
    [SerializeField]
    private Vector2 _roomSize = new Vector2(1920, 1080);

    [SerializeField]
    private float _tileSize = 64;

    [SerializeField]
    private Vector2 _mapSize = new Vector2(10, 10);

    [SerializeField]
    private Color gridColor;

    void OnDrawGizmos()
    {

        float screenSizeX = (_roomSize.x / _tileSize);
        float screenSizeY = (_roomSize.y / _tileSize);
        // Draw a yellow sphere at the transform's position
        for (int x = 0; x < _mapSize.x; x++)
        {

            for (int y = 0; y < _mapSize.y; y++)
            {
                Vector2 position = new Vector2(x * screenSizeX, y * screenSizeY);
                Vector2 size = new Vector3(screenSizeX, screenSizeY);

                Gizmos.color = gridColor;
                Gizmos.DrawLine(position, new Vector2(position.x + screenSizeX,position.y));
                Gizmos.DrawLine(new Vector2(position.x + screenSizeX, position.y), new Vector2(position.x + screenSizeX, position.y + screenSizeY));
                Gizmos.DrawLine(new Vector2(position.x, position.y + screenSizeY), new Vector2(position.x, position.y));
                Gizmos.DrawLine(new Vector2(position.x, position.y + screenSizeY), new Vector2(position.x + screenSizeX, position.y + screenSizeY));
            }
        }
    }
}
