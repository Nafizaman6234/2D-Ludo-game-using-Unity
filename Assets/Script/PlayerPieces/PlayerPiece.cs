using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPiece : MonoBehaviour
{

    public bool isReady; 
    public bool MoveNow;
    public int NumberOfStepsToMove;
    public int NumberOfStepsAlreadyMove;

    public PathObjectParent PathParent;
    Coroutine MovePlayerPiece;

    public PathPoint Previouspathpoint;
    public PathPoint Currentpathpoint;

    private void Awake()
    {
        PathParent = FindObjectOfType<PathObjectParent>();

    }


    public void MoveSteps(PathPoint[] PathPointToMoveOn)
    {
      MovePlayerPiece = StartCoroutine(MoveSteps_Enum(PathPointToMoveOn));
    }

    public void MakePlayerReadyToMove(PathPoint[] PathPointToMoveOn)
    {
        isReady = true; 
        transform.position = PathPointToMoveOn[0].transform.position;
        NumberOfStepsAlreadyMove = 1;

        Previouspathpoint = PathPointToMoveOn[0];
        Currentpathpoint = PathPointToMoveOn[0];
        Currentpathpoint.AddPlayerPiece(this);

        GameManager.gm.AddPathPoint(Currentpathpoint);

        GameManager.gm.canDiceRoll = true;
        GameManager.gm.selfDice = true;
        GameManager.gm.transferDice = false;
    }


    IEnumerator MoveSteps_Enum(PathPoint[] PathPointToMoveOn)
  {
    GameManager.gm.transferDice = false;
    yield return new WaitForSeconds(0.2f);
    NumberOfStepsToMove =GameManager.gm.NumberOfStepsToMove;


   for(int i=NumberOfStepsAlreadyMove;i<(NumberOfStepsAlreadyMove+NumberOfStepsToMove);i++)
   {
     

     if(isPathPointAvailableToMove(NumberOfStepsToMove,NumberOfStepsAlreadyMove,PathPointToMoveOn))
     {
       transform.position = PathPointToMoveOn[i].transform.position;
       yield return new WaitForSeconds(0.35f);
     }

    }

    if(isPathPointAvailableToMove(NumberOfStepsToMove,NumberOfStepsAlreadyMove,PathPointToMoveOn))
    {
      
      NumberOfStepsAlreadyMove += NumberOfStepsToMove;
     

     GameManager.gm.RemovePathPoint(Previouspathpoint);
     Previouspathpoint.RemovePlayerPiece(this);
     Currentpathpoint = PathPointToMoveOn[NumberOfStepsAlreadyMove-1];

     if(Currentpathpoint.AddPlayerPiece(this))
     {
       if(NumberOfStepsAlreadyMove==57)
       {
         GameManager.gm.selfDice = true;
       }
       else
       {
         if(GameManager.gm.NumberOfStepsToMove!=6)
          {  
            GameManager.gm.transferDice = true;
          }    
          else
          {
           GameManager.gm.selfDice = true;
          }
       }
     }
     else
     {
       GameManager.gm.selfDice = true;
     }
     
     GameManager.gm.AddPathPoint(Currentpathpoint);
     Previouspathpoint = Currentpathpoint;

     
     GameManager.gm.NumberOfStepsToMove = 0;

    }   
    GameManager.gm.CanPlayerMove = true;

    GameManager.gm.RollingDiceManager();

    if(MovePlayerPiece != null)
    {
     StopCoroutine("MoveSteps_Enum");
    }
 }
    
    bool isPathPointAvailableToMove( int NumberOfStepsToMove,int NumberOfStepsAlreadyMove,PathPoint[] PathPointToMove)
    {
      if (NumberOfStepsToMove ==0)
      {
        return false;
      }
      int leftNumberOfPath = PathPointToMove.Length - NumberOfStepsAlreadyMove;
      if(leftNumberOfPath >= NumberOfStepsToMove)
      {
        return true;
      }
      else
      {
        return false;
      }


    }



}
