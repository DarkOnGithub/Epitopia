using UnityEngine;

public class PlayerFaceChange : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] Sprite[] faceSprites;
    [SerializeField] Sprite actualFace;
    public Sprite startSprite;

    private void Start()
    {
        actualFace = startSprite;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) //vers left
        {
            actualFace = faceSprites[0];
        }
        if (Input.GetKeyDown(KeyCode.D)) //vers right
        {
            actualFace = faceSprites[1];
        }
        if (Input.GetKeyDown(KeyCode.Space))  //jump
        {
            if (actualFace == faceSprites[0])
            {
                actualFace = faceSprites[2]; //static left
            }
            else
            {
                actualFace = faceSprites[3]; //static right
            }
        }
        
        gameObject.GetComponent<SpriteRenderer>().sprite = actualFace;
    }
}
