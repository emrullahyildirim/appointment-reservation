using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientService.Entities.Enums
{
    public enum AppointmentStatus
    {
        Pending,        // Hasta veya sistem tarafından oluşturuldu, henüz onaylanmadı
        Confirmed,      // Doktor veya sistem tarafından onaylandı
        Cancelled,      // Hasta veya doktor tarafından iptal edildi
        Completed,      // Randevu başarıyla gerçekleşti
        NoShow,         // Hasta randevuya gelmedi
        Rescheduled     // Randevu başka bir zamana taşındı
    }
}
