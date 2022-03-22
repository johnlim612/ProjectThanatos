using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class DialogueUI: MonoBehaviour {

        [SerializeField]
        private Sprite[] _charImages;

        [SerializeField]
        public Image ImageBox;    

        [Header("DialogueBoxes")]
        public Button[] Buttons;

        public Text NameText;
        public Text DialogueText;
        public Button NextButton;
        public Animator Animator;

        // Start is called before the first frame update
        void Awake() {
            ImageBox.enabled = false;
        }

        public void UpdateImage(string name) {
            foreach (Sprite image in _charImages) {
                if (image.name == name) {
                    ImageBox.sprite = image;
                    ImageBox.enabled = true;
                }
            }
		}
    }
}
