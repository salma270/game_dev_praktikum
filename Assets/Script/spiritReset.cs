using UnityEngine;

public class spiritReset : MonoBehaviour
{
    void Awake()
    {
        Spirit.totalSpirits = 0;
        Spirit.collectedSpirits = 0;
    }
}
