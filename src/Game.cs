class Game
{
    private Player player;
    private Parser parser;

    public Game()
    {
        player = new Player();
        parser = new Parser();
        CreateRooms();
    }

    private void CreateRooms()
    {
        player.TakeDamage(10);
        #region Rooms

        Room outside = new("infront of the haunted house");
        Room Hallway = new("in the hallway");
        Room Parlor = new("in the parlor");
        Room DinningRoom = new("in the dinning room");
        Room Kitchen = new("in the kitchen");
        Room backyard = new("in the backyard");

        #endregion Rooms

        #region exits

        outside.AddExit("north", Hallway);

        Hallway.AddExit("east", Parlor);
        Parlor.AddExit("west", Hallway);

        Hallway.AddExit("north", DinningRoom);
        DinningRoom.AddExit("south", Hallway);

        DinningRoom.AddExit("east", Kitchen);
        Kitchen.AddExit("west", DinningRoom);

        // locked door
        Kitchen.AddLockedDoor("north", backyard, "red-key");

        #endregion exits

        #region items

        Hallway.AddItem(new Item("sword", "A weak sword", 10, 10));
        Parlor.AddItem(new Item("health-potion", "A potion that heals 10 health", 1, 10));
        Parlor.AddItem(new Item("red-key", "A red key", 1, 0));

        #endregion items

        #region enemies

        outside.AddEnemy(new Enemy("evil-spirit", 30, 10));
        Parlor.AddEnemy(new Enemy("evil-spirit", 30, 10));

        #endregion enemies

        #region entry message

        outside.SetEntryMessage("You see a evil spirit. You don't have anything to defend yourself.");
        outside.SetEntryMessage("The spirit attacked you. you are bleeding.");
        Hallway.SetEntryMessage("The door to outside is locked");

        #endregion entry message

        player.CurrentRoom = outside; // Set the player's initial room
    }

    public void Play()
    {
        PrintWelcome();

        bool finished = false;
        while (!finished)
        {
            Command command = parser.GetCommand();
            finished = ProcessCommand(command);
        }
        Console.WriteLine("Thank you for playing.");
        Console.WriteLine("Press [Enter] to continue.");
        Console.ReadLine();
    }

    private void PrintWelcome()
    {
        Console.WriteLine();
        Console.WriteLine("Welcome to Zuul!");
        Console.WriteLine("Zuul is a new, incredibly boring adventure game.");
        Console.WriteLine("Type 'help' if you need help.");
        Console.WriteLine();
        Console.WriteLine(player.CurrentRoom.GetLongDescription());
    }

    private bool ProcessCommand(Command command)
    {
        bool wantToQuit = false;

        if (command.IsUnknown())
        {
            Console.WriteLine("I don't know what you mean...");
            return wantToQuit;
        }

        switch (command.CommandWord)
        {
            case "help":
                PrintHelp();
                break;
            case "go":
                GoRoom(command);
                player.TakeDamage(5);
                break;
            case "look":
                Console.WriteLine(player.CurrentRoom.GetLongDescription());
                break;
            case "status":
                player.Status();
                break;
            case "drop":
                player.DropItem(command);
                break;
            case "use":
                Console.WriteLine(player.UseItem(command.SecondWord, command.ThirdWord));
                foreach (KeyValuePair<string, Enemy> enemy in player.CurrentRoom.GetEnemies())
                {
                    Console.WriteLine("The " + enemy.Value.GetName() + " attacked you");
                    Console.WriteLine("You took " + enemy.Value.attack(player) + " damage");
                }
                break;
            case "inspect":
                Console.WriteLine(player.CurrentRoom.Inspect(command.SecondWord));
                break;
            case "take":
                if (player.TakeFromChest(command.SecondWord))
                {
                    Console.WriteLine("You took the " + command.SecondWord + " from the chest.");
                }
                else
                {
                    Console.WriteLine("You couldn't find " + command.SecondWord + " in the chest");
                }
                break;
            case "inventory":
                Console.WriteLine(player.Inventory());
                break;
            case "quit":
                wantToQuit = true;
                break;
        }

        return wantToQuit;
    }

    private void PrintHelp()
    {
        Console.WriteLine("You are lost. You are alone.");
        Console.WriteLine("You wander around at the university.");
        Console.WriteLine();
        parser.PrintValidCommands();
    }

    private void GoRoom(Command command)
    {
        if (!command.HasSecondWord())
        {
            Console.WriteLine("Go where?");
            return;
        }

        string direction = command.SecondWord;
        Room nextRoom = player.CurrentRoom.GetExit(direction);
        if (nextRoom == null)
        {
            Console.WriteLine("There is no door to " + direction + "!");
            return;
        }

        player.CurrentRoom = nextRoom;
        Console.WriteLine(player.CurrentRoom.GetLongDescription());
    }
}

