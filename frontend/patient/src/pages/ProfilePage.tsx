export default function ProfilePage() {
  return (
    <div className="min-h-[calc(100vh-80px)] bg-gray-50 px-4 py-10">
      <div className="max-w-3xl mx-auto bg-white rounded-2xl shadow p-6">
        <h1 className="text-xl font-semibold text-gray-900 mb-6">
          Profil Bilgileri
        </h1>

        <div className="flex items-center gap-6 mb-8">
          <div className="w-24 h-24 rounded-full bg-secondary flex items-center justify-center text-white text-2xl">
            T
          </div>

          <button className="px-4 py-2 rounded-lg border text-sm">
            Fotoğraf Değiştir
          </button>
        </div>

        <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
          <div>
            <label className="text-sm text-gray-600">Ad Soyad</label>
            <input
              disabled
              value="Tarık Akgün"
              className="mt-1 w-full rounded-lg border px-3 py-2 bg-gray-100"
            />
          </div>

          <div>
            <label className="text-sm text-gray-600">TC Kimlik No</label>
            <input
              disabled
              value="***********"
              className="mt-1 w-full rounded-lg border px-3 py-2 bg-gray-100"
            />
          </div>

          <div>
            <label className="text-sm text-gray-600">Telefon</label>
            <input
              disabled
              value="05** *** ** **"
              className="mt-1 w-full rounded-lg border px-3 py-2 bg-gray-100"
            />
          </div>

          <div>
            <label className="text-sm text-gray-600">E-posta</label>
            <input
              disabled
              value="tarik@example.com"
              className="mt-1 w-full rounded-lg border px-3 py-2 bg-gray-100"
            />
          </div>
        </div>

        {/* Actions */}
        <div className="mt-8 flex justify-end gap-3">
          <button className="px-5 py-2 rounded-lg border">
            Vazgeç
          </button>
          <button className="px-5 py-2 rounded-lg bg-primary text-white">
            Kaydet
          </button>
        </div>
      </div>
    </div>
  );
}
