type Props = {
  doctor: string;
  department: string;
  date: string;
  time: string;
};

export function SlotCard({ doctor, department, date, time }: Props) {
  return (
    <div className="bg-white rounded-xl shadow p-4 flex items-center justify-between">
      <div>
        <p className="text-sm text-gray-600">{department}</p>
        <h3 className="font-medium text-gray-900">{doctor}</h3>
        <p className="text-sm text-gray-500">
          {date} • {time}
        </p>
      </div>

      <button className="px-4 py-2 rounded-lg bg-primary text-white text-sm">
        Randevu Al
      </button>
    </div>
  );
}
