using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameBehavior : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI messageGui, titleGui;
    [SerializeField] GameObject
        backdrop,
        floatie,
        ghost,
        health,
        shield;
    [SerializeField] Transform PowerupParent;
    [SerializeField] float powerupFreq = 2.0f;

    public Frog[] Frogs = new Frog[2];

    public static GameBehavior Instance;

    public int
        xBound=4,
        yBoundDown=3,
        yBoundUp=12;
    public State CurrentState;

    float camY;

    void Awake()
    {
        //singleton Instance of the GameBehavior Object
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        //On awake, set the Message GUI to poke the player to start the game.
        //Set the current state to be the Title state
        backdrop.SetActive(true);
        GuiBehavior.Instance.UpdateMessageGUI("Press Space to Start Game", messageGui, true);
        CurrentState = State.Title;
        camY = Camera.main.transform.position.y;
    }

    private void Update()
    {

        //Toggle states based upon what state the game is currently in.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (CurrentState.Equals(State.Title))
            {
                GuiBehavior.Instance.ToggleGuiVisibility(titleGui);
                GuiBehavior.Instance.UpdateMessageGUI("Leap to the top of the screen. " +
                    "If you are overcome by the bottom of the screen, you will lose a life. " +
                    "Floaties(orange) will help you pass water. " +
                    "Ghost(pink) will help you phase through cars. " +
                    "Health packs will restore a life (up to 3). " +
                    "Press Space to begin and P to pause.", messageGui, false);
                CurrentState = State.Start;
            }
            else if (CurrentState.Equals(State.Start))
            {
                GuiBehavior.Instance.ToggleGuiVisibility(messageGui);
                backdrop.SetActive(false);
                StartCoroutine("SpawnPowerupTimer");
                CurrentState = State.Play;
            }
            else if (CurrentState.Equals(State.GameOver))
            {
                GuiBehavior.Instance.ToggleGuiVisibility(titleGui);
                GuiBehavior.Instance.ToggleGuiVisibility(messageGui);
                GuiBehavior.Instance.UpdateMessageGUI("Press Space to Start Game", messageGui, false);
                Frogs[0].Lives = 3;
                Frogs[1].Lives = 3;
                CurrentState = State.Title;
            }
        }
        //Pause button implementation
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (CurrentState == State.Play || CurrentState == State.Pause)
            {
                CurrentState = CurrentState == State.Play ? State.Pause : State.Play;
                backdrop.SetActive(false);
                GuiBehavior.Instance.UpdateMessageGUI("Paused", GameBehavior.Instance.messageGui, true);
            }
        }
        camY = Camera.main.transform.position.y;
    }
    //If a frog dies, make the frog player lose a life;
    //and if one frog loses all of their lives, then declare which
    //frog has won the game while changing the state to GameOver
    public void UpdateLives()
    {
        foreach (Frog pl in Frogs)
        {
            if (pl.Lives <= 0)
            {
                CurrentState = State.GameOver;
                backdrop.SetActive(true);
                GuiBehavior.Instance.UpdateMessageGUI(pl + " Lose, Press Space to Return to Title", GameBehavior.Instance.messageGui, true);
            }
        }
    }

    IEnumerator SpawnPowerupTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(powerupFreq);
            SpawnPowerup();
        }
    }

    void SpawnPowerup()
    {
        //GameObject spawningPowerup = floatie;

        int
            powerupNum = Random.Range(0, 9),
            randX = Random.Range(-xBound, xBound),
            randY = Random.Range(yBoundDown, yBoundUp);

        randY += (int)camY;

        Vector3 powerupPosition = new Vector3(randX, randY, 1);

        switch (powerupNum)
        {
            case 0:
            case 1:
            case 2:
                Instantiate(
                    floatie,
                    powerupPosition,
                    Quaternion.identity,
                    PowerupParent);
                break;
            case 3:
            case 4:
            case 5:
                Instantiate(
                    ghost,
                    powerupPosition,
                    Quaternion.identity,
                    PowerupParent);
                break;
            case 6:
            case 7:
                Instantiate(
                    health,
                    powerupPosition,
                    Quaternion.identity,
                    PowerupParent);
                break;
            case 8:
                Instantiate(
                    shield,
                    powerupPosition,
                    Quaternion.identity,
                    PowerupParent);
                break;
        }
    }

}


