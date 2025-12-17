import { useState } from "react";
import { useNavigate } from "react-router-dom";

export default function LoginPage() {
    const [email, setEmail] = useState("");

    const isValidEmail = (value: string) =>
        /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value);

    const handleEmailChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setEmail(e.target.value);
    };
    const [password, setPassword] = useState("");
    const [rememberMe, setRememberMe] = useState(false);
    const [error, setError] = useState("");
    const navigate = useNavigate();



    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();

        setError("");
    };

    return (
        <div className="min-h-screen flex items-center justify-center bg-gray-50 px-4">
            <div className="w-full max-w-md bg-white rounded-2xl shadow-lg p-6 sm:p-8">
                <h1 className="text-2xl font-semibold text-gray-900 text-center">
                    Giriş Yap
                </h1>

                <p className="text-sm text-gray-500 text-center mt-2">
                    Devam etmek için bilgilerinizi girin
                </p>

                <form onSubmit={handleSubmit} className="mt-6 space-y-4">
                    <div>
                        <label
                            htmlFor="tc"
                            className="block text-sm font-medium text-gray-700"
                        >
                            TC Kimlik No
                        </label>
                        <input
                            id="email"
                            name="email"
                            type="email"
                            autoComplete="email"
                            value={email}
                            onChange={handleEmailChange}
                            aria-invalid={email.length > 0 && !isValidEmail(email)}
                            aria-describedby="email-help"
                            placeholder="ornek@mail.com"
                            className={`mt-1 w-full rounded-lg border px-3 py-2
    focus:outline-none focus:ring-2
    ${email.length > 0 && !isValidEmail(email)
                                    ? "border-red-500 focus:ring-red-500"
                                    : "border-gray-300 focus:ring-primary focus:border-primary"}`}
                        />

                    </div>

                    <div>
                        <label
                            htmlFor="password"
                            className="block text-sm font-medium text-gray-700"
                        >
                            Şifre
                        </label>
                        <input
                            id="password"
                            name="password"
                            type="password"
                            autoComplete="current-password"
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                            className="mt-1 w-full rounded-lg border border-gray-300 px-3 py-2
                         focus:outline-none focus:ring-2 focus:ring-primary
                         focus:border-primary"
                        />
                    </div>

                    <div className="flex items-center justify-between">
                        <label className="flex items-center gap-2 text-sm text-gray-700">
                            <input
                                type="checkbox"
                                checked={rememberMe}
                                onChange={(e) => setRememberMe(e.target.checked)}
                                className="h-4 w-4 rounded border-gray-300 text-primary focus:ring-primary"
                            />
                            Beni hatırla
                        </label>

                        <button
                            type="button"
                            className="text-sm text-primary hover:underline"
                        >
                            Şifremi unuttum
                        </button>
                    </div>

                    {error && (
                        <p
                            role="alert"
                            className="text-sm text-error bg-error/10 rounded-lg px-3 py-2"
                        >
                            {error}
                        </p>
                    )}
                    <button
                        onClick={() => navigate("/")}
                        type="submit"
                        className="w-full rounded-lg bg-primary py-2.5 text-white
                       font-medium hover:bg-primary/90
                       focus:outline-none focus:ring-2 focus:ring-primary
                       focus:ring-offset-2"
                    >
                        Giriş Yap
                    </button>
                </form>
            </div>
        </div>
    );
}
