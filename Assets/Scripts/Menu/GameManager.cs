using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Menu
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance = null;

        private UIController _uiScript;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
            _uiScript = GetComponent<UIController>();
        }
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

