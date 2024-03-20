class Item
{
    // fields
    public string Name { get; private set; }
    public string Description { get; private set; }
    public int Weight { get; private set; }
    public int strength { get; private set; }

    // constructor
    public Item(string name, string desc, int weight, int strength)
    {
        Name = name;
        Description = desc;
        Weight = weight;
        this.strength = strength;
    }

    public string GetName()
    {
        return Name;
    }

    public string GetDescription()
    {
        return Description;
    }

    public int GetWeight()
    {
        return Weight;
    }
}
