import { ReactNode } from "react";

interface WrapperProps {
	className: string;
	children: ReactNode
}

const Wrapper = ({children, className}: WrapperProps) => {
	return (
		<div className={`max-w-310 mx-auto w-full h-full ${className}`} >
			{children}
		</div>
	)
}

export default Wrapper;