using TMPro;
using UnityEngine;

public class UiPointsText : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI text;
    [SerializeField] LevelManager levelManager;
    [SerializeField] GameObject parentObject;

    // Start is called before the first frame update
    void Start()
    {
        this.levelManager.OnBallExplode += this.OnBallExplode;
        this.levelManager.OnBallDestroy += this.onBallDestroy;
    }

    private void onBallDestroy(Vector3 position, Color color)
    {

        var pointsText = Instantiate(text, position, Quaternion.identity);
        pointsText.transform.parent = parentObject.transform;
        pointsText.color = color;
        pointsText.text = "10";
    }

    private void OnBallExplode(GridCell cell)
    {
        this.onBallDestroy(Camera.main.WorldToScreenPoint(cell.gridPosition), cell.ball.explosionColor);
    }

}
