using System.Collections.Generic;
using TinyUnits;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class TinyUnitStore : MonoBehaviour
    {
        public VisualTreeAsset itemTemplate; // Reference to the UXML template for an item
        public VisualElement rootVisualElement;

        private void OnEnable()
        {
            // Find the root element (the ScrollView container in the UXML)
            rootVisualElement = GetComponent<UIDocument>().rootVisualElement;
            ScrollView itemList = rootVisualElement.Q<ScrollView>("itemList");

            // Example list of items to populate
            var items = new List<TinyUnit>(Resources.LoadAll<TinyUnit>("GameObjects/TinyUnits"));

            // Populate the ScrollView with items
            foreach (var item in items)
            {
                // Create a new instance of the item template
                VisualElement newItem = itemTemplate.CloneTree();
            
                // Set the item's icon
                newItem.Q<Image>("itemIcon").image = item.iconSprite.texture;

                // Set the item's name
                newItem.Q<Label>("itemName").text = item.unitName;

                // Set the item's description
                newItem.Q<Label>("itemDescription").text = item.description;

                // Set the item's price
                newItem.Q<Label>("itemPrice").text = $"Price: {item.unitBaseCost}";

                // Add button functionality
                Button purchaseButton = newItem.Q<Button>("purchaseButton");
                purchaseButton.clicked += () => PurchaseItem(item);

                // Add the new item to the ScrollView
                itemList.Add(newItem);
            }
        }

        private void PurchaseItem(TinyUnit item)
        {
            Debug.Log($"Purchased {item.unitName} for {item.unitBaseCost}!");
        }
    }
}