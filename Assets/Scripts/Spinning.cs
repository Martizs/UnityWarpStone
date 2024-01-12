using UnityEngine;

public class Spinning : MonoBehaviour
{
    [SerializeField]
    float spinSpeed = 100f;

    [SerializeField]
    bool reverse = false;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate((reverse ? Vector3.back : Vector3.forward) * Time.deltaTime * spinSpeed);
    }
}
