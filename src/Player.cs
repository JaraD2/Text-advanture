using System;
using System.Threading.Channels;

class Player
{
    private int health;
    private Room currentRoom; // Changed type from string to Room
    private Inventory backpack;

    public Room CurrentRoom
    {
        get { return currentRoom; }
        set { currentRoom = value; }
    }

    public Player()
    {
        health = 100;
        currentRoom = null; // Initialize to null until assigned a value
        backpack = new Inventory(25);
    }



    #region inventory
    public string UseItem(string itemName, string target)
    {
        // TODO implement:
        // Remove the Item from the backpack.
        // Use the Item.
        // Communicate to the user what's happening.
        // Return a message about the result of using the item.

        if (itemName == null)
        {
            return "Use what?";
        }

        Item item = backpack.Get(itemName);
        if (item == null)
        {
            return "You don't have that item.";
        }

        if (itemName == "sword")
        {
            if (currentRoom.enemies.Count == 0)
            {
                return "There are no enemies to attack.";
            }
            if (currentRoom.enemies.Count == 1)
            {
                return Attack(item, currentRoom.enemies.First().Key);
            }

            return Attack(item, target);
        }
        if (itemName == "health-potion")
        {

            if (health == 100)
            {
                return "You are already at full health.";
            }
            Heal(25);
            backpack.Remove(itemName);
            return "You drank the health potion and gained 25 health.";
        }
        if (itemName == "red-key")
        {
            Console.WriteLine(currentRoom.GetLockedExit());
            if (currentRoom.GetLockedExit() == null)
            {
                return "There is no locked door in this room.";
            }
            currentRoom.AddExit(currentRoom.GetLockedExit(), currentRoom.GetLockedExitNext());
            currentRoom.UnlockExit(currentRoom.GetLockedExit());
            return "You used the red key to unlock the door.";
        }
        return "Use what?";
    }

    public bool TakeFromChest(string itemName)
    {
        // Remove the Item from the Room
        // Put it in your backpack.
        // Inspect returned values.
        // If the item doesn't fit your backpack, put it back in the chest.
        // Communicate to the user what's happening.
        // Return true/false for success/failure

        Item item = currentRoom.chest.Get(itemName);

        if (item != null)
        {
            if (backpack.PutItemInInv(item))
            {
                currentRoom.chest.Remove(itemName);
                return true;
            }
            else
            {
                currentRoom.chest.PutItemInInv(item);
                return false;
            }
        }
        return false;
    }

    public void DropItem(Command command)
    {

        Item item = backpack.Get(command.SecondWord);
        if (item != null)
        {
            currentRoom.chest.PutItemInInv(item);
            Console.WriteLine("You dropped the " + command.SecondWord);
        }
        else
        {
            Console.WriteLine("You forgot what you wanted to drop");
        }
    }


    public string Inventory()
    {

        string str = "You are carrying:";

        if (backpack.IsEmpty())
        {
            return "You are not carrying anything.";
        }

        foreach (KeyValuePair<string, Item> item in backpack.GetItems())
        {
            str += " " + item.Key;
        }
        return str;
    }
    #endregion inventory

    #region health
    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    public void Heal(int heal)
    {
        if (health + heal > 100)
        {
            health = 100;
            return;
        }
        health += heal;
    }

    public int GetHealth()
    {
        return health;
    }
    #endregion health



    public string Attack(Item weapon, string target)
    {
        // If the enemy is in the room, attack it.
        // Communicate to the user what's happening.
        // Return a message about the result of the attack.


        if (target == null)
        {
            return "Attack what?";
        }

        if (!currentRoom.enemies.ContainsKey(target))
        {
            return "There is no " + target + " to attack.";
        }

        Enemy enemy = currentRoom.GetEnemy(target);

        enemy.TakeDamage(weapon.strength);
        if (enemy.GetHealth() <= 0)
        {
            currentRoom.enemies.Remove(target);
            return "You killed the " + enemy.GetName() + " with a " + weapon.GetName() + ".";
        }
        return "You attacked the " + enemy.GetName() + " with a " + weapon.GetName() + " for " + weapon.strength + " damage.";

    }

    public void Status()
    {
        Console.WriteLine("Your health is " + health);
        Console.WriteLine("Inventory: " + Inventory());
        Console.WriteLine("Weight: " + backpack.UsedSpace() + ":" + backpack.MaxWeight());
    }
}
