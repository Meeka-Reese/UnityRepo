using UnityEngine;
using Ink.Runtime;

public class DialogueVariables : MonoBehaviour
{
    public bool ImageChoice = false;
    public void StartListening(Story story)
    {
        story.variablesState.variableChangedEvent += VariableChanged;
        
    }

    public void StopListening(Story story)
    {
        story.variablesState.variableChangedEvent -= VariableChanged;
    }

    private void VariableChanged(string name, Ink.Runtime.Object value)
    {
        if (name == "ImageChoice")
        {
            ImageChoice = true;
        }
        Debug.Log("Variable changed: " + name + " value: " + value);
    }
}
