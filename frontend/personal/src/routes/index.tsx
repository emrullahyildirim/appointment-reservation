import {
    createBrowserRouter,
    RouterProvider} from "react-router-dom";

import AuthPage from "@/pages/AuthPage";
import MainPage from "@/pages/MainPage";
import ProfilePage from "@/pages/ProfilePage";
import MainLayout from "@/components/layout/MainLayout";
import ChatPage from "@/pages/ChatPage";



const router = createBrowserRouter([
    {
        path: "/auth",
        element: <AuthPage />
    },
    {
        path: "/",
        element: <MainLayout />,
        children: [
            {
                index: true,
                element: <MainPage/>
            },
            {
                path: "/profile",
                element: <ProfilePage/>
            },
            {
                path: "/chat",
                element: <ChatPage/>
            }
        ]
    }
]);

const PageRouter = () => {
    return (
        <RouterProvider router={router} />
    )
};

export default PageRouter;