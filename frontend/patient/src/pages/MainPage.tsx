import { AppointmentCard } from "@/components/card/AppointmentCard";

export default function MainPage() {
  return (
    <div className="bg-gray-50 min-h-[calc(100vh-80px)] px-4 py-8">
      <div className="max-w-4xl mx-auto space-y-8">
        <section>
          <h2 className="text-lg font-semibold mb-4">
            Aktif Randevularım
          </h2>

          <div className="space-y-3">
            <AppointmentCard
              title="Kontrol Muayenesi"
              department="Kardiyoloji"
              doctor="Dr. Ahmet Yılmaz"
              date="12 Mart 2025 • 14:30"
            />
          </div>
        </section>

        <section>
          <h2 className="text-lg font-semibold mb-4">
            Geçmiş Randevularım
          </h2>

          <div className="space-y-3">
            <AppointmentCard
              title="Muayene"
              department="Dahiliye"
              doctor="Dr. Ayşe Demir"
              date="3 Şubat 2025 • 10:00"
              status="past"
            />
          </div>
        </section>
      </div>
    </div>
  );
}
