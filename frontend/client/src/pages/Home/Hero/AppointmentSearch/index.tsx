import { FaRegUser } from "react-icons/fa";
import { MdKeyboardArrowDown } from "react-icons/md";
import { FaRegCalendarAlt } from "react-icons/fa";
import { GiMedicines } from "react-icons/gi";
import { FaSearch } from "react-icons/fa";

const AppointmentSearch = () => {
	return (
		<div className="flex flex-col lg:flex-row lg:gap-0 gap-4 justify-between bg-white shadow-sm  p-8 rounded-md w-full">
			<div className="flex  items-center gap-4 min-w-[250px] px-4 lg:py-0 py-4 border-b-2 lg:border-b-0 lg:border-r-2 border-gray-200 flex-1">
				<GiMedicines size={36} className="text-primary"/>
				<div className="flex flex-col gap-1">
					<div className="flex gap-1 items-center text-gray-600 text-sm ">
						<p>Bölüm</p>
						<MdKeyboardArrowDown/>
					</div>
					<p>Bölüm seçiniz...</p>
				</div>
			</div>
			<div className="flex items-center gap-4 min-w-[250px] px-4 lg:py-0 py-4 border-b-2 lg:border-b-0 lg:border-r-2 border-gray-200 flex-1">
				<FaRegUser size={36} className="text-primary"/>
				<div className="flex flex-col gap-1">
					<div className="flex gap-1 items-center text-gray-600 text-sm ">
						<p>Doktor</p>
						<MdKeyboardArrowDown/>
					</div>
					<p>Herhangi bir doktor</p>
				</div>
			</div>
			<div className="flex items-center gap-4 min-w-[250px] px-4 flex-1">
				<FaRegCalendarAlt size={36} className="text-primary"/>
				<div className="flex flex-col gap-1">
					<div className="flex gap-1 items-center text-gray-600 text-sm ">
						<p>Tarih</p>
						<MdKeyboardArrowDown/>
					</div>
					<p>Herhangi bir zaman</p>
				</div>
			</div>
			<div className="flex gap-2 bg-primary hover:bg-primary/90 transition cursor-pointer rounded-md p-3 lg:w-12 lg:h-12 text-white flex items-center justify-center">
				<FaSearch size={20} className=" text-white"/>
				<p className="text-white lg:hidden">Randevu ara</p>
			</div>
		</div>
	)
}

export default AppointmentSearch;