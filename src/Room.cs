
class Room
{
    // Private fields
    private string description;
    private Dictionary<string, Room> exits; // stores exits of this room.
    private Dictionary<string, Tuple<Room, string>> lockedExit;
    public Inventory chest;
    public Dictionary<string, Enemy> enemies;
    public string entryMessage;


    // Create a room described "description". Initially, it has no exits.
    // "description" is something like "in a kitchen" or "in a court yard".
    // als add a new enemy to the room
    public Room(string description)
    {
        this.description = description;
        exits = new Dictionary<string, Room>();
        lockedExit = new Dictionary<string, Tuple< Room,string>>();
        chest = new Inventory(999999);
        // dictionary of enemies
        enemies = new Dictionary<string, Enemy>();
        entryMessage = "";
    }

    // Define an exit for this room.

    #region Add
    public void AddExit(string direction, Room neighbor)
    {
        exits.Add(direction, neighbor);
    }
    public void AddItem(Item item)
    {
        chest.PutItemInInv(item);
    }
    public void AddEnemy(Enemy enemy)
    {
        enemies.Add(enemy.GetName(), enemy);
    }
    public void AddLockedDoor(string direction, Room backyard, string key)
    {
        lockedExit.Add(direction, new Tuple<Room, string>(backyard, key));
    }

    public void SetEntryMessage(string v)
    {
        entryMessage = v;
    }

    #endregion Add

    #region disc
    // Return the description of the room.
    public string GetShortDescription()
    {
        return description;
    }

    // Return a long description of this room, in the form:
    //     You are in the kitchen.
    //     Exits: north, west
    public string GetLongDescription()
    {
        string str = "";
        str += "You are " + description + ".\n";
        str += entryMessage + "\n";
        str += GetExitString() + "\n";
        if (lockedExit != null)
        {
            foreach (KeyValuePair<string, Tuple<Room, string>> locked in lockedExit)
            {
 //               str += "The " + locked.Value.Item2 + " door is locked.\n";
                str += "There is a locked door to the " + locked.Key + ".\n";
                str += "You need a " + locked.Value.Item2 + " to open the door.\n";

            }
        }

        if (!chest.IsEmpty())
        {
            str += "\n";
            str += "There is a chest in the room.";
        }
        if (enemies.Count > 0)
        {
            str += "\n";
            foreach (KeyValuePair<string, Enemy> enemy in enemies)
            {
                str += "you see a " + enemy.Value.GetName() + " in the room.";
            }
        }
        return str;
    }
    #endregion disc

    // Return the room that is reached if we go from this room in direction
    // "direction". If there is no room in that direction, return null.
    public Room GetExit(string direction)
    {
        if (exits.ContainsKey(direction))
        {
            return exits[direction];
        }
        return null;
    }

    public string GetLockedExit() 
    {
        
        foreach (KeyValuePair<string, Tuple<Room, string>> locked in lockedExit)
        {
            return locked.Key;
        }
        return null;
    }
    public Room GetLockedExitNext()
    {
        foreach (KeyValuePair<string, Tuple<Room, string>> locked in lockedExit)
        {
            return locked.Value.Item1;
        }
        return null;
    }

    // Return a string describing the room's exits, for example
    // "Exits: north, west".
    private string GetExitString()
    {
        string str = "Exits:";

        // Build the string in a `foreach` loop.
        // We only need the keys.
        int countCommas = 0;
        foreach (string key in exits.Keys)
        {
            if (countCommas != 0)
            {
                str += ",";
            }
            str += " " + key;
            countCommas++;
        }

        return str;
    }


    public string Inspect(string secondWord)
    {
        string str = "";
        if (secondWord == "chest")
        {
            if (chest.IsEmpty())
            {
                str += "There is no chest.";
            }
            foreach (KeyValuePair<string, Item> item in chest.GetItems())
            {
                str += item.Value.GetName() + "\n";
            }
        }
        return str;
    }

    public Enemy GetEnemy(string enemyName)
    {
        if (enemies.ContainsKey(enemyName))
        {
            return enemies[enemyName];
        }
        return null;

    }
    public void RemoveEnemy(string enemyName)
    {
        enemies.Remove(enemyName);
    }
    public bool HasEnemies()
    {
        return enemies.Count > 0;
    }
    public Dictionary<string, Enemy> GetEnemies()
    {
        return enemies;
    }

    public void UnlockExit(string direction)
    {
        if (lockedExit.ContainsKey(direction))
        {
            lockedExit.Remove(direction);
            
        }
    }
}

