import { SlotCard } from "@/components/card/AppointmentSlotCard";


export default function AppointmentSDetail() {
  return (
    <div className="min-h-[calc(100vh-80px)] bg-gray-50 px-4 py-8">
      <div className="max-w-4xl mx-auto">
        <h1 className="text-xl font-semibold text-gray-900 mb-6">
          Uygun Randevular
        </h1>

        <div className="space-y-4">
          <SlotCard
            doctor="Dr. Ahmet Yılmaz"
            department="Kardiyoloji"
            date="12 Mart 2025"
            time="09:30"
          />

          <SlotCard
            doctor="Dr. Ahmet Yılmaz"
            department="Kardiyoloji"
            date="12 Mart 2025"
            time="11:00"
          />

          <SlotCard
            doctor="Dr. Ahmet Yılmaz"
            department="Kardiyoloji"
            date="12 Mart 2025"
            time="14:30"
          />
        </div>
      </div>
    </div>
  );
}
