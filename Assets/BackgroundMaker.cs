using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;


public class BackgroundMaker : MonoBehaviour
{
    private PlayerMvoement playerMovement;
    public GameObject backgroundPrefab;

    float prefabWidth;
    float prefabHeight;
    private GameObject[][] backgrounds;

    private int rowCount;
    private int colCount;


    private float cameraHeight;
    private float cameraWidth;


    private void Awake()
    {
        playerMovement = FindObjectOfType<PlayerMvoement>();



        cameraHeight = 2f * Camera.main.orthographicSize; // ※ orthographicSize : 카메라 세로의 절반
        cameraWidth = 2f * Camera.main.orthographicSize * Camera.main.aspect;  // ※ aspect : 종횡비 


        GameObject instanObject;
        if(true)
        {
            string filePath = Path.Combine(Application.streamingAssetsPath, "background.png");
            if (!File.Exists(filePath))
            {
                Debug.LogError("파일없음");
                return;
            }

            byte[] imageBytes = File.ReadAllBytes(filePath);

            Texture2D texture = new Texture2D(2, 2);
            if (!texture.LoadImage(imageBytes))
            {
                Debug.LogError("이미지 로딩 실패");
                return;
            }

            Sprite createdSprite = Sprite.Create(texture,
                new Rect(0, 0, texture.width, texture.height), // Width // Height 
                new Vector2(0.5f, 0.5f)); // ※ Pivot (Center)

            GameObject go = new GameObject("LoadedObject");
            go.transform.position = Vector3.zero;
            SpriteRenderer spriteRenderer = go.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = createdSprite;

            instanObject = go;

            Renderer renderer = go.GetComponent<Renderer>();

            prefabWidth = renderer.bounds.size.x;
            prefabHeight = renderer.bounds.size.y;
        }
        else
        {
            instanObject = backgroundPrefab;

            Renderer renderer = backgroundPrefab.GetComponent<Renderer>();

            prefabWidth = renderer.bounds.size.x;
            prefabHeight = renderer.bounds.size.y;
        }


        if (true) // 우선 사용 안함.
        {
            rowCount = Mathf.CeilToInt((float)cameraHeight / prefabHeight) + 2;
            colCount = Mathf.CeilToInt((float)cameraWidth / prefabWidth) + 2;
        }
        if (false)
        {
            rowCount = 6;
            colCount = 4;
        }


        backgrounds = backgrounds = new GameObject[rowCount][];
        Vector2 centerPoint = playerMovement.transform.position;
        for (int i = 0; i < rowCount; i++)
        {
            backgrounds[i] = new GameObject[colCount];
            for (int j = 0; j < colCount; j++)
            {
                GameObject background = Instantiate(instanObject);
                backgrounds[i][j] = background;

                background.transform.position = new Vector2(centerPoint.x + (j - colCount / 2f) * prefabWidth, 
                    centerPoint.y + (i - rowCount / 2f) * prefabHeight);
            }
        }

        Destroy(instanObject);
    }

    public void OnPlayerMove(Vector2 movePos, bool isMovingUpward, bool isMovingRightward)
    {
        if (isMovingUpward
            && movePos.y + cameraHeight / 2f + 10f >= backgrounds[rowCount - 1][0].transform.position.y + prefabHeight / 2f)
        {
            GameObject[] tempArray = new GameObject[colCount];

            for (int i = 0; i < colCount; i++)
            {
                tempArray[i] = backgrounds[0][i];
            }
            for (int i = 0; i < rowCount - 1; i++)
            {
                for (int j = 0; j < colCount; j++)
                {
                    backgrounds[i][j] = backgrounds[i + 1][j];
                }
            }
            for (int i = 0; i < colCount; i++)
            {
                GameObject background = tempArray[i];

                backgrounds[rowCount - 1][i] = background;
                background.transform.position = new Vector2(background.transform.position.x,
                    background.transform.position.y + prefabHeight * rowCount);
            }
        }
        else if(!isMovingUpward 
            && movePos.y - cameraHeight / 2f - 10f <= backgrounds[0][0].transform.position.y - prefabHeight / 2f)
        {
            GameObject[] tempArray = new GameObject[colCount];

            for(int i = 0; i < colCount; i++)
            {
                tempArray[i] = backgrounds[rowCount - 1][i];
            }
            for(int i = rowCount - 1; i > 0; i--)
            {
                for(int j = 0; j < colCount; j++)
                {
                    backgrounds[i][j] = backgrounds[i - 1][j];
                }
            }
            for(int i = 0; i < colCount; i++)
            {
                GameObject background = tempArray[i];

                backgrounds[0][i] = background;
                background.transform.position = new Vector2(background.transform.position.x,
                    background.transform.position.y - prefabHeight * rowCount);
            }
        }

        if (isMovingRightward 
            && movePos.x + cameraWidth / 2f + 10f >= backgrounds[0][colCount -1].transform.position.x + prefabWidth / 2f)
        {
            GameObject[] tempArray = new GameObject[rowCount];

            for(int i = 0; i < rowCount; i++)
            {
                tempArray[i] = backgrounds[i][0];
            }
            for(int i = 0; i < colCount - 1; i++)
            {
                for (int j = 0; j < rowCount; j++)
                {
                    backgrounds[j][i] = backgrounds[j][i + 1];
                }
            }
            for(int i = 0; i < rowCount; i++)
            {
                GameObject background = tempArray[i];

                backgrounds[i][colCount - 1] = background;
                background.transform.position = new Vector2(background.transform.position.x + prefabWidth * colCount,
                    background.transform.position.y);
            }
        }
        else if(!isMovingRightward
            && movePos.x - cameraWidth / 2f - 10f <= backgrounds[0][0].transform.position.x - prefabWidth / 2f)
        {
            GameObject[] tempArray = new GameObject[rowCount];

            for(int i = 0; i < rowCount; i++)
            {
                tempArray[i] = backgrounds[i][colCount - 1];
            }
            for(int i = colCount - 1; i > 0; i--)
            {
                for(int j = 0; j < rowCount; j++)
                {
                    backgrounds[j][i] = backgrounds[j][i - 1];
                }
            }
            for(int i = 0; i < rowCount; i++)
            {
                GameObject background = tempArray[i];

                backgrounds[i][0] = background;
                background.transform.position = new Vector2(background.transform.position.x - prefabWidth * colCount,
                    background.transform.position.y);
            }
        }
        

    }


}
