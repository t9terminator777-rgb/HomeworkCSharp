interface IAccountOperations
{
    void ShowInfo();
    void Deposit(decimal amount);
}

class BankAccount : IAccountOperations
{
    public string Owner { get; }
    public decimal Balance { get; private set; }

    public BankAccount(string owner, decimal startingBalance)
    {
        Owner = owner;
        Balance = startingBalance;
    }

    public void ShowInfo()
    {
        Console.WriteLine($"Владелец: {Owner}");
        Console.WriteLine($"Баланс: {Balance} сом");
    }

    public void Deposit(decimal amount)
    {
        if (amount <= 0)
        {
            Console.WriteLine("Сумма пополнения должна быть больше нуля.");
            return;
        }

        Balance += amount;
        Console.WriteLine($"Счёт пополнен на {amount} сом.");
    }
}

class Program
{
    static void Main()
    {
        BankAccount account = new BankAccount("Актан", 1000m);

        account.ShowInfo();
        account.Deposit(500m);
        account.ShowInfo();
    }
}
