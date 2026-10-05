# Desain Antarmuka LabRepair

## Untuk siapa
Teknisi dan kepala lab yang membuka aplikasi di sela pekerjaan, sering dari PC lab itu sendiri.
Pertanyaan utama mereka: **"laporan mana yang harus saya kerjakan sekarang?"**

## Keputusan
- **Warna:** putih sebagai dasar, hijau tua `#1f6b45` sebagai satu-satunya warna aksi
  (tombol utama, link, tab aktif). Hijau dipilih karena dekat dengan makna "beres/berfungsi".
  Itu kondisi yang dituju setiap laporan.
- **Warna status** hanya dipakai untuk menandai keadaan nyata:
  kuning = belum disentuh, hijau muda = sedang dikerjakan, hijau tua = selesai, abu = tidak bisa diperbaiki.
  Merah hanya untuk prioritas tinggi.
- **Huruf:** Public Sans. Dirancang untuk aplikasi layanan publik/institusi: polos, angka mudah dibaca
  di tabel. Jika tidak ada internet, jatuh ke font sistem.
- **Permukaan:** kartu rata dengan garis tipis, tanpa bayangan. Tidak ada elemen yang "melayang"
  karena tidak ada lapisan yang perlu diangkat.
- **Dashboard** bukan deretan kartu angka: isi utamanya antrean laporan terbuka, diurutkan dari
  prioritas tertinggi dan yang paling lama menunggu. Angka ringkasan cukup satu baris kalimat.
- **Tanpa emoji, ikon dekoratif, gradien, atau animasi berulang.**
- **Keadaan kosong** selalu menyebut sebabnya dan langkah berikutnya.
