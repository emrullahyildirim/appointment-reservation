import { useState } from "react";
import LoginForm from "./Login";
import RegisterForm from "./Register";

const AuthPage = () => {
  const [form, setForm] = useState("login");

  return (
    <main className="flex h-screen w-full">
      <div className="w-1/2  relative flex justify-center items-center">
	  	<div className="absolute bottom-0 left-0 -translate-x-1/4 translate-y-1/4 w-[800px] h-[800px] blur-[1200px] bg-primary">

		</div>
	  	<img src="doctors.png" className="w-2/3 animation-wiggle"/>
	  </div>
      <div className="w-1/2 p-10 flex justify-center items-center">
		{
			form === "login" ? <LoginForm setForm={setForm}/> : <RegisterForm setForm={setForm}/>
		}
	  </div>
    </main>
  );
};

export default AuthPage;
