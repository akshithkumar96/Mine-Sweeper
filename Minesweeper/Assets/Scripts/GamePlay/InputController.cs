using System;
using UnityEngine;

namespace Com.Minesweeper.Gameplay
{
    public class InputController : MonoBehaviour
    {
        public static Action<KeyCode> OnKeyBoardkeyDown;
        public static Action<int> MouseButtonDown;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                OnKeyBoardkeyDown?.Invoke(KeyCode.S);
            }
            if (Input.GetMouseButtonDown(1))
            {
                MouseButtonDown?.Invoke(1);
            }
            else if (Input.GetMouseButtonDown(0))
            {
                MouseButtonDown?.Invoke(0);
            }
        }
    }
}