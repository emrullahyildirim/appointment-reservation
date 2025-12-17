import { Link } from "react-router-dom";
import Wrapper from "../Wrapper";
import { RxHamburgerMenu } from "react-icons/rx";

interface TopbarProps {
	setSidebarOpened: (value: boolean | ((prevVar: boolean) => boolean)) => void;
}

const Topbar = ({ setSidebarOpened }: TopbarProps) => {
	const links = [
		{ href: "/", label: "Ana Sayfa" },
		{ href: "/doctors", label: "Doktorlar"},
		{ href: "/appointments", label: "Randevularım"}
	]
	return (
		<section
			className="flex justify-between items-center h-25 w-full px-8" 
		>
			<Wrapper className="flex  justify-between items-center">
				<Link to="/" className="flex gap-2 items-center justify-start flex-1">
					<img src="logo.png" className="w-12.5 h-12.5"/>
					<p className="text-primary font-bold xl:text-2xl text-xl lg:block hidden">Smart<span className="text-secondary">Appointment</span></p>
				</Link>
				<div
					className="gap-8 items-center justify-center flex-1 md:flex hidden"
				>
					{
						links.map((link, key) => (
							<Link 
								className="text-secondary hover:text-secondary/80 transition-all xl:text-base text-sm font-medium text-nowrap"
								key={key}
								to={link.href}
							>
								{link.label}
							</Link>
						))
					}
				</div>
				<div
					className="flex gap-8 flex-1 justify-end items-center"
				>
					<Link 
						className="bg-primary hover:bg-primary/90 transition-colors text-white text-base px-8 py-3 rounded-lg whitespace-nowrap"
						to="/auth"
					>
						Randevu Al
					</Link>
					<RxHamburgerMenu className="md:hidden block cursor-pointer" size={32} onClick={() => setSidebarOpened(prev => !prev)}/>
				</div>
			</Wrapper>
		</section>
	)
}

export default Topbar;