import Button from "../common/Button";
import { useNavigate } from "react-router-dom";


export default function Header() {
  const navigate = useNavigate();
  return (
    <header className="w-full bg-white border-b">
      <div className="max-w-7xl mx-auto px-6 h-20 flex items-center justify-between">
        <div className="flex items-center gap-3">
          <Button text="Randevu Al" onClick={() => navigate("/book-appointment")} />
        </div>
        <div className="flex items-center gap-3">
          <span className="text-lg font-medium text-black">
            Tarık Akgün
          </span>

          <button
            type="button"
            className="w-12 h-12 rounded-full bg-secondary flex items-center justify-center
             text-white text-lg font-medium
             hover:bg-secondary/90 focus:outline-none focus:ring-2 focus:ring-primary"
             onClick={() => navigate("/profile")}
          >
            T
          </button>
        </div>
      </div>
    </header>
  );
}
