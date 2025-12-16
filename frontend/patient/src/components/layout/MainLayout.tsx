
import Header from "@/components/layout/Header";
import { Outlet } from "react-router-dom";


const MainLayout = () => {
  return (
    <>
      <Header />
      <Outlet />
    </>
  );
};

export default MainLayout;
