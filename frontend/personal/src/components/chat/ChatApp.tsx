import React, { useEffect, useState, useRef } from "react";
import { db } from "@/firebase";
import {
  addDoc,
  collection,
  onSnapshot,
  orderBy,
  query,
  Timestamp,
} from "firebase/firestore";

// Mock user data with avatars
const users = {
  "1": { name: "Dr. Aydın", avatar: "https://i.pravatar.cc/150?u=1" },
  "2": { name: "Mehmet Yılmaz", avatar: "https://i.pravatar.cc/150?u=2" },
};

// 🔹 Message tipi
type ChatMessage = {
  id: string;
  senderId: string;
  content: string;
  timestamp: Timestamp;
};

// 🔹 Chat tipi
type Chat = {
  id: string;
  users: string[];
  lastMessage: string;
};

// Auth sisteminden gelen kullanıcı ID
const currentUserId = "1";

const ChatApp: React.FC = () => {
  const [chats] = useState<Chat[]>([
    { id: "1xd1", users: ["1", "2"], lastMessage: "" },
  ]);
  const [selectedChat, setSelectedChat] = useState<Chat | null>(null);
  const [messages, setMessages] = useState<ChatMessage[]>([]);
  const [newMessage, setNewMessage] = useState("");
  const messagesEndRef = useRef<null | HTMLDivElement>(null);

  const scrollToBottom = () => {
    messagesEndRef.current?.scrollIntoView({ behavior: "smooth" });
  };

  useEffect(scrollToBottom, [messages]);

  useEffect(() => {
    if (!selectedChat) return;

    const q = query(
      collection(db, "chats", selectedChat.id, "messages"),
      orderBy("timestamp", "asc")
    );

    const unsubscribe = onSnapshot(q, (snapshot) => {
      const fetchedMessages: ChatMessage[] = snapshot.docs.map((doc) => ({
        id: doc.id,
        ...(doc.data() as Omit<ChatMessage, "id">),
      }));
      setMessages(fetchedMessages);
    });

    return unsubscribe;
  }, [selectedChat]);

  const sendMessage = async () => {
    if (!newMessage.trim() || !selectedChat) return;
    await addDoc(collection(db, "chats", selectedChat.id, "messages"), {
      senderId: currentUserId,
      content: newMessage,
      timestamp: Timestamp.now(),
    });
    setNewMessage("");
  };

  const handleKeyPress = (e: React.KeyboardEvent<HTMLInputElement>) => {
    if (e.key === "Enter") sendMessage();
  };

  const getOtherUser = (chat: Chat) => {
    const otherUserId = chat.users.find((u) => u !== currentUserId);
    return otherUserId ? users[otherUserId as keyof typeof users] : null;
  };
  
  const handleSelectChat = (chat: Chat) => {
    setSelectedChat(chat);
  }

  return (
    <div className="flex h-[calc(100vh-8rem)] bg-gray-50 rounded-lg shadow-lg overflow-hidden">
      <div className={`
        w-full md:w-1/3 bg-white border-r border-gray-200 rounded-l-lg
        ${selectedChat ? 'hidden md:flex flex-col' : 'flex flex-col'}
      `}>
        <div className="p-4 border-b border-gray-200">
          <h1 className="text-2xl font-bold text-gray-800">Sohbetler</h1>
        </div>
        <div className="overflow-y-auto">
          {chats.map((chat) => {
            const otherUser = getOtherUser(chat);
            return (
              <div
                key={chat.id}
                className="flex items-center p-4 cursor-pointer transition-colors duration-200 hover:bg-gray-100"
                onClick={() => handleSelectChat(chat)}
              >
                <img src={otherUser?.avatar} alt={otherUser?.name} className="w-12 h-12 rounded-full mr-4" />
                <div className="flex-1">
                  <div className="font-semibold text-gray-900">{otherUser?.name}</div>
                  <p className="text-sm text-gray-600 truncate">{chat.lastMessage}</p>
                </div>
              </div>
            );
          })}
        </div>
      </div>

      <div className={`
        w-full md:flex-1 flex flex-col bg-gray-50 rounded-r-lg
        ${selectedChat ? 'flex' : 'hidden md:flex'}
      `}>
        {selectedChat ? (
          <>
            <div className="flex items-center p-3 border-b border-gray-200 bg-white rounded-tr-lg">
                <button 
                  className="mr-2 md:hidden p-1 rounded-full hover:bg-gray-200"
                  onClick={() => setSelectedChat(null)}
                >
                  <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth={1.5} stroke="currentColor" className="w-6 h-6">
                    <path strokeLinecap="round" strokeLinejoin="round" d="M15.75 19.5L8.25 12l7.5-7.5" />
                  </svg>
                </button>
                <img src={getOtherUser(selectedChat)?.avatar} alt="Avatar" className="w-10 h-10 rounded-full mr-3" />
                <h2 className="text-xl font-bold text-gray-800">{getOtherUser(selectedChat)?.name}</h2>
            </div>

            <div className="flex-1 p-6 overflow-y-auto">
              <div className="space-y-5">
                {messages.map((msg) => {
                    const sentByUser = msg.senderId === currentUserId;
                    const sender = users[msg.senderId as keyof typeof users];
                    return (
                        <div key={msg.id} className={`flex items-start gap-3 ${sentByUser ? "justify-end" : ""}`}>
                            {!sentByUser && <img src={sender?.avatar} alt={sender?.name} className="w-8 h-8 rounded-full" />}
                            <div className={`max-w-xs lg:max-w-md p-3 rounded-2xl shadow-sm ${
                                sentByUser ? "bg-blue-600 text-white rounded-br-none" : "bg-white text-gray-800 rounded-bl-none"
                            }`}>
                                <p className="break-words">{msg.content}</p>
                            </div>
                            {sentByUser && <img src={sender?.avatar} alt={sender?.name} className="w-8 h-8 rounded-full" />}
                        </div>
                    )
                })}
                <div ref={messagesEndRef} />
              </div>
            </div>

            <div className="p-4 bg-white border-t border-gray-200 rounded-br-lg">
              <div className="flex items-center gap-3">
                <input
                  type="text"
                  value={newMessage}
                  onChange={(e) => setNewMessage(e.target.value)}
                  onKeyPress={handleKeyPress}
                  placeholder="Bir mesaj yazın..."
                  className="flex-1 bg-gray-100 border-transparent rounded-full px-4 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
                />
                <button
                  onClick={sendMessage}
                  className="bg-blue-600 text-white rounded-full p-3 hover:bg-blue-700 transition-colors duration-200 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500"
                >
                  <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth={1.5} stroke="currentColor" className="w-6 h-6">
                    <path strokeLinecap="round" strokeLinejoin="round" d="M6 12L3.269 3.126A59.768 59.768 0 0121.485 12 59.77 59.77 0 013.27 20.876L5.999 12zm0 0h7.5" />
                  </svg>
                </button>
              </div>
            </div>
          </>
        ) : (
          <div className="flex-1 items-center justify-center text-gray-500 text-lg hidden md:flex">
            Başlamak için bir sohbet seçin.
          </div>
        )}
      </div>
    </div>
  );
};

export default ChatApp;