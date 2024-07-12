using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedPlayerPiece : PlayerPiece
{

  Rolling_Dice redHomeRolling;


  void Start()
  {
   redHomeRolling = GetComponentInParent<RedHome>().rollingDice;

  }
  
  public void OnMouseDown()
  {
    if(GameManager.gm.RollingDice != null)
    {
     if(!isReady)
      {
        if(GameManager.gm.RollingDice==redHomeRolling && GameManager.gm.NumberOfStepsToMove==6)
        {
          GameManager.gm.redOut+=1;
         MakePlayerReadyToMove(PathParent.RedPathPoints);
         GameManager.gm.NumberOfStepsToMove = 0;

         return;
        }
      }

     if(GameManager.gm.RollingDice == redHomeRolling && isReady && GameManager.gm.CanPlayerMove)
     {
      GameManager.gm.CanPlayerMove = false;
      MoveSteps(PathParent.RedPathPoints);
     }
      
    }
  }

}
