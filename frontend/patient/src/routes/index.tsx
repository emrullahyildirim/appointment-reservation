import {
  createBrowserRouter,
  RouterProvider,
} from "react-router-dom";

import AuthPage from "@/pages/Auth";
import Layout from "@/containers/Layout";
import HomePage from "@/pages/Home";
import DoctorsPage from "@/pages/Doctors";

import { Toaster } from "react-hot-toast";
import AppointmentsPage from "@/pages/Appointments";

const router = createBrowserRouter([
	{
		path: "/auth",
		element: <AuthPage/>,
	},
	{
		path: "/",
		element: <Layout/>,
		children: [
			{ path: "/doctors", element: <DoctorsPage/> },
			{ path: "/appointments", element: <AppointmentsPage/>},
			{ path: "/", element: <HomePage/> }
		]
	}
]);

const PageRouter = () => {
	return (
		<>
			<Toaster/>
			<RouterProvider router={router} />
		</>
	)
};

export default PageRouter;
