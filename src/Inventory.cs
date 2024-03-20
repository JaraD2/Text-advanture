

class Inventory
{
    // fields
    private int maxWeight;
    private Dictionary<string, Item> items;

    // constructor
    public Inventory(int maxWeight)
    {
        this.maxWeight = maxWeight;
        this.items = new Dictionary<string, Item>();
    }

    public int UsedSpace()
    {
        int usedSpace = 0;
        foreach (KeyValuePair<string, Item> item in items)
        {
            usedSpace += item.Value.Weight;
        }
        return usedSpace;
    }
    public int FreeSpace()
    {
        return maxWeight - UsedSpace();
    }
    public int MaxWeight()
    {
        return maxWeight;
    }

    public bool PutItemInInv(Item item)
    {
        // Check the Weight of the Item and check for enough space in the Inventory
        // Does the Item fit?
        if (item.Weight <= FreeSpace())
        {
            // Put Item in the items Dictionary
            items[item.Name] = item;
            return true;
        }

        return false;
    }

    public Item Get(string itemName)
    {
        if (itemName == null)
        {
            return null;
        }
        if (items.ContainsKey(itemName))
        {
            Item foundItem = items[itemName];
            return foundItem;
        }

        return null;
    }

    public Dictionary<string, Item> GetItems()
    {
        return items;
    }

    public bool IsEmpty()
    {
        return items.Count == 0;
    }

    public void Remove(string itemName)
    {
        if (items.ContainsKey(itemName))
        {
            items.Remove(itemName);
        }
    }
}