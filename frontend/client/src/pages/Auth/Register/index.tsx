import { Formik, Form, Field } from "formik";
import { IoIdCardOutline } from "react-icons/io5";
import { CiPhone } from "react-icons/ci";
import { CiMail } from "react-icons/ci";
import { MdLockOpen } from "react-icons/md";
import { RegisterWithCredentials } from "@/services/auth";
import { IoPersonOutline } from "react-icons/io5";
import toast from "react-hot-toast";
import { useState } from "react";
import { LiaBirthdayCakeSolid } from "react-icons/lia";
import { TbGenderBigender } from "react-icons/tb";


interface AuthRegisterProps {
  email: string;
  password: string;
  firstName: string;
  lastName: string;
  phoneNumber: string;
  identityNumber: string;
  birthdate: string;
  gender: string;
}

interface RegisterFormProps {
  setForm: (value: string | ((prevVar: string) => string)) => void;
}

const RegisterForm = ({ setForm }: RegisterFormProps) => {
  const [loading, setLoading] = useState(false);

  const initialValues: AuthRegisterProps = {
    email: "",
    password: "",
    firstName: "",
    lastName: "",
    phoneNumber: "",
    identityNumber: "",
    birthdate: "",
    gender: "",
  };

  const handleSubmit = (values: typeof initialValues) => {
    if (loading) return;
    setLoading(true);
    const {
      email,
      password,
      firstName,
      lastName,
      phoneNumber,
      identityNumber,
      birthdate,
      gender,
    } = values;

    const promise = RegisterWithCredentials(
      email,
      password,
      firstName,
      lastName,
      phoneNumber,
      identityNumber,
      birthdate,
      gender
    );

    toast.promise(promise, {
      loading: "Kayıt olunuyor...",
      success: "Başarıyla kayıt olundu!",
      error: "Kayıt olunurken bir hata oluştu.",
    });
    setLoading(false);
  };

  return (
    <div className="flex flex-col items-center gap-8 max-w-87.5">
      <div className="text-center flex flex-col gap-8">
        <h1 className="text-gray-700 text-3xl font-semibold">Kayıt Ol</h1>
        <p className="text-gray-500 text-base ">
          Formu doldurarak kayıt olabilirsiniz.
        </p>
      </div>
      <Formik initialValues={initialValues} onSubmit={handleSubmit}>
        <Form className="flex flex-col gap-8 w-full">
          <div className="flex flex-row items-center bg-primary/10  rounded-lg">
            <div className="h-full pl-4 pr-2">
              <CiMail size={22} className="text-gray-800" />
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
          <div className="flex flex-row items-center bg-primary/10  rounded-lg">
            <div className="h-full pl-4 pr-2">
              <IoPersonOutline size={25} className="text-gray-800" />
            </div>
            <Field
              as="input"
              id="firstName"
              name="firstName"
              placeholder="İsim"
              className="py-3 text-gray-800"
            />
          </div>
		  <div className="flex flex-row items-center bg-primary/10  rounded-lg">
            <div className="h-full pl-4 pr-2">
              <IoPersonOutline size={25} className="text-gray-800" />
            </div>
            <Field
              as="input"
              id="lastName"
              name="lastName"
              placeholder="Soyisim"
              className="py-3 text-gray-800"
            />
          </div>
          <div className="flex flex-row items-center bg-primary/10  rounded-lg">
            <div className="h-full pl-4 pr-2">
              <CiPhone size={25} className="text-gray-800" />
            </div>
            <Field
              as="input"
              id="phoneNumber"
              name="phoneNumber"
              placeholder="Telefon Numarası (0'sız)"
              className="py-3 text-gray-800"
            />
          </div>
          <div className="flex flex-row items-center bg-primary/10  rounded-lg">
            <div className="h-full pl-4 pr-2">
              <IoIdCardOutline size={25} className="text-gray-800" />
            </div>
            <Field
              as="input"
              id="identityNumber"
              name="identityNumber"
              placeholder="TC Kimlik Numarası"
              className="py-3 text-gray-800"
            />
          </div>
          <div className="flex flex-row items-center bg-primary/10  rounded-lg">
            <div className="h-full pl-4 pr-2">
              <LiaBirthdayCakeSolid size={25} className="text-gray-800" />
            </div>
            <Field
              as="input"
              id="birthdate"
              name="birthdate"
              placeholder="Doğum tarihi (YY-AA-GG)"
              className="py-3 text-gray-800"
            />
          </div>
          <div className="flex flex-row items-center bg-primary/10  rounded-lg">
            <div className="h-full pl-4 pr-2">
              <TbGenderBigender size={25} className="text-gray-800" />
            </div>
            <Field
              as="input"
              id="gender"
              name="gender"
              placeholder="Cinsiyetiniz."
              className="py-3 text-gray-800"
            />
          </div>
          <button
            type="submit"
            disabled={loading}
            className="bg-primary disabled:bg-primary/10 hover:bg-primary/80 cursor-pointer transition-colors text-white rounded-md py-3 w-fit px-10 m-auto"
          >
            {loading ? "Yükleniyor..." : "Kayıt Ol"}
          </button>
        </Form>
      </Formik>
      <p className="text-gray-600 font-base text-center">
        Hesabın var mı? Hemen <span
          className="font-medium cursor-pointer"
          onClick={() => setForm("login")}>Giriş Yap
        </span>
        !
      </p>
    </div>
  );
};

export default RegisterForm;
