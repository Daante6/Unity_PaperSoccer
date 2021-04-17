using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class DrawAll : MonoBehaviour
{

	private int ballX = 0;
	private int ballY = 0;
	public LineRenderer LineRenderer;
	public Button button;
	private Vector3[] points = new Vector3[300];
	private int pointsCounter = 1;
	private int selectionX;
	private int selectionY;
	public Text text;
	public Text winnerText;
	public Text scoreText;
	public Text upPlayer;
	public Text downPlayer;
	private bool isPlayerTurn = true;
	private bool isEndGame = false;
	public Transform BallTransform;

	// Use this for initialization
	void Start()
	{
		LineRenderer.positionCount = 1;
		points[0] = new Vector3(ballX, ballY, 1.0f);
		//drawBallLine();

		for (int i = 1; i < points.Length; i++)
		{
			points[i] = new Vector3(100f, 100f, 1.0f);
		}
        if (StaticNameController.aiActive)
        {
			upPlayer.text = "Player Side";
			downPlayer.text = "AI Side";
        }
        else
        {
			upPlayer.text = "Player1 Side";
			downPlayer.text = "Player2 Side";
		}
	}

	// Update is called once per frame
	void Update()
	{
		drawFieldLines();
		UpdateSelection();
        if (!isEndGame)
        {
			if (isPlayerTurn)
			{
				if (Input.GetMouseButtonDown(0)) //check if clicked
				{
					if (checkLegalMove())
					{
						makeMove(selectionX, selectionY);
					}
				}
			}
			else //AI Turn
			{
                if (StaticNameController.aiActive)
                {
					AITurn();
                }
                else
                {
					if (Input.GetMouseButtonDown(0)) //check if clicked
					{
						if (checkLegalMove())
						{
							makeMove(selectionX, selectionY);
						}
					}
				}
				
			}
		}
		scoreText.text = "Player score: " + StaticNameController.playerWins + "\nAI score: " + StaticNameController.AIWins;
	}
	public void drawBallLine()
	{
		LineRenderer.SetPositions(points);
	}

	public void drawFieldLines()
	{
		Vector3 widthLine = Vector3.right * 8;
		Vector3 heightLine = Vector3.up;
		Vector3 start = new Vector3(0f, 0f, 0f);
		for (int i = -5; i <= 5; i++)
		{
			start = new Vector3(-4, i, 0f);
			Debug.DrawLine(start, start + widthLine);
			if (i == 5)
			{
				break;
			}
			for (int j = 0; j <= 7; j++)
			{
				start = start + Vector3.right;
				if (j == 3)
				{
					Debug.DrawLine(start + new Vector3(0, -1, 0), start + heightLine + new Vector3(0, 1, 0));
				}
				else
				{
					Debug.DrawLine(start, start + heightLine);
				}


			}
		}
	}
	private void UpdateSelection()
	{
		if (!Camera.main)
		{
			return;
		}
		RaycastHit hit;
		if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25.0f, LayerMask.GetMask("FieldPlane")))
		{
			selectionX = (int)Decimal.Round((decimal)hit.point.x);
			selectionY = (int)Decimal.Round((decimal)hit.point.y);
		}
		else
		{

		}
	}
	private bool checkLegalMove()
	{
		//check same move
		for (int i = 0; i < pointsCounter; i++)
		{
			if (points[i].x == ballX && points[i].y == ballY)//if there was move
			{
				if (points[i + 1].x == selectionX && points[i + 1].y == selectionY) //check next
				{
					return false;
				}
				if (i > 0)
				{
					if (points[i - 1].x == selectionX && points[i - 1].y == selectionY) //check next
					{
						return false;
					}
				}

			}
		}
		//check rest
		if (selectionX == ballX && selectionY == ballY) // same place
		{
			return false;
		}
		else if ((selectionX < ballX - 1) || (selectionX > ballX + 1)) // 2 points far horizontal
		{
			return false;
		}
		else if ((selectionY < ballY - 1) || (selectionY > ballY + 1)) // 2 point far vertical
		{
			return false;
		}
		else if ((ballX == -4 && selectionX == -4) && (ballY != selectionY)) //left bump
		{
			return false;
		}
		else if ((ballX == 4 && selectionX == 4) && (ballY != selectionY)) //right bump
		{
			return false;
		}
		else if (selectionX < -4 || selectionX > 4) //select outside of game horizontal
		{
			return false;
		}
		else if (selectionY < -5 && selectionX != 0) //select outside of game bottom
		{
			return false;
		}
		else if (selectionY > 5 && selectionX != 0) //select outside of game top
		{
			return false;
		}
		else
		{
			return true;
		}

	}
	bool checkBounce()
	{
		for (int i = 0; i < pointsCounter - 2; i++) // check previous ball spots
		{
			if (points[i].x == ballX && points[i].y == ballY)
			{
				return true;
			}
		}
		if (ballX == -4 || ballX == 4) //check left/right bump
		{
			return true;
		}
		if (ballY == -5 || ballY == 5) //check up/down bump
		{
			if (ballX != 0)
            {
				return true;
			}
				
		}
		return false;
	}
	void endTurn()
	{
		isPlayerTurn = !isPlayerTurn;
        if (StaticNameController.aiActive)
        {
			if (isPlayerTurn)
			{
				text.text = "Turn: Player";
			}
			else
			{
				text.text = "Turn: AI";
			}
		}
        else
        {
			if (isPlayerTurn)
			{
				text.text = "Turn: Player 1";
			}
			else
			{
				text.text = "Turn: Player 2";
			}
		}
		
	}
	void makeMove(int selectionX, int selectionY)
	{
		ballX = selectionX;
		ballY = selectionY;
		points[pointsCounter] = new Vector3(ballX, ballY, 1.0f);
		LineRenderer.positionCount++;
		pointsCounter++;
		drawBallLine();
		if (ballY > 5)
		{
			endGame(false);
		}
		else if (ballY < -5)
		{
			endGame(true);
		}
		if (!checkBounce())
		{
			endTurn();
		}
		BallTransform.position = new Vector3(ballX,ballY,1.0f);
	}
	private void endGame(bool winner)
	{
		button.gameObject.SetActive(true);
		winnerText.gameObject.SetActive(true);
		if (winner)
		{
			StaticNameController.playerWins++;
            if (StaticNameController.aiActive)
            {
				winnerText.text = "Player wins!";
			}
            else
            {
				winnerText.text = "Player1 wins!";
			}
			
		}
		else
		{
			StaticNameController.AIWins++;
			if (StaticNameController.aiActive)
			{
				winnerText.text = "Computer wins!";
			}
			else
			{
				winnerText.text = "Player2 wins!";
			}
		}
		isEndGame = true;
	}
	private void AITurn()
	{
		if (ballX < 0)
		{
			selectionX = ballX + 1; selectionY = ballY + 1;
			if (checkLegalMove())
			{
				makeMove(selectionX, selectionY);
				return;
			}
			selectionX = ballX; selectionY = ballY + 1;
			if (checkLegalMove())
			{
				makeMove(selectionX, selectionY);
				return;
			}
			selectionX = ballX - 1; selectionY = ballY + 1;
			if (checkLegalMove())
			{
				makeMove(selectionX, selectionY);
				return;
			}
			selectionX = ballX - 1; selectionY = ballY;
			if (checkLegalMove())
			{
				makeMove(selectionX, selectionY);
				return;
			}
			selectionX = ballX + 1; selectionY = ballY;
			if (checkLegalMove())
			{
				makeMove(selectionX, selectionY);
				return;
			}
			selectionX = ballX + 1; selectionY = ballY - 1;
			if (checkLegalMove())
			{
				makeMove(selectionX, selectionY);
				return;
			}
			selectionX = ballX - 1; selectionY = ballY - 1;
			if (checkLegalMove())
			{
				makeMove(selectionX, selectionY);
				return;
			}
			selectionX = ballX; selectionY = ballY - 1;
			if (checkLegalMove())
			{
				makeMove(selectionX, selectionY);
				return;
			}
		}
		else if (ballX > 0)
		{
			selectionX = ballX - 1; selectionY = ballY + 1;
			if (checkLegalMove())
			{
				makeMove(selectionX, selectionY);
				return;
			}
			selectionX = ballX; selectionY = ballY + 1;
			if (checkLegalMove())
			{
				makeMove(selectionX, selectionY);
				return;
			}
			selectionX = ballX; selectionY = ballY + 1;
			if (checkLegalMove())
			{
				makeMove(selectionX, selectionY);
				return;
			}
			selectionX = ballX - 1; selectionY = ballY;
			if (checkLegalMove())
			{
				makeMove(selectionX, selectionY);
				return;
			}
			selectionX = ballX + 1; selectionY = ballY;
			if (checkLegalMove())
			{
				makeMove(selectionX, selectionY);
				return;
			}
			selectionX = ballX + 1; selectionY = ballY - 1;
			if (checkLegalMove())
			{
				makeMove(selectionX, selectionY);
				return;
			}
			selectionX = ballX ; selectionY = ballY - 1;
			if (checkLegalMove())
			{
				makeMove(selectionX, selectionY);
				return;
			}
			selectionX = ballX - 1; selectionY = ballY - 1;
			if (checkLegalMove())
			{
				makeMove(selectionX, selectionY);
				return;
			}
		}
		else
		{
			selectionX = ballX; selectionY = ballY + 1;
			if (checkLegalMove())
			{
				makeMove(selectionX, selectionY);
				return;
			}
			selectionX = ballX + 1; selectionY = ballY + 1;
			if (checkLegalMove())
			{
				makeMove(selectionX, selectionY);
				return;
			}
			selectionX = ballX - 1; selectionY = ballY + 1;
			if (checkLegalMove())
			{
				makeMove(selectionX, selectionY);
				return;
			}
			selectionX = ballX - 1; selectionY = ballY;
			if (checkLegalMove())
			{
				makeMove(selectionX, selectionY);
				return;
			}
			selectionX = ballX + 1; selectionY = ballY;
			if (checkLegalMove())
			{
				makeMove(selectionX, selectionY);
				return;
			}
			selectionX = ballX + 1; selectionY = ballY - 1;
			if (checkLegalMove())
			{
				makeMove(selectionX, selectionY);
				return;
			}
			selectionX = ballX; selectionY = ballY - 1;
			if (checkLegalMove())
			{
				makeMove(selectionX, selectionY);
				return;
			}
			selectionX = ballX - 1; selectionY = ballY - 1;
			if (checkLegalMove())
			{
				makeMove(selectionX, selectionY);
				return;
			}
		}
	}
}
