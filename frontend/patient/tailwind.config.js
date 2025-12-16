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
