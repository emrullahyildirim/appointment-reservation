import Button from "@/components/common/Button";
import { useNavigate } from "react-router-dom";


export default function BookAppointmentCard() {
    const navigator = useNavigate();
  return (
    <div className="bg-white rounded-2xl shadow-md p-6 max-w-md">
      <h2 className="text-lg font-semibold text-gray-900 mb-4">
        Book Appointment
      </h2>

      <div className="space-y-4">
        <div>
          <label className="text-sm text-gray-700">Bölüm</label>
          <select className="mt-1 w-full rounded-lg border px-3 py-2">
            <option>Seçiniz</option>
            <option>Kardiyoloji</option>
            <option>Dahiliye</option>
            <option>Cilt Hastalıkları</option>
            <option>Kadın Hastalıkları</option>
            <option>Üroloji</option>
            <option>Ortapedi</option>
            <option>Nöroloji</option>
          </select>
        </div>

        <div>
          <label className="text-sm text-gray-700">Doktor</label>
          <select className="mt-1 w-full rounded-lg border px-3 py-2">
            <option>Seçiniz</option>
            <option>Dr. Alp Yılmaz</option>
            <option>Dr. Ayşe Demir</option>
            <option>Opr. Dr. Neslihan Tunç</option>
            <option>Uz. Dr. Kerem Altın</option>
          </select>
        </div>

        <div>
          <label className="text-sm text-gray-700">Tarih</label>
          <input
            type="date"
            className="mt-1 w-full rounded-lg border px-3 py-2"
          />
        </div>

        <Button text="Randevu Al" onClick={() => (navigator("/appointment-detail"))}/>

      </div>
    </div>
  );
}
