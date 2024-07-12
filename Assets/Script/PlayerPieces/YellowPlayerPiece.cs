using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowPlayerPiece : PlayerPiece
{

  Rolling_Dice yellowHomeRolling;


  void Start()
  {
    yellowHomeRolling = GetComponentInParent<YellowHome>().rollingDice;

  }
   
 

  public void OnMouseDown()
  {
    if(GameManager.gm.RollingDice != null)
    {
     if(!isReady)
      {
        if(GameManager.gm.RollingDice==yellowHomeRolling && GameManager.gm.NumberOfStepsToMove==6)
        {
          GameManager.gm.yellowOut+=1;
         MakePlayerReadyToMove(PathParent.YellowPathPoints);
         GameManager.gm.NumberOfStepsToMove = 0;

         return;
        }
      }

     if(GameManager.gm.RollingDice == yellowHomeRolling && isReady && GameManager.gm.CanPlayerMove)
     {
      GameManager.gm.CanPlayerMove = false;
      MoveSteps(PathParent.YellowPathPoints);
     }
      
    }
  }


}
