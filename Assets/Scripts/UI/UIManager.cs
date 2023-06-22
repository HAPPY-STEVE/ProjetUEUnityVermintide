using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using UI.Tween;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    /// <summary>
    /// Gere les differents menus en jeu. Change les inputs selon l'UI 
    /// Score et pause actuellement. 
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        private StarterAssetsInputs starter;
        public List<TweenContainer> scoreMenu;
        public List<TweenContainer> pauseMenu;
        private bool scoreMenuActive = false;
        public bool pauseMenuActive = false;
        private InputManager im;

        public void Start()
        {
            im = FindObjectOfType<InputManager>();
            starter = FindObjectOfType<StarterAssetsInputs>();

        }

        public void Update()
        {
            if (starter.scoreMenu == true)
            {
                if (scoreMenuActive == false && pauseMenuActive == false)
                {
                    revealScoreMenu();
                    scoreMenuActive = true;

                }
            }
            else
            {
                if (scoreMenuActive == true)
                {
                    hideScoreMenu();
                    scoreMenuActive = false;

                }
            }
            if (starter.pauseMenu == true)
            {
                if (pauseMenuActive == false)
                {
                    im.setFirstPersonController(false);
                    starter.SetCursorState(false);
                    im.ToggleActionMap("UI");
                    revealPauseMenu();
                    pauseMenuActive = true;

                }
            }
            else
            {
                if (pauseMenuActive == true)
                {
                    starter.SetCursorState(true);

                    im.ToggleActionMap("Player");
                    im.setFirstPersonController(true);
                    hidePauseMenu();
                    pauseMenuActive = false;

                }
            }

        }

        public void setPause(bool t)
        {
            starter.pauseMenu = false;
        }
        public void hidePauseMenu()
        {
            Debug.Log("hide pause");
            foreach (TweenContainer t in pauseMenu)
            {
                t.OnClose();
            }

        }

        private void revealPauseMenu()
        {
            Debug.Log("reveal pause");
            foreach (TweenContainer t in pauseMenu)
            {
                t.OnActive();
            }

            foreach (TweenContainer t in scoreMenu)
            {
                t.OnClose();
            }
        }

        private void hideScoreMenu()
        {
            Debug.Log("hide score");
            foreach (TweenContainer t in scoreMenu)
            {
                t.OnClose();
            }
        }

        private void revealScoreMenu()
        {
            Debug.Log("reveal score");
            foreach (TweenContainer t in scoreMenu)
            {
                t.OnActive();
            }
        }
    }

}
