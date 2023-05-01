using UnityEngine;
using TMPro;

public class GameOverScore : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textMesh;

    void Awake()
    {
        textMesh.text = GlobalGameState.Score.ToString();
    }
}
