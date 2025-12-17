import { Link } from "react-router-dom";
import { IoMdClose } from "react-icons/io";

interface SidebarProps {
	setSidebarOpened: (value: boolean | ((prevVar: boolean) => boolean)) => void;
	sidebarOpened: boolean;
}

const Sidebar = ({ sidebarOpened, setSidebarOpened}: SidebarProps) => {
  const links = [
    { href: "/", label: "Ana Sayfa" },
    { href: "/doctors", label: "Doktorlar" },
    { href: "/appointments", label: "Randevularım" },
  ];
  return (
    <div className={`absolute h-screen p-5 z-10 w-full bg-white flex flex-col items-center gap-10 transition-all md:-right-full ${sidebarOpened ? "right-0" : "-right-full"} top-0`}>
		<div className="w-full flex justify-end cursor-pointer">
			<IoMdClose size={25} onClick={() => setSidebarOpened(false)}/>
		</div>
      {links.map((link, key) => (
        <Link
          className="text-gray-700 hover:text-gray-600/80 transition-all text-base font-medium uppercase ease-in-out"
          key={key}
          to={link.href}
        >
          {link.label}
        </Link>
      ))}
    </div>
  );
};

export default Sidebar;
