using Unity.VisualScripting;
using UnityEngine;

public class NoteManager1 : MonoBehaviour
{
   private GameObject[] noteObjects;
   [SerializeField] private MouseDragScript mouseDragScript;
   [SerializeField] private SoundDrag soundDragScript;
   private GameObject Layer1;
   private GameObject Layer2;
   private int Holder = 0;
   public int Index = 0;
   public bool Dragging = false;

   private GameObject CurrentNote;
   public int NoteIndex;

   public int DragNum = 0;
   private bool wait = false;
   private bool Layer2Object = false;
   private int WaitIndex = 0;




   private void Start()
   {
      Layer1 = GameObject.Find("Layer1");
      Layer2 = GameObject.Find("Layer2");

      noteObjects = GameObject.FindGameObjectsWithTag("NotePadHud");
      CurrentNote = noteObjects[0];
      int i = 0;
      foreach (GameObject noteObject in noteObjects)
      {
         noteObject.transform.SetParent(Layer1.transform);
         Debug.Log(noteObject.name + i);
         
         i++;
      }

   }

   private void Update()
   {

      GameObject LastDrag = null;
      if (mouseDragScript.isDragging)
      {
         noteObjects[0].transform.SetParent(Layer1.transform);
         noteObjects[1].transform.SetParent(Layer2.transform);
      }
      else if (soundDragScript.isDragging)
      {
         noteObjects[1].transform.SetParent(Layer1.transform);
         noteObjects[0].transform.SetParent(Layer2.transform);
      }
   }
}