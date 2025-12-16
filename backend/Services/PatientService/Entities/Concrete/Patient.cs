using Core.Entities;

namespace Entities.Concrete
{
    public class Patient : IEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public string IdentityNumber { get; set; }
        public DateTime CreatedAt { get; set; }


        public virtual ICollection<Appointment> Appointments { get; set; }= new List<Appointment>();
    }
}