using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BluePlayerPiece : PlayerPiece
{
   
  Rolling_Dice blueHomeRolling;


  void Start()
  {
     blueHomeRolling = GetComponentInParent<BlueHome>().rollingDice;

  }

  public void OnMouseDown()
  {
    if(GameManager.gm.RollingDice != null)
    {
      if(!isReady)
      {
        if(GameManager.gm.RollingDice==blueHomeRolling && GameManager.gm.NumberOfStepsToMove==6)
        {
          GameManager.gm.blueOut+=1;
         MakePlayerReadyToMove(PathParent.BluePathPoints);
         GameManager.gm.NumberOfStepsToMove = 0;

         return;
        }
      }
        
      if(GameManager.gm.RollingDice == blueHomeRolling && isReady && GameManager.gm.CanPlayerMove)
      {
       GameManager.gm.CanPlayerMove = false;
       MoveSteps(PathParent.BluePathPoints);
      }
        
    }
  }

}
      
 
