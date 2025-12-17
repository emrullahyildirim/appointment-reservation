import {
  createBrowserRouter,
  RouterProvider,
} from "react-router-dom";

import AuthPage from "@/pages/Auth";
import Layout from "@/containers/Layout";
import HomePage from "@/pages/Home";
import { Toaster } from "react-hot-toast";

const router = createBrowserRouter([
	{
		path: "/auth",
		element: <AuthPage/>,
	},
	{
		path: "/",
		element: <Layout/>,
		children: [
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
