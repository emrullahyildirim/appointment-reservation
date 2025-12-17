import Topbar from "@/components/Topbar";
import Sidebar from "@/components/Sidebar";
import { Outlet } from "react-router-dom";
import { useState } from "react";

const Layout = () => {
	const [sidebarOpened, setSidebarOpened] = useState(false);
	return (
		<div className="flex flex-col relative overflow-hidden">
			<Topbar setSidebarOpened={setSidebarOpened}/>
			<Sidebar sidebarOpened={sidebarOpened} setSidebarOpened={setSidebarOpened}/>
			<Outlet/>
		</div>
	);
}

export default Layout;