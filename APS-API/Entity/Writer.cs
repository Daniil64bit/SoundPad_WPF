namespace WebAPITEst.Entity
{
    public class Writer
    {
        public int Id { get; set; }
        public string FullName { get; set; } =string.Empty;
        public DateOnly DateOfBirth { get; set; } = DateOnly.MinValue;
    }
}
