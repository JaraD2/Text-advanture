class Enemy
{
    private string name;
    private int health;
    private int damage;

    public Enemy(string name, int health, int damage)
    {
        this.name = name;
        this.health = health;
        this.damage = damage;
    }

    public string GetName()
    {
        return name;
    }
    public int attack(Player player)
    {
        int random = new Random().Next(damage / 2, damage);
        player.TakeDamage(random);
        return random;
    }

    public int GetHealth()
    {
        return health;
    }
    public int GetDamage()
    {
        return damage;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }

}
