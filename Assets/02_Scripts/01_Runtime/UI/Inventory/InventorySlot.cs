using MinD.SO.Item;
using UnityEngine;
using UnityEngine.UI;
using NotImplementedException = System.NotImplementedException;

namespace MinD.Runtime.UI
{
    public class InventorySlot : MonoBehaviour
    {
        public Image itemImage;
        public Text itemCountText;
        public GameObject selectionImage;

        private Item currentItem;
        private InventoryUI inventoryUI;

        public int categoryId;

        void Start()
        {
            inventoryUI = FindObjectOfType<InventoryUI>();
        }

        public void SetItem(Item item, int itemCategoryId)
        {
            if (categoryId != itemCategoryId)
            {
                ClearSlot();
                return;
            }

            currentItem = item;

            if (item != null)
            {
                itemImage.sprite = item.itemImage;
                itemImage.enabled = true;

                if (item.itemCount >= 2)
                {
                    itemCountText.text = item.itemCount.ToString();
                    itemCountText.enabled = true;
                }
                else
                {
                    itemCountText.enabled = false;
                }
            }
            else
            {
                ClearSlot();
            }
        }

        public void ClearSlot()
        {
            currentItem = null;
            itemImage.enabled = false;
            itemCountText.enabled = false;
            SetSelected(false);
        }

        public void SetSelected(bool isSelected)
        {
            if (selectionImage != null)
            {
                selectionImage.SetActive(isSelected);
            }
        }

        public void OnClick()
        {
            if (inventoryUI != null)
            {
                inventoryUI.OnSlotClicked(this);

                var item = GetCurrentItem();
                if (item != null && item.itemCount > 0)
                {
                    int slotIndex = inventoryUI.GetSlotIndex(this);
                    inventoryUI.itemActionPanel.ShowPanel(item);
                }
            }
        }

        public Item GetCurrentItem()
        {
            return currentItem;
        }
    }

}
