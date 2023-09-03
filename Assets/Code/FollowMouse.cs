using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    public GameObject ballSpawner;

    private bool clicked = false;
    // Start is called before the first frame update
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {
        Vector2 GridMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        this.transform.position = GridMousePos;
        if (Input.GetMouseButtonDown(0) && !this.clicked)
        {
            clicked = true;
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ball")
        {
            if (clicked)
            {
                Debug.Log("trigger" + other.gameObject.name);
                GridCell cell = GameGrid.current.GetGridPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                Vector2 mouseClick = cell.gridPosition;
                Ball otherBall = other.gameObject.GetComponent<Ball>();
                int colorIndex = otherBall.color;
                Color color = otherBall.gameObject.GetComponent<SpriteRenderer>().color;
                GameObject GameBall = Instantiate(ballSpawner.GetComponent<BallSpawner>().Balls[colorIndex - 1], mouseClick, Quaternion.identity);
                Ball ball = GameBall.GetComponent<Ball>();
                ball.color = colorIndex;
                ball.GetComponent<SpriteRenderer>().color = color;
                ball.Snap();
                GameGrid.current.RemoveConnected(cell);
                clicked = false;
            }

        }
    }
}
