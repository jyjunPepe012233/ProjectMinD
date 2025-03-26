using MinD.Runtime.Entity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using MinD.SO.Item;

namespace MinD.Runtime.UI
{
    public class InteractionPanelController : MonoBehaviour
    {
        [SerializeField] private GameObject interactionPanel;      // 기존 상호작용 패널
        [SerializeField] private GameObject itemRootingPanel;     // 루팅 패널
        [SerializeField] private CanvasGroup rootingCanvasGroup;  // 루팅 패널의 CanvasGroup (투명도 조정용)
        [SerializeField] private TextMeshProUGUI itemNameText;    // 루팅 패널의 아이템 이름
        [SerializeField] private Image itemImage;                 // 루팅 패널의 아이템 이미지
        [SerializeField] private float fadeOutDuration = 1f;      // 페이드 아웃 지속 시간

        private bool isFadingOut = false;       // 페이드 아웃 상태 확인용
        private Player player;


        public void Awake()
        {
            player = FindObjectOfType<Player>();
        }
        public void RefreshInteractionPanel()
        {
            if (player == null || player.interaction == null) return;

            bool hasInteractables = player.interaction.currentInteractables.Count > 0;
            interactionPanel.SetActive(hasInteractables);

            if (hasInteractables)
            {
                interactionPanel.GetComponentInChildren<TextMeshProUGUI>().text =
                    player.interaction.currentInteractables[0].interactionText;
            }
            // if (Player.player.interaction.currentInteractables.Count == 0)
            // {
            //     UnDisplayItemInteractionPanel();
            // }
            // else
            // {
            //     DisplayItemInteractionPanel();
            //     interactionPanel.GetComponentInChildren<TextMeshProUGUI>().text = Player.player.interaction.currentInteractables[0].interactionText;
            // }
        }
        public void ShowLootingPanel(Item item)
        {
            if (item == null) return;
            
            itemNameText.text = item.itemName;
            itemImage.sprite = item.itemImage;
            
            itemRootingPanel.SetActive(true);
            rootingCanvasGroup.alpha = 1f;
            isFadingOut = false;
            
            StartCoroutine(FadeOutLootingPanel());
        }

        private IEnumerator FadeOutLootingPanel()
        {
            if (isFadingOut) yield break;
            
            isFadingOut = true;
            yield return new WaitForSeconds(1.5f);
            
            float elapsedTime = 0f;
            while (elapsedTime < fadeOutDuration)
            {
                elapsedTime += Time.deltaTime;
                rootingCanvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeOutDuration);
                yield return null;
            }

            rootingCanvasGroup.alpha = 0f;
            itemRootingPanel.SetActive(false);
            isFadingOut = false;
        }
    }
}
