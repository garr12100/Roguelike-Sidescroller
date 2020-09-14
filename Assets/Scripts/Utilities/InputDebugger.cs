using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputDebugger : MonoBehaviour
{
    public List<DebugInput> debugInputs = new List<DebugInput>();
	
	// Update is called once per frame
	void Update ()
    {
        foreach (DebugInput debugInput in debugInputs)
        {
            switch (debugInput.type)
            {
                case DebugInput.InputType.Down:
                    if (Input.GetButtonDown(debugInput.input))
                        Debug.Log("ButtonDown: " + debugInput.input);
                    break;
                case DebugInput.InputType.Hold:
                    if (Input.GetButton(debugInput.input))
                        Debug.Log("ButtonHold: " + debugInput.input);
                    break;
                case DebugInput.InputType.Up:
                    if (Input.GetButtonUp(debugInput.input))
                        Debug.Log("ButtonUp: " + debugInput.input);
                    break;
            }
        }
	}
}

[System.Serializable]
public class DebugInput
{
    public enum InputType { Down, Hold, Up };
    public string input;
    public InputType type;
}
