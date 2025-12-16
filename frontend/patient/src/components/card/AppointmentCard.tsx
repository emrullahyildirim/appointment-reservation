type Props = {
  title: string;
  doctor: string;
  department: string;
  date: string;
  status?: "active" | "past";
};

export function AppointmentCard({
  title,
  doctor,
  department,
  date,
  status = "active",
}: Props) {
  return (
    <div className="bg-white rounded-xl shadow p-4 flex justify-between items-center">
      <div>
        <h3 className="font-medium text-gray-900">{title}</h3>
        <p className="text-sm text-gray-600">
          {department} • {doctor}
        </p>
        <p className="text-sm text-gray-500">{date}</p>
      </div>

      {status === "active" ? (
        <span className="text-sm text-info font-medium">Aktif</span>
      ) : (
        <span className="text-sm text-gray-400">Tamamlandı</span>
      )}
    </div>
  );
}
