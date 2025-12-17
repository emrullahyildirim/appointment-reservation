import Wrapper from "@/components/Wrapper"

const awaitingAppointments = [
	{doctor: "İsmet Özcan", title: "Çocuk Cerrahisi Uzmanı", date: "18 Aralık 13:00"}
]


const AppointmentsCard = ({doctor, title, date}: {doctor: string, title: string, date: string }) => {
	return (
		<div className="bg-white rounded-md w-full overflow-hidden shadow-md ">
			<div className="p-4 flex flex-col gap-4 min-h-[100px] justify-between">
				<div>
					<p className="text-gray-700 text-lg font-bold">{date}</p>
					<p className="text-gray-500">Bölüm: {title}</p>
				</div>
				<p>Doktor: {doctor}</p>
				<button className="border border-red-500 text-red-500 px-4 py-2 rounded-md hover:bg-red-500 hover:text-white transition-all cursor-pointer">Randevuyu İptal Et</button>
			</div>
		</div>
	)
}

const AppointmentsGroup = ({title, appointments}:{title: string, appointments: typeof awaitingAppointments}) => {
	return (
		<div className="flex flex-col gap-4">
		<h2 className="text-3xl font-semibold text-gray-700">{title}</h2>
		<div className="grid lg:grid-cols-4 md:grid-cols-2 grid-cols-1 gap-4">
			{
				appointments.length > 0 
				? 
				appointments.map((appointment, key) => (
					<AppointmentsCard key={key} doctor={appointment.doctor} title={appointment.title} date={appointment.date}/>
				)) 
				:
				<p className="text-lg text-gray-700">Randevu bulunamadı.</p>
			}
		</div>
		</div>
	)
}


const AppointmentsPage = () => {
	return (
		<section className="p-8 w-full flex  relative min-h-[calc(100vh-100px)]">
      		<Wrapper className="flex flex-col gap-16">
				<AppointmentsGroup title="Aktif Randevular" appointments={awaitingAppointments} />
				<AppointmentsGroup title="Geçmiş Randevular" appointments={[]} />
			</Wrapper>
    	</section>
	)
}

export default AppointmentsPage;