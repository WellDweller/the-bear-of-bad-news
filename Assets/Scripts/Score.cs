using UnityEngine;
using UnityEngine.Events;
using TMPro;


public struct ScoreUpdate
{
    public int scored;

    public int total;
}


public class Score : MonoBehaviour
{
    [field:SerializeField] public UnityEvent<ScoreUpdate> OnScore { get; private set; }



    [field:SerializeField] public int Value { get; private set; }

    [SerializeField] TextMeshProUGUI textMesh;



    void Awake()
    {
        Reset();
    }




    public void HandleMinigameResult(MinigameResult result)
    {
        AddPoints(10);
    }

    public void Reset()
    {
        Value = 0;
        textMesh.text = "0";
    }

    void AddPoints(int points)
    {
        Value += points;
        textMesh.text = Value.ToString();
        OnScore?.Invoke(new ScoreUpdate { scored = points, total = Value });
    }
}
