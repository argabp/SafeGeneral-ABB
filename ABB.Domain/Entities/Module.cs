namespace ABB.Domain.Entities
{
    public class Module
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Icon { get; set; }

        public int Sequence { get; set; }

        public bool IsDeleted { get; set; }
    }
}