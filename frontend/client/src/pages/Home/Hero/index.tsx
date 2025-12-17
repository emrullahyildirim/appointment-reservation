import Wrapper from "@/components/Wrapper";
import AppointmentSearch from "./AppointmentSearch";

const Hero = () => {
	return (
		<section className="py-25 px-8 w-full flex items-center justify-center relative min-h-[calc(100vh-100px)]">
			<Wrapper className="flex items-center justify-center w-full">
				<div className="absolute right-1/4 bottom-1/2 translate-y-1/2 h-100 w-100 bg-primary blur-[230px] -z-10">sa</div>
				<div className="flex flex-col gap-8 w-full">
					<div className="text-center flex flex-col gap-4">
						<h2
							className="text-5xl text-secondary font-bold"
						>
							Sağlığınızı koruyun.
						</h2>
						<p className="text-2xl text-secondary font-base">Alanlarında en iyisi olan doktorlarımızla bir randevu ayarlayın ve bir ömür sağlıklı kalın.</p>
					</div>
					<AppointmentSearch />
				</div>
			</Wrapper>
		</section>
	)
}

export default Hero;