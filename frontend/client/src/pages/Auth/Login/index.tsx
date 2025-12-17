import { Formik, Form, Field } from "formik";
import { FaRegUser } from "react-icons/fa";
import { MdLockOpen } from "react-icons/md";
import { LoginWithCredentials } from "@/services/auth";
import toast from "react-hot-toast";
import { useNavigate } from "react-router-dom";
import { useState } from "react";

interface AuthLoginProps {
  email: string;
  password: string;
}

interface AuthFormProps {
  setForm: (value: string | ((prevVar: string) => string)) => void;
}

const RegisterForm = ({ setForm }: AuthFormProps) => {
  const [loading, setLoading] = useState(false);
	const navigate = useNavigate();
  const initialValues: AuthLoginProps = {
    email: "",
    password: "",
  };

  const handleSubmit = (values: typeof initialValues) => {
    if (loading) return;
    setLoading(true);
    const {
      email,
      password,
    } = values;

    const promise = LoginWithCredentials(
      email,
      password,
    );
	
    toast.promise(promise, {
      loading: "Giriş yapılıyor...",
      success: "Başarıyla giriş yapıldı!",
      error: "Giriş yapılırken bir hata oluştu.",
    });
    setLoading(false);
  };

  return (
    <div className="flex flex-col items-center gap-8 max-w-87.5">
      <div className="text-center flex flex-col gap-8">
        <h1 className="text-gray-700 text-3xl font-semibold">Giriş Yap</h1>
        <p className="text-gray-500 text-base ">
          Formu doldurarak kayıt olabilirsiniz.
        </p>
      </div>
      <Formik initialValues={initialValues} onSubmit={handleSubmit}>
        <Form className="flex flex-col gap-8 w-full">
          <div className="flex flex-row items-center bg-primary/10  rounded-lg">
            <div className="h-full pl-4 pr-2">
              <FaRegUser size={22} className="text-gray-800" />
            </div>
            <Field
              as="input"
              id="email"
              name="email"
              placeholder="E-Mail Adresiniz"
              className="py-3 text-gray-800"
            />
          </div>
          <div className="flex flex-row items-center bg-primary/10  rounded-lg">
            <div className="h-full pl-4 pr-2">
              <MdLockOpen size={25} className="text-gray-800" />
            </div>
            <Field
              as="input"
              id="password"
              name="password"
              placeholder="Şifre"
              type="text"
              className="py-3 text-gray-800"
            />
          </div>

          <button
            type="submit"
            disabled={loading}
            className="bg-primary disabled:bg-primary/10 hover:bg-primary/80 cursor-pointer transition-colors text-white rounded-md py-3 w-fit px-10 m-auto"
          >
            {loading ? "Yükleniyor..." : "Giriş Yap"}
          </button>
        </Form>
      </Formik>
      <p className="text-gray-600 font-base text-center">
        Hesabın yok mu? Hemen <span
          className="font-medium cursor-pointer"
          onClick={() => setForm("register")}
        >
          kayıt ol!
        </span>
        !
      </p>
    </div>
  );
};

export default RegisterForm;
