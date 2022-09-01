namespace Devon4Net.Application.Keycloak.Model
{
    public class Teacher : Person
    {
        public List<Student> Students { get; set; }
    }
}
