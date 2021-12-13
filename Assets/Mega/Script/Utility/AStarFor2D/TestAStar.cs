using System;
using System.Collections;
using UnityEngine; 
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class SceneGrid : MonoBehaviour
{

}

 public class TestAStar : MonoBehaviour  
 {
     public Camera mainCamera; 
    

     public GameObject cubeObject; 
     public GameObject pathObject;    
     public SceneGrid sceneGrid;
 
     private AStarUtils aStarUtils; 
     private AStarNode beginNode; 

     private int cols = 20; 
     private int rows = 20;    

     private IList<GameObject> pathList;

     private List<List<int>> infoArray = new List<List<int>>();
    

     void Awake() 
     { 
         for(int i=0; i<20; i++)
         {
             List<int> tmp = new List<int>();
             for (int j = 0; j < 20; j++)
             {
                 tmp.Add(0);
             }
             infoArray.Add(tmp);
         }
         for(int i = 0; i < 16; i++)
         {
             infoArray[1][i] = 10;
             infoArray[3][19-i] = 10;        
             infoArray[5][i+1] = 10;
             infoArray[10][i] = 10;
             infoArray[13][(i % 9) + 3] = 10;
         }


         this.pathList = new List<GameObject> (); 
         this.aStarUtils = new AStarUtils (this.cols, this.rows);   

         // cols 
         for(int i = 0; i < this.cols; i++) 
         { 
             // rows 
             for(int j = 0; j < this.rows; j++) 
             { 
                 AStarUnit aStarUnit = new AStarUnit();

                 if(infoArray[i][j] == 0) 
                 {
                     aStarUnit.isPassable = true; 
                     
                 }
                 else
                 { 
                     aStarUnit.isPassable = false;

                     GameObject gameObject = (GameObject)Instantiate(cubeObject);
                     gameObject.name = "cube_" + i + "_" + j;
                     if(gameObject != null)
                     {
                         gameObject.transform.localPosition = new Vector3(i * 5f + 2.5f, (cols - j - 1) * 5f + 2.5f, 0f);
                     } 
                 } 
                 this.aStarUtils.GetNode(i,j).AddUnit(aStarUnit); 
             } 
         }

         beginNode = aStarUtils.GetNode(0,0); 
     } 
  

     private void FindPath(int x, int y) 
     { 
         AStarNode endNode = this.aStarUtils.GetNode(x, y); 
         if (this.beginNode == null)  
         { 
             this.beginNode = endNode; 
             return; 
         }   

         if (this.pathList != null && this.pathList.Count > 0)  
         { 
             foreach (GameObject xxObject in this.pathList)  
             { 
                 Destroy(xxObject); 
             } 
         } 
        
         if(endNode != null && endNode.walkable) 
         { 
             System.DateTime dateTime = System.DateTime.Now;   

             IList<AStarNode> pathList = this.aStarUtils.FindPath(this.beginNode, endNode); 
             System.DateTime currentTime = System.DateTime.Now; 
             System.TimeSpan timeSpan = currentTime.Subtract(dateTime); 
             Debuger.Log(timeSpan.Seconds + "秒" + timeSpan.Milliseconds + "毫秒");

             if(pathList != null && pathList.Count > 0) 
             { 
                 foreach(AStarNode nodeItem in pathList) 
                 { 
                     GameObject gameObject = (GameObject)Instantiate(pathObject); 
                     this.pathList.Add(gameObject); 
                    
                     gameObject.transform.localPosition = new Vector3(nodeItem.nodeX * 5f+2.5f, (cols-nodeItem.nodeY - 1) * 5f+2.5f, 0f); 
                 } 
             } 
             //this.beginNode = endNode; 
         } 
     }   

     void Update() 
     { 
         if (Input.GetMouseButtonDown (0))
         {
             Vector3 pos = mainCamera.ScreenToWorldPoint(Input.mousePosition);// ScreenToViewportPoint(Input.mousePosition);
             Debuger.Log("click pos: "+pos);

             int colIndex = (int)(pos.x/5.0f);
             int rowIndex = (int)(rows - pos.y/5.0f);
             FindPath(colIndex, rowIndex); 
            
         } 

     } 

 } 