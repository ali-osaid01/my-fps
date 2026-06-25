using Unity.FPS.Game;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Unity.FPS.UI
{
    public class LoadSceneButton : MonoBehaviour
    {
        public string SceneName = "";
        public bool SetDifficultyBeforeLoading;
        public GameDifficulty Difficulty = GameDifficulty.Medium;
        public bool BuildDifficultyButtons;

        private InputAction m_SubmitAction;
        
        void Start()
        {
            if (BuildDifficultyButtons)
                CreateDifficultyButtons();

            m_SubmitAction = InputSystem.actions.FindAction("UI/Submit");
            m_SubmitAction.Enable();
        }
        
        void Update()
        {
            if (EventSystem.current.currentSelectedGameObject == gameObject
                && m_SubmitAction.WasPressedThisFrame())
            {
                LoadTargetScene();
            }
        }

        public void LoadTargetScene()
        {
            if (SetDifficultyBeforeLoading)
                DifficultySettings.SetDifficulty(Difficulty);

            SceneManager.LoadScene(SceneName);
        }

        void CreateDifficultyButtons()
        {
            Button templateButton = GetComponent<Button>();
            if (!templateButton)
                return;

            GameObject easy = CreateDifficultyButton(templateButton, "EasyButton", "Easy", GameDifficulty.Easy, 90f);
            GameObject medium = CreateDifficultyButton(templateButton, "MediumButton", "Medium", GameDifficulty.Medium, 10f);
            GameObject hard = CreateDifficultyButton(templateButton, "HardButton", "Hard", GameDifficulty.Hard, -70f);

            LinkNavigation(easy.GetComponent<Button>(), medium.GetComponent<Button>());
            LinkNavigation(medium.GetComponent<Button>(), hard.GetComponent<Button>());
            LinkNavigation(hard.GetComponent<Button>(), easy.GetComponent<Button>());

            MenuNavigation menuNavigation = FindAnyObjectByType<MenuNavigation>();
            if (menuNavigation)
                menuNavigation.DefaultSelection = easy.GetComponent<Selectable>();

            gameObject.SetActive(false);
        }

        GameObject CreateDifficultyButton(Button templateButton, string objectName, string label, GameDifficulty difficulty, float y)
        {
            GameObject buttonObject = Instantiate(templateButton.gameObject, transform.parent);
            buttonObject.name = objectName;

            RectTransform rectTransform = buttonObject.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(0f, y);
            rectTransform.sizeDelta = new Vector2(380f, 64f);

            TMP_Text text = buttonObject.GetComponentInChildren<TMP_Text>(true);
            if (text)
            {
                text.text = label;
                text.fontSize = 34f;
                text.fontStyle = FontStyles.Bold;
                text.color = Color.white;
            }

            LoadSceneButton loadSceneButton = buttonObject.GetComponent<LoadSceneButton>();
            loadSceneButton.SceneName = SceneName;
            loadSceneButton.SetDifficultyBeforeLoading = true;
            loadSceneButton.Difficulty = difficulty;
            loadSceneButton.BuildDifficultyButtons = false;

            Button button = buttonObject.GetComponent<Button>();
            StyleDifficultyButton(button, difficulty);
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(loadSceneButton.LoadTargetScene);

            return buttonObject;
        }

        static void StyleDifficultyButton(Button button, GameDifficulty difficulty)
        {
            Image image = button.GetComponent<Image>();
            if (image)
            {
                image.color = new Color(0.015f, 0.02f, 0.04f, 0.92f);
            }

            Color accent = GetDifficultyAccent(difficulty);
            ColorBlock colors = button.colors;
            colors.normalColor = new Color(0.015f, 0.02f, 0.04f, 0.92f);
            colors.highlightedColor = accent;
            colors.selectedColor = accent;
            colors.pressedColor = Color.Lerp(accent, Color.white, 0.25f);
            colors.disabledColor = new Color(0.1f, 0.1f, 0.1f, 0.4f);
            colors.colorMultiplier = 1f;
            colors.fadeDuration = 0.08f;
            button.colors = colors;

            Outline outline = button.GetComponent<Outline>();
            if (!outline)
                outline = button.gameObject.AddComponent<Outline>();

            outline.effectColor = accent;
            outline.effectDistance = new Vector2(2f, -2f);

            Shadow shadow = button.GetComponent<Shadow>();
            if (!shadow)
                shadow = button.gameObject.AddComponent<Shadow>();

            shadow.effectColor = new Color(0f, 0f, 0f, 0.75f);
            shadow.effectDistance = new Vector2(4f, -4f);
        }

        static Color GetDifficultyAccent(GameDifficulty difficulty)
        {
            switch (difficulty)
            {
                case GameDifficulty.Easy:
                    return new Color(0.05f, 0.85f, 1f, 1f);
                case GameDifficulty.Hard:
                    return new Color(1f, 0.12f, 0.45f, 1f);
                default:
                    return new Color(0.8f, 0.18f, 1f, 1f);
            }
        }

        static void LinkNavigation(Button current, Button next)
        {
            Navigation navigation = current.navigation;
            navigation.mode = Navigation.Mode.Explicit;
            navigation.selectOnDown = next;
            navigation.selectOnUp = next;
            current.navigation = navigation;
        }
    }
}
