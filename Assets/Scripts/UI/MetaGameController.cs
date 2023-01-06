using Platformer.Mechanics;
using Platformer.UI;
using UnityEngine;

namespace Platformer.UI
{
    /// <summary>
    /// The MetaGameController is responsible for switching control between the high level
    /// contexts of the application, eg the Main Menu and Gameplay systems.
    /// </summary>
    public class MetaGameController : MonoBehaviour
    {
        /// <summary>
        /// The main UI object which used for the menu.
        /// </summary>
        public GameObject mainMenu;

        /// <summary>
        /// A list of canvas objects which are used during gameplay (when the main ui is turned off)
        /// </summary>
        public Canvas[] gamePlayCanvasii;

        /// <summary>
        /// The game controller.
        /// </summary>
        public GameController gameController;

        public bool isMainMenuScene;

        private void Awake() {
            mainMenu.SetActive(false);
        }
        void OnEnable()
        {
            _ToggleMainMenu(isMainMenuScene);
        }

        /// <summary>
        /// Turn the main menu on or off.
        /// </summary>
        /// <param name="show"></param>
        public void ToggleMainMenu(bool show)
        {
            if (this.isMainMenuScene != show)
            {
                _ToggleMainMenu(show);
            }
        }

        void _ToggleMainMenu(bool show)
        {
            if (show)
            {
                if(!isMainMenuScene){
                    Time.timeScale = 0;
                }
                mainMenu.gameObject.SetActive(true);
                foreach (var i in gamePlayCanvasii) i.gameObject.SetActive(false);
            }
            else
            {
                if(!isMainMenuScene){
                    Time.timeScale = 1;
                }
                mainMenu.gameObject.SetActive(false);
                foreach (var i in gamePlayCanvasii) i.gameObject.SetActive(true);
            }
            this.isMainMenuScene = show;
        }

        void Update()
        {
            if (Input.GetButtonDown("Menu") && !isMainMenuScene)
            {
                Debug.Log("Menu button pressed"+isMainMenuScene);
                ToggleMainMenu(show: !isMainMenuScene);
            }
        }

    }
}
