export default function Profile() {
  return (
    <div className="p-6 max-w-4xl mx-auto">
      <h1 className="text-3xl font-bold text-gray-800 mb-8">Profil Bilgileri</h1>

      <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
        <div className="lg:col-span-1">
          <div className="bg-white rounded-2xl shadow-lg p-6 flex flex-col items-center">
            <div className="w-32 h-32 rounded-full bg-gray-300 flex items-center justify-center mb-6">
              <span className="text-5xl text-white">T</span>
            </div>
            <h2 className="text-xl font-bold text-gray-800">Dr. Tarık Akgün</h2>
            <p className="text-gray-500">Kardiyolog</p>
            <button className="mt-6 w-full px-4 py-2 rounded-lg bg-blue-600 text-white font-semibold hover:bg-blue-700 transition-all">
              Fotoğraf Yükle
            </button>
          </div>
        </div>
        <div className="lg:col-span-2">
          <div className="bg-white rounded-2xl shadow-lg p-8">
            <div className="grid grid-cols-1 sm:grid-cols-2 gap-6">
              <div>
                <label className="text-sm font-medium text-gray-600">Ad Soyad</label>
                <input
                  disabled
                  value="Tarık Akgün"
                  className="mt-2 w-full rounded-lg border px-4 py-2 bg-gray-100 text-gray-800"
                />
              </div>

              <div>
                <label className="text-sm font-medium text-gray-600">TC Kimlik No</label>
                <input
                  disabled
                  value="***********"
                  className="mt-2 w-full rounded-lg border px-4 py-2 bg-gray-100"
                />
              </div>

              <div>
                <label className="text-sm font-medium text-gray-600">Telefon</label>
                <input
                  disabled
                  value="05** *** ** **"
                  className="mt-2 w-full rounded-lg border px-4 py-2 bg-gray-100"
                />
              </div>

              <div>
                <label className="text-sm font-medium text-gray-600">E-posta</label>
                <input
                  disabled
                  value="tarik@example.com"
                  className="mt-2 w-full rounded-lg border px-4 py-2 bg-gray-100"
                />
              </div>
              
              <div className="sm:col-span-2">
                <label className="text-sm font-medium text-gray-600">Uzmanlık Alanı</label>
                <input
                  disabled
                  value="Kardiyoloji"
                  className="mt-2 w-full rounded-lg border px-4 py-2 bg-gray-100"
                />
              </div>

              <div className="sm:col-span-2">
                <label className="text-sm font-medium text-gray-600">Hakkında</label>
                <textarea
                  disabled
                  rows={4}
                  value="Dr. Akgün, 10 yılı aşkın deneyime sahip bir kardiyoloji uzmanıdır. Özellikle kalp ritim bozuklukları ve koroner arter hastalıkları üzerine yoğunlaşmıştır."
                  className="mt-2 w-full rounded-lg border px-4 py-2 bg-gray-100"
                />
              </div>
            </div>

            <div className="mt-10 flex justify-end gap-4">
              <button className="px-6 py-2 rounded-lg border font-semibold text-gray-700 hover:bg-gray-100 transition-all">
                Vazgeç
              </button>
              <button className="px-6 py-2 rounded-lg bg-green-600 text-white font-semibold hover:bg-green-700 transition-all">
                Değişiklikleri Kaydet
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}