import { initializeApp } from 'firebase/app';
import { getFirestore } from 'firebase/firestore';

const firebaseConfig = {
    apiKey: "AIzaSyBgG4toihrz9547BXPgReCXk4kYEYXORPQ",
    authDomain: "hackathon-208ed.firebaseapp.com",
    projectId: "hackathon-208ed",
    storageBucket: "hackathon-208ed.firebasestorage.app",
    messagingSenderId: "852162920468",
    appId: "1:852162920468:web:e2b52448fb51ad53912701",
    measurementId: "G-8RMCLYPLEM"
};

const app = initializeApp(firebaseConfig);

export const db = getFirestore(app);