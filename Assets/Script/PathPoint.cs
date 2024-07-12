using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPoint : MonoBehaviour
{

    PathPoint [] PathPointToMoveOn;
    public PathObjectParent pathObjectParent;
    public List <PlayerPiece> PlayerPieceList = new List<PlayerPiece>();
    // Start is called before the first frame update
    void Start()
    {
      pathObjectParent= GetComponentInParent<PathObjectParent>(); 
    }

    public bool AddPlayerPiece(PlayerPiece playerpiece)
    {

       if(this.name=="center_path") { Completed(playerpiece);}

      if(this.name!="PathPoint" && this.name!="PathPoint (8)" && this.name!="PathPoint (13)" && this.name!="PathPoint (21)"
          && this.name!="PathPoint (26)" && this.name!="PathPoint (34)" && this.name!="PathPoint (39)" && this.name!="PathPoint (47)" && this.name!="center_path")
      {
        if(PlayerPieceList.Count==1)
        {
           string prevPlayerName = PlayerPieceList[0].name;
           string curPlayerName = playerpiece.name;
           curPlayerName = curPlayerName.Substring(0, curPlayerName.Length-4);

           if(!prevPlayerName.Contains(curPlayerName))
           {
              PlayerPieceList[0].isReady = false;
              
              StartCoroutine (revertOnStart(PlayerPieceList[0]));
             

              PlayerPieceList[0].NumberOfStepsAlreadyMove=0;
              RemovePlayerPiece(PlayerPieceList[0]);
              PlayerPieceList.Add(playerpiece);
              return false;
           }
        }
      }


       addplayer(playerpiece);
       return true;

    }

    IEnumerator revertOnStart(PlayerPiece playerpiece)
    {
        if(playerpiece.name.Contains("Blue")) {GameManager.gm.blueOut -= 1; PathPointToMoveOn= pathObjectParent.BluePathPoints;}
        else if(playerpiece.name.Contains("Red")) {GameManager.gm.redOut -= 1;PathPointToMoveOn= pathObjectParent.RedPathPoints;}
        else if(playerpiece.name.Contains("Green")) {GameManager.gm.greenOut -= 1;PathPointToMoveOn= pathObjectParent.GreenPathPoints;}
        else if(playerpiece.name.Contains("Yellow")) {GameManager.gm.yellowOut -= 1;PathPointToMoveOn= pathObjectParent.YellowPathPoints;}
       
       for(int i=playerpiece.NumberOfStepsAlreadyMove;i>=0;i--)
       {
         playerpiece.transform.position=PathPointToMoveOn[i].transform.position;
         yield return new WaitForSeconds(0.02f);
       }
       playerpiece.transform.position = pathObjectParent.BasePoint[BasePointPosition(playerpiece.name)].transform.position;
       
    }

    int BasePointPosition(string name)
    {
     
        for(int i =0;i<pathObjectParent.BasePoint.Length;i++)
        {
            if(pathObjectParent.BasePoint[i].name==name)
            {
                return i;
            }
        } 
        return -1; 
    }

    void addplayer(PlayerPiece playerpiece)
    {
        PlayerPieceList.Add(playerpiece);
        RescaleandRepositionAllPlayerPiece();
    }

    

    public void RemovePlayerPiece(PlayerPiece playerpiece)
    {
        if(PlayerPieceList.Contains(playerpiece))
        {
            PlayerPieceList.Remove(playerpiece);
            RescaleandRepositionAllPlayerPiece();

        }


    }

    private void Completed(PlayerPiece playerpiece)
    {
       if(playerpiece.name.Contains("Blue")) {GameManager.gm.blueComplete += 1;GameManager.gm.blueOut -= 1; if(GameManager.gm.blueComplete==4){ShowCeleb();}}
        else if(playerpiece.name.Contains("Red")) {GameManager.gm.redComplete += 1;GameManager.gm.redOut -= 1;if(GameManager.gm.redComplete==4){ShowCeleb();}}
        else if(playerpiece.name.Contains("Green")) {GameManager.gm.greenComplete += 1;GameManager.gm.greenOut -= 1;if(GameManager.gm.greenComplete==4){ShowCeleb();}}
        else if(playerpiece.name.Contains("Yellow")) {GameManager.gm.yellowComplete += 1;GameManager.gm.yellowOut -= 1;if(GameManager.gm.yellowComplete==4){ShowCeleb();}}
         
    }
    void ShowCeleb()
    {
      //ShowCeleb()  
    }

    public void RescaleandRepositionAllPlayerPiece()
    {

        int playerCount = PlayerPieceList.Count;
        bool isOdd =(playerCount %2)==0?false:true;

        int extend = playerCount/2;
        int counter =0;
        int spriteLayer =0;

        if (isOdd)
        {
            for(int i= -extend;i <= extend;i++)
            {
                //PlayerPieceList[counter].transform.localScale= new Vector3(pathObjectParent.scales[playerCount-1],pathObjectParent.scales[playerCount - 1], 1f);
                PlayerPieceList[counter].transform.position = new Vector3(transform.position.x + (i* pathObjectParent.positionDifference[playerCount-1]),transform.position.y, 0f);
                counter++;
            }
        }
        else
        {
            for(int i= -extend;i < extend;i++)
            {
                //PlayerPieceList[counter].transform.localScale= new Vector3(pathObjectParent.scales[playerCount-1],pathObjectParent.scales[playerCount-1], 1f);
                PlayerPieceList[counter].transform.position = new Vector3(transform.position.x + (i* pathObjectParent.positionDifference[playerCount-1]),transform.position.y, 0f);
                counter++;
            }
        }

        for (int i=0;i<PlayerPieceList.Count;i++)
        {
            PlayerPieceList[i].GetComponentInChildren<SpriteRenderer>().sortingOrder = spriteLayer;
            spriteLayer++;
        }
    }


}
