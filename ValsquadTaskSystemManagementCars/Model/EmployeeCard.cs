namespace ValsquadTaskSystemManagementCars.Model
{
    public class EmployeeCard
    {
        public EmployeeCard()
        {
            CountAccess = 1;
            Credit = 10;
        }
        public void setdebit(decimal value)
        {
            Debit = value;
            CountAccess++;
        }
        public void setCountAccessSameMinute(decimal value)
        {
            Debit = value;
        }
        public int Id { get; set; }
        public virtual Employee Employees { get; set; }
        public virtual Car Cars { get; set; }
        public int CountAccessSameMinute { get; private set; }
        public int CountAccess { get; private set; }
        public decimal Credit { get; private set; }
        public decimal Debit { get; private set; }
        public decimal RemainingBalance { get { return Credit - Debit; } }
        public DateTime DateTimeNow { get; set; } = DateTime.Now;
    }
}
