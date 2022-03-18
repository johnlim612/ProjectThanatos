using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class UIDialogue: MonoBehaviour {

        [SerializeField]
        private Sprite[] CharImages;

        [SerializeField]
        private Image ImageBox;

        // Start is called before the first frame update
        void Start() {
        }
        public void UpdateImage(string name) {
            foreach (Sprite image in CharImages) {
                if (image.name == name) {
                    ImageBox.sprite = image;
                }
            }
		}
    }
}
