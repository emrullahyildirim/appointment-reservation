import { useState } from "react";
import Button from "@/components/common/Button";
import { useNavigate } from "react-router-dom";


export default function LoginPage() {
  const [tab, setTab] = useState<"login" | "register">("login");

  const [tc, setTc] = useState("");
  const [password, setPassword] = useState("");
  const [rememberMe, setRememberMe] = useState(false);
  const navigate = useNavigate();

  const handleTcChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setTc(e.target.value.replace(/\D/g, "").slice(0, 11));
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-50 px-4">
      <div className="w-full max-w-md bg-white rounded-2xl shadow-lg p-6">
        <div className="flex mb-6 bg-gray-100 rounded-lg p-1">
          <button
            onClick={() => setTab("login")}
            className={`flex-1 py-2 text-sm rounded-md ${
              tab === "login"
                ? "bg-white text-primary shadow"
                : "text-gray-500"
            }`}
          >
            Giriş Yap
          </button>
          <button
            onClick={() => setTab("register")}
            className={`flex-1 py-2 text-sm rounded-md ${
              tab === "register"
                ? "bg-white text-primary shadow"
                : "text-gray-500"
            }`}
          >
            Kayıt Ol
          </button>
        </div>

        {tab === "login" ? (
          <form className="space-y-4">
            <div>
              <label className="text-sm text-gray-700">TC Kimlik No</label>
              <input
                value={tc}
                onChange={handleTcChange}
                className="mt-1 w-full rounded-lg border px-3 py-2 focus:ring-2 focus:ring-primary"
              />
            </div>

            <div>
              <label className="text-sm text-gray-700">Şifre</label>
              <input
                type="password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                className="mt-1 w-full rounded-lg border px-3 py-2 focus:ring-2 focus:ring-primary"
              />
            </div>

            <div className="flex items-center justify-between">
              <label className="flex items-center gap-2 text-sm">
                <input
                  type="checkbox"
                  checked={rememberMe}
                  onChange={(e) => setRememberMe(e.target.checked)}
                />
                Beni hatırla
              </label>

              <Button text="Şifremi unuttum" onClick={() => {}} />
            </div>

            <Button text="Giriş Yap" onClick={() => navigate("/")} />
          </form>
        ) : (
          <form className="space-y-3">
            <input placeholder="TC Kimlik No" className="w-full border rounded-lg px-3 py-2" />
            <input placeholder="Ad Soyad" className="w-full border rounded-lg px-3 py-2" />
            <input placeholder="GSM" className="w-full border rounded-lg px-3 py-2" />
            <input placeholder="E-posta" className="w-full border rounded-lg px-3 py-2" />
            <input type="date" className="w-full border rounded-lg px-3 py-2" />

            <select className="w-full border rounded-lg px-3 py-2">
              <option>Cinsiyet</option>
              <option>Kadın</option>
              <option>Erkek</option>
            </select>

            <input type="password" placeholder="Şifre" className="w-full border rounded-lg px-3 py-2" />
            <input type="password" placeholder="Şifre Tekrar" className="w-full border rounded-lg px-3 py-2" />

            <Button text="Kayıt Ol" onClick={() => navigate("/")}/>
          </form>
        )}
      </div>
    </div>
  );
}
