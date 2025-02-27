using UnityEngine;
using UnityEngine.InputSystem;

public class InputTest : MonoBehaviour
{
    private void Update()
    {
        if (Keyboard.current != null)
        {
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                Debug.Log("E key pressed");
            }

            if (Keyboard.current.fKey.wasPressedThisFrame)
            {
                Debug.Log("F key pressed");
            }
        }
    }
}
