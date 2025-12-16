type Props = {
  text: string;
  onClick?: () => void;
};

export default function Button({ text, onClick }: Props) {
  return (
    <button
      onClick={onClick}
      className="bg-primary text-white px-4 py-2 rounded-md hover:opacity-90"
    >
      {text}
    </button>
  );
}
