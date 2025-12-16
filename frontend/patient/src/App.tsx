import { Routes, Route } from "react-router-dom";
import MainLayout from "@/components/layout/MainLayout";
import LoginPage from "./pages/Login";
import BookAppointment from "./pages/BookAppointment";
import ProfilePage from "./pages/ProfilePage";
import MainPage from "./pages/MainPage"
import AppointmentSDetail from "./pages/AppointmentDetail";

export default function App() {
  return (
    <div>
      <Routes>
        <Route path="login" element={<LoginPage />} />
      <Route path="/" element={<MainLayout />}>
        <Route index element={<MainPage />} />
        <Route path="book-appointment" element={<BookAppointment />} />
        <Route path="profile" element={<ProfilePage />} />
        <Route path="appointment-detail" element={<AppointmentSDetail />} />
      </Route>
      </Routes>
    </div>
  );
}

