using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class DialogueUI: MonoBehaviour {

        [SerializeField]
        private Sprite[] _charImages;

        [SerializeField]
        private Image _imageBox;

        [Header("DialogueBoxes")]
        public Button[] Buttons;

        public Text NameText;
        public Text DialogueText;
        public Button NextButton;
        public Animator Animator;

        // Start is called before the first frame update
        void Awake() {

        }

        public void UpdateImage(string name) {
            foreach (Sprite image in _charImages) {
                if (image.name == name) {
                    _imageBox.sprite = image;
                }
            }
		}
    }
}
