import axios from "axios";

export const LoginWithCredentials = (email: string, password: string) => {
	return axios.post("http://localhost:3001/login", { email, password }); 
}

export const RegisterWithCredentials = (email: string, password: string, firstName: string, lastName: string, phoneNumber: string, identityNumber: string, birthdate: string, gender: string) => {
	return axios.post("/api/auth/register", {email, password, firstName, lastName, phoneNumber, identityNumber, birthdate, gender});
}

