/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./index.html",
    "./src/**/*.{js,ts,jsx,tsx}",
  ],
  theme: {
    extend: {
      colors: {
        primary: '#512888',
        secondary: '#9683EC',
        error: '#D53434',
        info: '#CFA83D',
        succes: '#30BAA5',
        'blue-500': '#3B82F6',
        'blue-600': '#2563EB',
        'green-500': '#22C55E',
        'green-600': '#16A34A',
        'red-500': '#EF4444',
        'red-600': '#DC2626',
      },
      fontFamily: {
        sans: ['Inter', 'sans-serif'],
      },
    },
    screens: {
      'xs': '320px',
      'sm': '480px',
      'md': '768px',
      'lg': '1024px',
      'xl': '1280px',
    },
  },
  plugins: [],
}