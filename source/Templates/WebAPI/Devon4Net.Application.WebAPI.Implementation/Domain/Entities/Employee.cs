namespace Devon4Net.Application.WebAPI.Implementation.Domain.Entities
{
    /// <summary>
    /// Entity class for Employee
    /// </summary>
    public partial class Employee
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Surname
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// mail
        /// </summary>
        public string Mail { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(Employee)) return false;
            Employee expected = obj as Employee;
            return expected.Name == Name
                && expected.Surname == Surname 
                && expected.Mail == Mail;
        }
    }
}
