using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenPlayerPiece : PlayerPiece
{

  Rolling_Dice greenHomeRolling;


  void Start()
  {
   greenHomeRolling = GetComponentInParent<GreenHome>().rollingDice;

  }
    

  public void OnMouseDown()
  {
    if(GameManager.gm.RollingDice != null)
    {
     if(!isReady)
      {
        if(GameManager.gm.RollingDice==greenHomeRolling && GameManager.gm.NumberOfStepsToMove==6)
        {
          GameManager.gm.greenOut+=1;
         MakePlayerReadyToMove(PathParent.GreenPathPoints);
         GameManager.gm.NumberOfStepsToMove = 0;

         return;
        }
      }

     if(GameManager.gm.RollingDice == greenHomeRolling && isReady && GameManager.gm.CanPlayerMove)
     {
      GameManager.gm.CanPlayerMove = false;
      MoveSteps(PathParent.GreenPathPoints);
     }
      
    }
  }

}
