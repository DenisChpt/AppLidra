namespace AppLidra.Shared.Models
{
    public class ExpenseModel
    {
        public string Name { get; set; } = "New Expense";
        public double Amount { get; set; }
        public DateTime Date { get; set; }
        public int ProjectId { get; set; }
        public List<ExpenseShare> Shares { get; set; } = [];

        public ExpenseModel(string name, double amount, DateTime date, int projectId, List<ExpenseShare> shares)
        {
            Name = name;
            Amount = amount;
            Date = date;
            ProjectId = projectId;
            double sharesCount = 0;
            for (int i = 0; i < shares.Count; i++)
            {
                sharesCount += shares[i].Share;
            }
            if (sharesCount != 1)
            {
                throw new ArgumentException("Shares do not add up to 1");
            }

            Shares = shares;
        }
    }
}
