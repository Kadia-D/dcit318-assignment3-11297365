using System;
using System.Collections.Generic;

namespace FinanceApp
{
    // a. Record type to represent financial data
    public record Transaction(int Id, DateTime Date, decimal Amount, string Category);

    // b. Interface for transaction processing
    public interface ITransactionProcessor
    {
        void Process(Transaction transaction);
    }

    // c. Implement processors
    public class BankTransferProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            Console.WriteLine($"[Bank Transfer] Processed {transaction.Amount:C} for {transaction.Category}");
        }
    }

    public class MobileMoneyProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            Console.WriteLine($"[Mobile Money] Processed {transaction.Amount:C} for {transaction.Category}");
        }
    }

    public class CryptoWalletProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            Console.WriteLine($"[Crypto Wallet] Processed {transaction.Amount:C} for {transaction.Category}");
        }
    }

    // d. Base class Account
    public class Account
    {
        public string AccountNumber { get; }
        public decimal Balance { get; protected set; }

        public Account(string accountNumber, decimal initialBalance)
        {
            AccountNumber = accountNumber;
            Balance = initialBalance;
        }

        public virtual void ApplyTransaction(Transaction transaction)
        {
            Balance -= transaction.Amount;
            Console.WriteLine($"Transaction applied. New balance: {Balance:C}");
        }
    }

    // e. Sealed SavingsAccount
    public sealed class SavingsAccount : Account
    {
        public SavingsAccount(string accountNumber, decimal initialBalance)
            : base(accountNumber, initialBalance) { }

        public override void ApplyTransaction(Transaction transaction)
        {
            if (transaction.Amount > Balance)
            {
                Console.WriteLine("Insufficient funds.");
            }
            else
            {
                Balance -= transaction.Amount;
                Console.WriteLine($"Transaction of {transaction.Amount:C} applied. Updated balance: {Balance:C}");
            }
        }
    }

    // f. FinanceApp 
    public class FinanceApp
    {
        private List<Transaction> _transactions = new();

        public void Run()
        {
            var savingsAccount = new SavingsAccount("ACC77438", 10000m);

            var transaction1 = new Transaction(1, DateTime.Now, 400m, "Groceries");
            var transaction2 = new Transaction(2, DateTime.Now, 7000m, "Rent");
            var transaction3 = new Transaction(3, DateTime.Now, 2700m, "Fees");

            ITransactionProcessor mobileMoney = new MobileMoneyProcessor();
            ITransactionProcessor bankTransfer = new BankTransferProcessor();
            ITransactionProcessor crypto = new CryptoWalletProcessor();

            mobileMoney.Process(transaction1);
            bankTransfer.Process(transaction2);
            crypto.Process(transaction3);

            savingsAccount.ApplyTransaction(transaction1);
            savingsAccount.ApplyTransaction(transaction2);
            savingsAccount.ApplyTransaction(transaction3);

            _transactions.Add(transaction1);
            _transactions.Add(transaction2);
            _transactions.Add(transaction3);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var app = new FinanceApp();
            app.Run();
        }
    }
}
