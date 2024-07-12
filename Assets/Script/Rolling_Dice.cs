using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rolling_Dice : MonoBehaviour
{
    [SerializeField] Sprite[] NumberSprites;
    [SerializeField] SpriteRenderer NumberedSpriteHolder;
    [SerializeField] SpriteRenderer RollingDiceAnimation;
    [SerializeField] int NumberGot;

    Coroutine genrateRandomNumberonDice;
    public int outPieces;

    public PathObjectParent  PathParent;
    PlayerPiece[] currentPlayerPieces;
    PathPoint[] pathPointToMoveOn;
    Coroutine MovePlayerPiece;
    PlayerPiece outPlayerPiece;
    int maxNum=6;

    
    private void Awake()
    {
       PathParent = FindObjectOfType<PathObjectParent>();
    }

    

    
    public void OnMouseDown()
    {
      
       genrateRandomNumberonDice = StartCoroutine(RollingDice());
    }

     public void mouseRoll()
    {
      
       genrateRandomNumberonDice = StartCoroutine(RollingDice());
    }




     IEnumerator RollingDice()
     {
        GameManager.gm.transferDice = false;
        yield return new WaitForEndOfFrame();
     if (GameManager.gm.canDiceRoll) 
      {
        GameManager.gm.canDiceRoll = false;
        NumberedSpriteHolder.gameObject.SetActive(false);
        RollingDiceAnimation.gameObject.SetActive(true);
        yield return new WaitForSeconds(.5f);

        if(GameManager.gm.totalSix==2) { maxNum=5;}

        NumberGot = Random.Range(0,maxNum);
        if(NumberGot==6)
        {
          GameManager.gm.totalSix +=1;
        }
        else
        {
          GameManager.gm.totalSix=0;
        }
        NumberedSpriteHolder.sprite = NumberSprites[NumberGot];
        NumberGot += 1;

        GameManager.gm.NumberOfStepsToMove = NumberGot;
        GameManager.gm.RollingDice = this;

        NumberedSpriteHolder.gameObject.SetActive(true);
        RollingDiceAnimation.gameObject.SetActive(false);
        yield return new WaitForEndOfFrame();


        int numberGot=GameManager.gm.NumberOfStepsToMove;

        if(PlayerCantMove())
        {
          yield return new WaitForSeconds(.5f);

          if(numberGot!=6)
          {          
            GameManager.gm.transferDice = true;
          }
          else {GameManager.gm.selfDice= true;}
        }
        
        
      else
      {
        if(GameManager.gm.RollingDice==GameManager.gm.manageRollingDice[0]){ outPieces=GameManager.gm.blueOut;}
        else if(GameManager.gm.RollingDice==GameManager.gm.manageRollingDice[1]){ outPieces=GameManager.gm.redOut;}
        else if(GameManager.gm.RollingDice==GameManager.gm.manageRollingDice[2]){ outPieces=GameManager.gm.greenOut;}
        else if(GameManager.gm.RollingDice==GameManager.gm.manageRollingDice[3]){ outPieces=GameManager.gm.yellowOut;}
        

        if(outPieces==0 && numberGot!=6)
        {
          yield return new WaitForSeconds(.5f);
          GameManager.gm.transferDice = true;
        }
        else
        {
          if(outPieces==0 && numberGot==6)
          { 
            MakePlayerReadyToMove(0);
          }
          else if(outPieces==1 && numberGot!=6 && GameManager.gm.CanPlayerMove)
          { 
            int playerposition = CheckoutPlayer();
            if(playerposition>=0)
            {
              GameManager.gm.CanPlayerMove = false;
              MovePlayerPiece = StartCoroutine(MoveSteps_Enum(playerposition));
            }
            else
            {
               yield return new WaitForSeconds(.5f);

              if(numberGot!=6)
              {          
                GameManager.gm.transferDice = true;
              }
              else {GameManager.gm.selfDice= true;}
            }
            
           
          }
          else if(GameManager.gm.totalPlayerCanPlay==1 && GameManager.gm.RollingDice==GameManager.gm.manageRollingDice[2])
          {
            if(numberGot==6 && outPieces<=4)
            {
              MakePlayerReadyToMove(outPlayerToMove());
            }
            else 
            {
              int playerposition = CheckoutPlayer();
               if(playerposition>=0)
                {
                  GameManager.gm.CanPlayerMove = false;
                  MovePlayerPiece = StartCoroutine(MoveSteps_Enum(playerposition));
            
                }
                else
                {
                   yield return new WaitForSeconds(.5f);

                   if(numberGot!=6)
                   {          
                      GameManager.gm.transferDice = true;
                   }
                   else {GameManager.gm.selfDice= true;}
                }

            }
            
          }
          else
            {
              if(CheckoutPlayer()<0)
              {
                yield return new WaitForSeconds(.5f);

                   if(numberGot!=6)
                   {          
                      GameManager.gm.transferDice = true;
                   }
                   else {GameManager.gm.selfDice= true;}
              }
            }
        }


      } 

      GameManager.gm.RollingDiceManager();  

     

        if(genrateRandomNumberonDice != null)
        {
            StopCoroutine(RollingDice());
        }
      }
     }





   int outPlayerToMove()
   {
     for(int i=0;i<4;i++)
     {
      if(!GameManager.gm.greenplayerpiece[i].isReady)
      {
         return i;
      }
     }
      return 0;
   }

   int CheckoutPlayer()
   {
    
    if(GameManager.gm.RollingDice==GameManager.gm.manageRollingDice[0]){ currentPlayerPieces=GameManager.gm.blueplayerpiece;pathPointToMoveOn=PathParent.BluePathPoints;}
    else if(GameManager.gm.RollingDice==GameManager.gm.manageRollingDice[1]){ currentPlayerPieces=GameManager.gm.redplayerpiece;pathPointToMoveOn=PathParent.RedPathPoints;}
    else if(GameManager.gm.RollingDice==GameManager.gm.manageRollingDice[2]){ currentPlayerPieces=GameManager.gm.greenplayerpiece;pathPointToMoveOn=PathParent.GreenPathPoints;}
    else if(GameManager.gm.RollingDice==GameManager.gm.manageRollingDice[3]){ currentPlayerPieces=GameManager.gm.yellowplayerpiece;pathPointToMoveOn=PathParent.YellowPathPoints;}

     for(int i=0;i<currentPlayerPieces.Length;i++)
     {
       if(currentPlayerPieces[i].isReady && isPathPointAvailableToMove(GameManager.gm.NumberOfStepsToMove, currentPlayerPieces[i].NumberOfStepsAlreadyMove, pathPointToMoveOn))
          {
            return i;
          }
     }
     return -1; 
   }


   public bool PlayerCantMove()
   {
      if(outPieces>0)
      {
         bool cantMove = false;
         if(GameManager.gm.RollingDice==GameManager.gm.manageRollingDice[0]){ currentPlayerPieces=GameManager.gm.blueplayerpiece;pathPointToMoveOn = PathParent.BluePathPoints;}
        else if(GameManager.gm.RollingDice==GameManager.gm.manageRollingDice[1]){ currentPlayerPieces=GameManager.gm.redplayerpiece;pathPointToMoveOn = PathParent.RedPathPoints;}
        else if(GameManager.gm.RollingDice==GameManager.gm.manageRollingDice[2]){ currentPlayerPieces=GameManager.gm.greenplayerpiece;pathPointToMoveOn = PathParent.GreenPathPoints;}
        else if(GameManager.gm.RollingDice==GameManager.gm.manageRollingDice[3]){ currentPlayerPieces=GameManager.gm.yellowplayerpiece;pathPointToMoveOn = PathParent.YellowPathPoints;}
         
        
       for(int i = 0;i<currentPlayerPieces.Length;i++)
       {
         if(currentPlayerPieces[i].isReady)
         {
            if(isPathPointAvailableToMove(GameManager.gm.NumberOfStepsToMove, currentPlayerPieces[i].NumberOfStepsAlreadyMove, pathPointToMoveOn))
            {
               return false;
            }
         }
         else
         {
            if(!cantMove)
            {cantMove=true;}
         }
        }
         if (cantMove)
         {
            return true;
         }

        
      }
     return false;
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

   public void MakePlayerReadyToMove(int outPlayer)
    {
      if(GameManager.gm.RollingDice==GameManager.gm.manageRollingDice[0]){ outPlayerPiece=GameManager.gm.blueplayerpiece[outPlayer];pathPointToMoveOn=PathParent.BluePathPoints;GameManager.gm.blueOut+=1;}
      else if(GameManager.gm.RollingDice==GameManager.gm.manageRollingDice[1]){ outPlayerPiece=GameManager.gm.redplayerpiece[outPlayer];pathPointToMoveOn=PathParent.RedPathPoints;GameManager.gm.redOut+=1;}
      else if(GameManager.gm.RollingDice==GameManager.gm.manageRollingDice[2]){ outPlayerPiece=GameManager.gm.greenplayerpiece[outPlayer];pathPointToMoveOn=PathParent.GreenPathPoints;GameManager.gm.greenOut+=1;}
      else if(GameManager.gm.RollingDice==GameManager.gm.manageRollingDice[3]){ outPlayerPiece=GameManager.gm.yellowplayerpiece[outPlayer];pathPointToMoveOn=PathParent.YellowPathPoints;GameManager.gm.yellowOut+=1;}
        
        outPlayerPiece.isReady = true; 
        outPlayerPiece.transform.position = pathPointToMoveOn[0].transform.position;
        outPlayerPiece.NumberOfStepsAlreadyMove = 1;

        outPlayerPiece.Previouspathpoint = pathPointToMoveOn[0];
        outPlayerPiece.Currentpathpoint = pathPointToMoveOn[0];
        outPlayerPiece.Currentpathpoint.AddPlayerPiece(outPlayerPiece);

        GameManager.gm.AddPathPoint(outPlayerPiece.Currentpathpoint);

        GameManager.gm.canDiceRoll = true;
        GameManager.gm.selfDice = true;
        GameManager.gm.transferDice = false;
        GameManager.gm.NumberOfStepsToMove = 0;
        GameManager.gm.SelfRoll();
    }

    IEnumerator MoveSteps_Enum(int movePlayer)
  {

    if(GameManager.gm.RollingDice==GameManager.gm.manageRollingDice[0]){ outPlayerPiece=GameManager.gm.blueplayerpiece[movePlayer];pathPointToMoveOn=PathParent.BluePathPoints;}
    else if(GameManager.gm.RollingDice==GameManager.gm.manageRollingDice[1]){ outPlayerPiece=GameManager.gm.redplayerpiece[movePlayer];pathPointToMoveOn=PathParent.RedPathPoints;}
    else if(GameManager.gm.RollingDice==GameManager.gm.manageRollingDice[2]){ outPlayerPiece=GameManager.gm.greenplayerpiece[movePlayer];pathPointToMoveOn=PathParent.GreenPathPoints;}
    else if(GameManager.gm.RollingDice==GameManager.gm.manageRollingDice[3]){ outPlayerPiece=GameManager.gm.yellowplayerpiece[movePlayer];pathPointToMoveOn=PathParent.YellowPathPoints;}
      



    GameManager.gm.transferDice = false;
    yield return new WaitForSeconds(0.3f);
    int NumberOfStepsToMove =GameManager.gm.NumberOfStepsToMove;


   for(int i=outPlayerPiece.NumberOfStepsAlreadyMove;i<(outPlayerPiece.NumberOfStepsAlreadyMove+NumberOfStepsToMove);i++)
   {
     

     if(isPathPointAvailableToMove(NumberOfStepsToMove,outPlayerPiece.NumberOfStepsAlreadyMove,pathPointToMoveOn))
     {
       outPlayerPiece.transform.position = pathPointToMoveOn[i].transform.position;
       yield return new WaitForSeconds(0.35f);
     }

    }

    if(isPathPointAvailableToMove(NumberOfStepsToMove,outPlayerPiece.NumberOfStepsAlreadyMove,pathPointToMoveOn))
    {
      
      outPlayerPiece.NumberOfStepsAlreadyMove += NumberOfStepsToMove;
     

     GameManager.gm.RemovePathPoint(outPlayerPiece.Previouspathpoint);
     outPlayerPiece.Previouspathpoint.RemovePlayerPiece(outPlayerPiece);
     outPlayerPiece.Currentpathpoint = pathPointToMoveOn[outPlayerPiece.NumberOfStepsAlreadyMove-1];

     if(outPlayerPiece.Currentpathpoint.AddPlayerPiece(outPlayerPiece))
     {
       if(outPlayerPiece.NumberOfStepsAlreadyMove==57)
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
     
     GameManager.gm.AddPathPoint(outPlayerPiece.Currentpathpoint);
     outPlayerPiece.Previouspathpoint = outPlayerPiece.Currentpathpoint;

     
     GameManager.gm.NumberOfStepsToMove = 0;

    }   
    GameManager.gm.CanPlayerMove = true;

    GameManager.gm.RollingDiceManager();

    if(MovePlayerPiece != null)
    {
     StopCoroutine("MoveSteps_Enum");
    }
 }



}
