using UnityEngine;

public class Spinning : MonoBehaviour
{
    [SerializeField]
    float spinSpeed = 0f;

    [SerializeField]
    Vector3 spinVector;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        if (spinSpeed != 0)
        {
            transform.Rotate(spinVector * Time.deltaTime * spinSpeed);
        }
    }
}
