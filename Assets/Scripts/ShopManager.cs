using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopManager : MonoBehaviour
{
    public int[,] shopItems = new int[8, 8];
    public float coins;
    public Text coinsTXT;
    public GameObject panel;
    public GameObject panel1;
    public GameObject panel2;
    public Dropdown dropdown;
    private bool isIncreasingCoins = false;
    public GameObject UpgradeButton;
    public GameObject textBox;
    private bool isVisible = false;

    public GameObject Health;
    public GameObject Power;
    public GameObject Upgrade1;
    public GameObject Upgrade2;

    public GameObject item5Shop;
    public GameObject item6Shop;

    public GameObject item5Backpack;
    public GameObject item6Backpack;

    public GameObject item5Chest;
    public GameObject item6Chest;

    public GameObject sellItem5;
    public GameObject sellItem6;

    public int initialQuantityLimit = 4; // Initial quantity limit
    public int upgradedQuantityLimit = 5;


    void Start()


    {
        coinsTXT.text = "Money " + coins.ToString();


        shopItems[1, 1] = 1;//IDS
        shopItems[1, 2] = 2;
        shopItems[1, 3] = 3;
        shopItems[1, 4] = 4;
        shopItems[1, 5] = 5;
        shopItems[1, 6] = 6;

        shopItems[2, 1] = 30;//Price
        shopItems[2, 2] = 25;
        shopItems[2, 3] = 15;
        shopItems[2, 4] = 50;
        shopItems[2, 5] = 100;
        shopItems[2, 6] = 500;

        //Quanitity
        shopItems[3, 1] = 0;
        shopItems[3, 2] = 0;
        shopItems[3, 3] = 0;
        shopItems[3, 4] = 0;
        shopItems[3, 5] = 0;
        shopItems[3, 6] = 0;
        StartCoroutine(IncreaseCoinsOverTime());
        StartCoroutine(EnableNewItems());
    }

 


    public void ToggleTextBox()
    {
        isVisible = !isVisible;
        textBox.gameObject.SetActive(isVisible);
    }



    IEnumerator IncreaseCoinsOverTime()
    {
        isIncreasingCoins = true;

        while (true)
        {
            yield return new WaitForSeconds(10f); // Wait 10 seconds
            coins += 25; // Increase coins by 25
            coinsTXT.text = "Coins: " + coins.ToString();

            if (coins > 150)
            {
                // Activate the upgrade button when the player has 150 coin
                UpgradeButton.SetActive(true);
            }
        }
    }

    IEnumerator EnableNewItems()
    {
        // Initially, disable game objects representing items 5 and 6
        item5Shop.SetActive(false);
        item6Shop.SetActive(false);
        item5Backpack.SetActive(false);
        item6Backpack.SetActive(false);
        item5Chest.SetActive(false);
        item6Chest.SetActive(false);
        sellItem5.SetActive(false);
        sellItem6.SetActive(false);

        // Wait for 30 seconds
        yield return new WaitForSeconds(30f);

        // Enable game objects representing items 5 and 6
        item5Shop.SetActive(true);
        item6Shop.SetActive(true);
        item5Backpack.SetActive(true);
        item6Backpack.SetActive(true);
        item5Chest.SetActive(true);
        item6Chest.SetActive(true);
        sellItem5.SetActive(true);
        sellItem5.SetActive(true);

        Debug.Log("New items (ITEM ID 5 and 6) enabled after 30 seconds.");
    }



    public void Buy()
    {
        GameObject buttonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;
        ButtonInfo buttonInfo = buttonRef.GetComponent<ButtonInfo>();

        int itemID = buttonInfo.ItemID;
        int currentQuantity = shopItems[3, itemID];

        if (coins >= shopItems[2, itemID] && currentQuantity < initialQuantityLimit) // Check against initialQuantityLimit
        {
            coins -= shopItems[2, itemID];
            shopItems[3, itemID]++;
            coinsTXT.text = "Coins: " + coins.ToString();
            buttonInfo.QuantityTXT.text = shopItems[3, itemID].ToString();
        }
        else if (currentQuantity >= initialQuantityLimit)
        {
            Debug.Log("Maximum quantity reached. Cannot add more items.");
        }
        else
        {
            Debug.Log("Insufficient coins to buy the item.");
        }
    }

    public void Sell()
    {
        GameObject buttonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;
        ButtonInfo buttonInfo = buttonRef.GetComponent<ButtonInfo>();

        int itemID = buttonInfo.ItemID;
        int currentQuantity = shopItems[3, itemID];

        if (currentQuantity > 0)
        {
            coins += shopItems[2, itemID]; // Increase coins
            shopItems[3, itemID]--; // Decrease quantity
            coinsTXT.text = "Coins: " + coins.ToString();
            buttonInfo.QuantityTXT.text = shopItems[3, itemID].ToString();

            Debug.Log("Item sold successfully!");
        }
        else
        {
            Debug.Log("No items to sell.");
        }
    }
    public void Chest()
    {
        GameObject buttonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;
        ButtonInfo buttonInfo = buttonRef.GetComponent<ButtonInfo>();

        int itemID = buttonInfo.ItemID;

        if (shopItems[3, itemID] > 0)
        {
            // Decrease quantity in the backpack
            shopItems[3, itemID]--;

            // Increase quantity in the chest
            shopItems[4, itemID]++;

            // Update quantity in QuantityTXT
            buttonInfo.QuantityTXT.text = shopItems[3, itemID].ToString();

            // Update quantity in QuantityChestTXT
            buttonInfo.QuantityChestTXT.text = shopItems[4, itemID].ToString();

            Debug.Log("Item moved to chest successfully!");
        }
        else
        {
            Debug.Log("No items to move to chest.");
        }
    }
    public void ChestToBackpack()
    {
        GameObject buttonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;
        ButtonInfo buttonInfo = buttonRef.GetComponent<ButtonInfo>();

        int itemID = buttonInfo.ItemID;

        if (shopItems[4, itemID] > 0)
        {
            // Decrease quantity in the chest
            shopItems[4, itemID]--;

            // Increase quantity in the backpack
            shopItems[3, itemID]++;

            // Update quantity in quantityChestTXT
            buttonInfo.QuantityChestTXT.text = "Quantity " + shopItems[4, itemID].ToString();

            // Update quantity in quantityTXT
            buttonInfo.QuantityTXT.text = "Quantity " + shopItems[3, itemID].ToString();

            Debug.Log("Item moved from chest to backpack successfully!");
        }
        else
        {
            Debug.Log("No items to move from chest to backpack.");
        }
    }

    public void FilterHealth()
    {
        Health.SetActive(true);

        if (Power.activeInHierarchy==false)
        {
            Power.SetActive(true);
            Upgrade1.SetActive(true);
            Upgrade2.SetActive(true);
        }
        else
        {
            Power.SetActive(false);
            Upgrade1.SetActive(false);
            Upgrade2.SetActive(false);
        }

    }

    public void FilterUpgrade()
    {
        Upgrade1.SetActive(true);
        Upgrade2.SetActive(true);

        if (Power.activeInHierarchy == false)
        {
            Power.SetActive(true);
            Health.SetActive(true);
        }
        else
        {
            Power.SetActive(false);
            Health.SetActive(false);
        }

    }
    public void FilterPower()
    {
        Power.SetActive(true);

        if (Health.activeInHierarchy == false)
        {
            Health.SetActive(true);
            Upgrade1.SetActive(true);
            Upgrade2.SetActive(true);
        }
        else
        {
            Health.SetActive(false);
            Upgrade1.SetActive(false);
            Upgrade2.SetActive(false);
        }

    }







    public void ShopAct()
    {
        if (panel.activeInHierarchy == true)
        {
            panel.SetActive(false);
            panel1.SetActive(false);
            panel2.SetActive(false);
        }
        else
        {
            panel.SetActive(true);
            panel1.SetActive(true);
            panel2.SetActive(false);
        }


    }

    public void BackpackAct()
    {
        if (panel1.activeInHierarchy == true)
        {
            panel1.SetActive(false);
            panel2.SetActive(false);
            panel.SetActive(false);
        }
        else
        {
            panel1.SetActive(true);
            panel2.SetActive(true);
            panel.SetActive(false);
        }


    }



    public void UpgradeQuantityLimit()
    {
        if (coins >= 150)
        {
            coins -= 150;
            coinsTXT.text = "Coins: " + coins.ToString();

            // Increase the initial quantity limit by 1
            initialQuantityLimit++;

            // Update the quantity limit for all items
            for (int i = 0; i < shopItems.GetLength(1); i++)
            {
                shopItems[3, i] = Mathf.Min(upgradedQuantityLimit, shopItems[3, i] + 1); // Use upgradedQuantityLimit
                GameObject[] buttons = GameObject.FindGameObjectsWithTag("ShopItemButton");
                foreach (GameObject button in buttons)
                {
                    ButtonInfo buttonInfo = button.GetComponent<ButtonInfo>();
                    if (buttonInfo.ItemID == i)
                    {
                        buttonInfo.QuantityTXT.text = shopItems[3, i].ToString();
                        break;
                    }
                }
            }

            Debug.Log("Quantity limit upgraded to " + upgradedQuantityLimit + " for all items.");
        }
        else
        {
            Debug.Log("Insufficient coins to perform the upgrade.");
        }
    }









    public void CheckQuantityLimit()
    {
        GameObject buttonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;
        ButtonInfo buttonInfo = buttonRef.GetComponent<ButtonInfo>();

        int itemID = buttonInfo.ItemID;
        int maxQuantity = shopItems[3, itemID]; // Assuming the maximum quantity is stored in the same array as the current quantity

        if (maxQuantity >= 5)
        {
            Debug.Log("Maximum quantity reached for item with ID: " + itemID + ". Cannot add more items.");
        }
    }

}

