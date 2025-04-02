using Unity.VisualScripting;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
   private GameObject[] noteObjects;
   private GameObject Layer1;
   private GameObject Layer2;
   private int Holder = 0;
   public int Index = 0;
   public bool Dragging = false;
   private NoteDrag[] noteDrag;
   private GameObject CurrentNote;
   public int NoteIndex;
   public bool MultDragging;
   public int DragNum = 0;
   private bool wait = false;
   private bool Layer2Object = false;
   private int WaitIndex = 0;
   
  
   

   private void Start()
   {
      Layer1 = GameObject.Find("Layer1");
      Layer2 = GameObject.Find("Layer2");
      noteDrag = FindObjectsOfType<NoteDrag>();
      noteObjects = GameObject.FindGameObjectsWithTag("NotePadHud");
      CurrentNote = noteObjects[0];
      int i = 0;
      foreach (GameObject noteObject in noteObjects)
      {
         noteObject.transform.SetParent(Layer1.transform);
         Debug.Log(noteObject.name + i);
         noteObject.SetActive(false);
         i++;
      }
      
   }

   private void Update()
   {
      
      GameObject LastDrag = null;
      for (int i = 0; i < noteDrag.Length; i++)
      {
         if (noteDrag[i].isDragging)
         {
            if (noteDrag[i].GameObject().transform.parent == Layer1.transform)
            {
               WaitIndex = i;
               wait = true;
            }
            else
            {
               LastDrag = noteDrag[i].gameObject;
               LayerChange(noteObjects[i], Layer2);
               Layer2Object = true;
            }
            
         }
      }

      if (wait)
      {
         if (!Layer2Object)
         {
            LayerChange(noteObjects[WaitIndex], Layer2);
         }
         wait = false;
         Layer2Object = false;
         WaitIndex = 0;
      }
      for (int i = 0; i < noteDrag.Length; i++)
      {
         if (LastDrag != null)
         {
            if (!noteDrag[i].isDragging && noteDrag[i].gameObject != LastDrag)
            {
               LayerChange(noteObjects[i], Layer1);
            }
         }
      }

      DragNum = 0;
      //Make sure only one object can be dragged at once
      for (int note = 0; note < noteDrag.Length; note++)
      {
         if (noteDrag[note].IndivDragging)
         {
            DragNum++;
         }
      }

      if (DragNum > 1)
      {
         MultDragging = true;
      }
      else
      {
         MultDragging = false;
      }

   }




   public void LayerChange(GameObject Tab, GameObject Layer)
   {
      Tab.transform.SetParent(Layer.transform);
   }

   public void NoteOn(GameObject note)
   {
      note.transform.SetParent(Layer2.transform);
      note.SetActive(true);
   }

   public void NoteOff(GameObject note)
   {
      note.SetActive(false);
   }
}
