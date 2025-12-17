import { useState } from "react";

// ---------------- MOCK DATA ----------------
const dayData = [
  { time: "09:00", status: "busy", patient: "Ali Veli" },
  { time: "09:30", status: "free" },
  { time: "10:00", status: "busy", patient: "Ayşe Yılmaz" },
  { time: "10:30", status: "break" },
  { time: "11:00", status: "free" },
  { time: "11:30", status: "busy", patient: "Fatma Öztürk" },
  { time: "12:00", status: "free" },
  { time: "12:30", status: "free" },
  { time: "13:00", status: "break" },
  { time: "13:30", status: "busy", patient: "Mehmet Kaya" },
  { time: "14:00", status: "free" },
];

const weekData = {
  Pazartesi: ["09:00", "10:00", "11:30", "13:30"],
  Salı: ["11:00", "14:00"],
  Çarşamba: [],
  Perşembe: ["09:30", "14:00", "15:00"],
  Cuma: ["10:00", "11:00"],
};

type MonthAppointments = {
  [day: number]: string[];
};

const monthData: MonthAppointments = {
  3: ["09:00", "10:30", "14:00"],
  7: ["11:00", "12:00"],
  12: ["09:00", "13:00"],
  15: ["10:00"],
  21: ["15:00", "16:00"],
  28: ["11:30"],
};

// ---------------- COMPONENTS ----------------

const DaySlot = () => (
  <div className="space-y-3">
    {dayData.map((slot, index) => (
      <div
        key={index}
        className={`flex justify-between items-center p-4 rounded-lg shadow-sm transition-all
        ${
          slot.status === "busy"
            ? "text-primary border-l-4 border-primary"
            : slot.status === "free"
            ? "bg-green-50 border-l-4 border-green-500"
            : "bg-gray-100 border-l-4 border-gray-400"
        }`}
      >
        <span className="font-semibold text-gray-800">{slot.time}</span>
        <div className="flex items-center gap-3">
          <span
            className={`font-medium text-sm ${
              slot.status === "busy"
                ? "text-primary-700"
                : slot.status === "free"
                ? "text-green-700"
                : "text-gray-700"
            }`}
          >
            {slot.status === "busy"
              ? ` Dolu – ${slot.patient}`
              : slot.status === "free"
              ? " Müsait"
              : " Mola"}
          </span>
          {slot.status === "free" && (
            <button className="px-3 py-1 text-xs font-semibold text-white bg-green-600 rounded-full hover:bg-blue-600">
              Randevu Al
            </button>
          )}
        </div>
      </div>
    ))}
  </div>
);

const WeekSlot = () => (
  <div className="grid grid-cols-1 md:grid-cols-5 gap-6">
    {Object.entries(weekData).map(([day, times]) => (
      <div key={day} className="bg-white rounded-lg shadow p-4">
        <h3 className="font-bold text-gray-800 mb-3 border-b pb-2">{day}</h3>
        {times.length === 0 ? (
          <p className="text-sm text-gray-500">Randevu Yok</p>
        ) : (
          <ul className="text-sm space-y-2">
            {times.map((time, i) => (
              <li key={i} className="text-gray-600 bg-primary/10 rounded-full px-1 text-center">
                {time}
              </li>
            ))}
          </ul>
        )}
      </div>
    ))}
  </div>
);

const MonthSlot = () => {
  const daysInMonth = Array.from({ length: 30 }, (_, i) => i + 1);

  return (
    <div className="grid grid-cols-7 gap-4">
      {daysInMonth.map((day) => (
        <div
          key={day}
          className="bg-white rounded-lg shadow p-3 text-center transition-all hover:shadow-md"
        >
          <div
            className={`font-bold mb-2 ${
              monthData[day] ? "text-blue-600" : "text-gray-700"
            }`}
          >
            {day}
          </div>
          {monthData[day] && (
            <ul className="text-xs space-y-1">
              {monthData[day].map((time, i) => (
                <li key={i} className="text-gray-600 bg-primary/10 rounded-full px-1">
                  {time}
                </li>
              ))}
            </ul>
          )}
        </div>
      ))}
    </div>
  );
};


// ---------------- MAIN DASHBOARD ----------------

export default function Dashboard() {
  const [view, setView] = useState("day");

  const renderView = () => {
    switch (view) {
      case "day":
        return <DaySlot />;
      case "week":
        return <WeekSlot />;
      case "month":
        return <MonthSlot />;
      default:
        return null;
    }
  };

  const title =
    view === "day"
      ? "17 Aralık 2025"
      : view === "week"
      ? "Aralık 17 – Aralık 23, 2025"
      : "Aralık 2025";

  return (
    <div className="p-6 max-w-7xl mx-auto">
      <div className="flex justify-between items-center mb-8">
        <div>
          <h1 className="text-3xl font-bold text-gray-800">Hoşgeldiniz, Dr. Tarık</h1>
          <p className="text-gray-500 mt-1">
            İşte randevu programınız.
          </p>
        </div>

        <div className="flex gap-2">
          {[
            { key: "day", label: "Gün" },
            { key: "week", label: "Hafta" },
            { key: "month", label: "Ay" },
          ].map((btn) => (
            <button
              key={btn.key}
              onClick={() => setView(btn.key)}
              className={`px-4 py-2 rounded-lg text-sm font-semibold transition-all ${
                view === btn.key
                  ? "bg-blue-600 text-white shadow-md"
                  : "bg-white text-gray-700 hover:bg-gray-100"
              }`}
            >
              {btn.label}
            </button>
          ))}
        </div>
      </div>

      <div className="bg-white p-6 rounded-2xl shadow-lg">
        <div className="flex justify-between items-center mb-6">
          <h2 className="text-xl font-bold text-gray-800">{title}</h2>
        </div>
        {renderView()}
      </div>
    </div>
  );
}