import { useState } from 'react';
import { NavLink } from 'react-router-dom';

const navigation = [
  {
    name: 'Panel',
    path: '/',
    icon: (
      <svg xmlns="http://www.w3.org/2000/svg" className="h-6 w-6 flex-shrink-0" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={2}>
        <path strokeLinecap="round" strokeLinejoin="round" d="M4 6a2 2 0 012-2h12a2 2 0 012 2v2a2 2 0 01-2 2H6a2 2 0 01-2-2V6zM4 12a2 2 0 012-2h12a2 2 0 012 2v2a2 2 0 01-2 2H6a2 2 0 01-2-2v-2zM4 18a2 2 0 012-2h12a2 2 0 012 2v2a2 2 0 01-2 2H6a2 2 0 01-2-2v-2z" />
      </svg>
    ),
  },
  {
    name: 'Sohbet',
    path: '/chat',
    icon: (
        <svg xmlns="http://www.w3.org/2000/svg" className="h-6 w-6 flex-shrink-0" fill="none" viewBox="0 0 24 24" strokeWidth={1.5} stroke="currentColor">
            <path strokeLinecap="round" strokeLinejoin="round" d="M12 20.25c4.97 0 9-3.694 9-8.25s-4.03-8.25-9-8.25S3 7.006 3 11.5c0 2.083.804 3.996 2.133 5.448V21l3.58-2.046A9.015 9.015 0 0012 20.25z" />
        </svg>
    ),
  },
  {
    name: 'Profil',
    path: '/profile',
    icon: (
      <svg xmlns="http://www.w3.org/2000/svg" className="h-6 w-6 flex-shrink-0" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={2}>
        <path strokeLinecap="round" strokeLinejoin="round" d="M15.75 6a3.75 3.75 0 11-7.5 0 3.75 3.75 0 017.5 0zM4.501 20.118a7.5 7.5 0 0114.998 0A17.933 17.933 0 0112 21.75c-2.676 0-5.216-.584-7.499-1.632z" />
      </svg>
    ),
  },
];

export default function SideBar() {
    const [isExpanded, setIsExpanded] = useState(true);

    return (
        <div className={`
            relative flex flex-col bg-white shadow-xl transition-all duration-300 ease-in-out h-full
            ${isExpanded ? 'w-64' : 'w-20'}
        `}>
            <div className="flex items-center h-20 border-b px-4">
                <button 
                    className="p-1.5 rounded-full hover:bg-gray-100 absolute top-6 bg-white border shadow-md
                    ${isExpanded ? 'right-[-12px]' : 'right-[-12px]'}"
                    onClick={() => setIsExpanded(!isExpanded)}
                >
                     <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth={2} stroke="currentColor" className="w-5 h-5 transition-transform duration-300" style={{ transform: isExpanded ? 'rotate(180deg)' : 'rotate(0deg)' }}>
                        <path strokeLinecap="round" strokeLinejoin="round" d="M15.75 19.5L8.25 12l7.5-7.5" />
                    </svg>
                </button>
            </div>
            <nav className="flex-1 mt-4">
                {navigation.map((item) => (
                    <NavLink
                        key={item.name}
                        to={item.path}
                        title={isExpanded ? '' : item.name}
                        className={({ isActive }) =>
                            `flex items-center my-2 mx-3 px-4 py-3 text-gray-700 rounded-lg transition-colors duration-200
                            ${ isExpanded ? 'justify-start' : 'justify-center' }
                            ${ isActive 
                                ? "bg-blue-100 text-blue-600 font-bold" 
                                : "hover:bg-gray-100 hover:text-gray-900" 
                            }`
                        }
                    >
                        {item.icon}
                        <span className={`overflow-hidden whitespace-nowrap transition-all duration-200 font-medium ${isExpanded ? 'ml-4 w-full' : 'w-0'}`}>
                            {item.name}
                        </span>
                    </NavLink>
                ))}
            </nav>

            <div className={`border-t p-4 flex items-center`}>
                <img className="w-10 h-10 rounded-full flex-shrink-0" src="https://i.pravatar.cc/150?u=1" alt="User Avatar" />
                <div className={`overflow-hidden whitespace-nowrap transition-all duration-200 ${isExpanded ? 'ml-3 w-full' : 'w-0'}`}>
                    <p className="text-sm font-semibold text-gray-800">Dr. Aydın</p>
                    <p className="text-xs text-gray-500">Çevrimiçi</p>
                </div>
            </div>
        </div>
    );
}