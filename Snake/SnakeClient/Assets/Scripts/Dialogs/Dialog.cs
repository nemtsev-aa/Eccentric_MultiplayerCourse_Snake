using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class Dialog : MonoBehaviour {
        [SerializeField] private Button _outsideClickArea;

        public virtual void Awake() {
            if (_outsideClickArea != null) {
                _outsideClickArea.onClick.AddListener(Hide);
            }
           
        }

        public virtual void Hide() {
            Destroy(gameObject);
        }

        public virtual void OnDestroy() {
            if (_outsideClickArea != null) {
                _outsideClickArea.onClick.RemoveAllListeners();
            }
        }
    }
}

