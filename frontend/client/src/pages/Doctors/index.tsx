import Wrapper from "@/components/Wrapper";
import { MdChatBubble } from "react-icons/md";

const Doctors = [
	{name: "İsmet Özcan", 	title: "Çocuk Cerrahisi Uzmanı", image: "https://www.merkezsaglikgrubu.com.tr/images/doktorlar/doktor-1.jpg"},
	{name: "Buket Aksaç", 	title: "Dahiliye (İç Hastalıkları) Uzmanı", image: "https://www.merkezsaglikgrubu.com.tr/images/doktorlar/doktor-33.jpg"},
	{name: "Aygün Aliyeva", title: "Pratisyen Hekim", image: "https://www.merkezsaglikgrubu.com.tr/images/doktorlar/doktor-871135538.jpg"},
	{name: "Cihangir Ay", 	title: "Pratisyen Hekim", image: "https://www.merkezsaglikgrubu.com.tr/images/doktorlar/doktor-361575346.jpg"},
]

const DoctorsCard = ({name, title, image}: {name: string, title: string, image: string }) => {
	return (
		<div className="bg-white rounded-md w-full overflow-hidden shadow-md">
			<img src={image} className="w-full h-[300px] object-cover"/>
			<div className="p-4 flex flex-col gap-4">
				<div>
					<p className="text-gray-700 text-lg font-bold">{name}</p>
					<p className="text-gray-500">{title}</p>
				</div>
				<button className="flex gap-2 bg-primary text-white text-center items-center justify-center w-full px-4 py-2 rounded-md font-semibold cursor-pointer hover:bg-primary/90 transition">
					<MdChatBubble size={18}/>
					Mesaj Gönder
				</button>
			</div>

		</div>
	)
}

const DoctorsPage = () => {
  return (
    <section className="p-8 w-full flex  relative min-h-[calc(100vh-100px)]">
      <Wrapper className="flex flex-col gap-4">
		<h2 className="text-3xl font-semibold text-gray-700">Doktor Kadromuz</h2>
		<div className="grid lg:grid-cols-4 md:grid-cols-2 grid-cols-1 gap-4">
			{
				Doctors.map((doctor, key) => (
					<DoctorsCard key={key} name={doctor.name} title={doctor.title} image={doctor.image}/>
				))
			}
		</div>
      </Wrapper>
    </section>
  );
};

export default DoctorsPage;
