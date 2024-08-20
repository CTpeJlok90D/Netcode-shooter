using UnityEngine;

namespace UI.Sightmark
{
    [Icon("Assets/_Project/UI/Sightmark/Editor/icons8-select-cursor-96.png")]
    public class Cursor : MonoBehaviour
    {
        private static Cursor _instance;

        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                throw new System.Exception("Cursor is singletone!");
            }
            _instance = this;
        }

        void OnEnable()
        {
            UnityEngine.Cursor.visible = true;
        }

        void OnDisable()
        {
            UnityEngine.Cursor.visible = false;
        }
    }
}
