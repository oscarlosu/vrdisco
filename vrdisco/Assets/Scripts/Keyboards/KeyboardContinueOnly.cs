using Normal.UI;
using UnityEngine;

namespace Keyboards
{
    public class KeyboardContinueOnly : Keyboard
    {
        public static Keyboard Instance { get; private set; }

        protected override void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            base.Awake();
        }

        // Internal
        public override void _MalletStruckKeyboardKey(KeyboardMallet mallet, KeyboardKey key)
        {
            // Did we hit the key for another keyboard?
            if (key._keyboard != this)
                return;

            // Trigger key press animation
            key.KeyPressed();

            string keyPress = key.GetCharacter();

            if (keyPress == "\\continue")
            {
                // Continue to next game state.
                if (GameHandler.Instance != null)
                {
                    GameHandler.Instance.NextState();
                    return;
                }
            }
        }
    }
}
