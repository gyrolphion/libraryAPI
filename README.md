# 📚 LibraryBox API

Bu API, Letterboxd mantığıyla geliştirilen bir kitap uygulamasının sunucu tarafını oluşturur. Kullanıcılar sisteme kayıt olabilir, giriş yapabilir, kitap arayabilir, kitaplara yorum yapabilir ve puan verebilir. Tüm işlemler kullanıcı doğrulaması (Token) ile yapılmaktadır.

## Genel Özellikler

- **Kayıt ve Giriş**: Kullanıcılar sisteme kullanıcı adı, şifre ve e-posta ile kayıt olabilir. Aynı kullanıcı adı veya e-posta sistemde varsa kayıt işlemi reddedilir. Giriş yapan kullanıcıya token verilir.
  
- **Token Sistemi**: Giriş sonrası oluşturulan token, sonraki tüm kullanıcı işlemleri için kimlik doğrulama amacıyla kullanılır.

- **Kitap Listeleme ve Arama**: Uygulama kitap listesini getirir. Kullanıcılar kitap adına ya da yazar adına göre arama yapabilir.

- **Yorum Ekleme ve Güncelleme**: Kullanıcılar her kitap için sadece bir kez yorum yapabilir. Yorum yaptıktan sonra aynı kitaba tekrar yorum yapamazlar. Ancak isterlerse mevcut yorumlarını ve puanlarını güncelleyebilirler.

- **Puan Verme**: Kullanıcılar kitaplara puan verebilir. Her kullanıcı her kitaba sadece bir kez puan verebilir. Puanlar ayrı bir tabloda tutulur.

- **Veritabanı İlişkileri**: Kullanıcılar, yorumlar, puanlar ve tokenlar birbiriyle ilişkilidir. Örneğin bir kullanıcı silinmek istenirse önce ona bağlı yorum ve token gibi kayıtlar silinmelidir.

## Kullanılan Teknolojiler

- ASP.NET Core Web API
- SQL Server
- RESTful servis mimarisi
- Özel token üretimi

