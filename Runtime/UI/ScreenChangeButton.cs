using UnityEngine;
using UnityEngine.UI;
using Debug = Padoru.Diagnostics.Debug;

namespace Padoru.Core
{
    [RequireComponent(typeof(Button))]
    public class ScreenChangeButton : MonoBehaviour
    {
        [SerializeField] private InstantiatorScreenProvider screenProvider;

        private Button button;
        private IScreenManager screenManager;

        private void Awake()
        {
            screenManager = Locator.Get<IScreenManager>();

            button = GetComponent<Button>();
            button.onClick.AddListener(OnButtonClick);
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(OnButtonClick);
        }

        public void OnButtonClick()
        {
            if(screenProvider == null)
            {
                Debug.LogError($"Cannot transition to a null screen", gameObject);
                return;
            }

            screenManager.CloseAndShowScreen(screenProvider);
        }
    }
}