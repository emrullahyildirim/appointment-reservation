namespace AuthService.Business.Constants
{
    public static class Messages
    {
        // Success Messages
        public const string UserRegistered = "Kullanıcı başarıyla kaydedildi.";
        public const string LoginSuccessful = "Giriş başarılı.";
        public const string LogoutSuccessful = "Çıkış başarılı.";
        public const string TokenCreated = "Token oluşturuldu.";
        public const string TokenRefreshed = "Token yenilendi.";
        public const string TokenRevoked = "Token iptal edildi.";
        public const string PasswordResetEmailSent = "Şifre sıfırlama bağlantısı email adresinize gönderildi.";
        public const string PasswordResetSuccess = "Şifreniz başarıyla değiştirildi.";
        public const string PasswordChanged = "Şifreniz başarıyla güncellendi.";
        public const string EmailVerified = "Email adresiniz doğrulandı.";
        public const string VerificationEmailSent = "Doğrulama emaili gönderildi.";
        public const string UserUpdated = "Kullanıcı güncellendi.";
        public const string UserDeleted = "Kullanıcı silindi.";
        public const string RoleAdded = "Rol eklendi.";
        public const string RoleRemoved = "Rol kaldırıldı.";

        // Error Messages
        public const string UserNotFound = "Kullanıcı bulunamadı.";
        public const string UserAlreadyExists = "Bu email adresi zaten kayıtlı.";
        public const string PasswordError = "Şifre hatalı.";
        public const string InvalidCredentials = "Email veya şifre hatalı.";
        public const string AccountLocked = "Hesabınız geçici olarak kilitlendi. Lütfen daha sonra tekrar deneyin.";
        public const string AccountDisabled = "Hesabınız devre dışı bırakılmıştır.";
        public const string EmailNotVerified = "Email adresiniz doğrulanmamış. Lütfen emailinizi kontrol edin.";
        public const string InvalidRefreshToken = "Geçersiz refresh token.";
        public const string RefreshTokenExpired = "Refresh token süresi dolmuş.";
        public const string InvalidPasswordResetToken = "Geçersiz veya süresi dolmuş şifre sıfırlama bağlantısı.";
        public const string InvalidEmailVerificationToken = "Geçersiz veya süresi dolmuş doğrulama bağlantısı.";
        public const string EmailAlreadyVerified = "Email adresi zaten doğrulanmış.";
        public const string PasswordsDoNotMatch = "Şifreler eşleşmiyor.";
        public const string CurrentPasswordIncorrect = "Mevcut şifre hatalı.";
        public const string RoleNotFound = "Rol bulunamadı.";
        public const string UserAlreadyHasRole = "Kullanıcı zaten bu role sahip.";
        public const string UserDoesNotHaveRole = "Kullanıcı bu role sahip değil.";

        // S2S (Service-to-Service) Messages
        public const string S2STokenCreated = "S2S token başarıyla oluşturuldu.";
        public const string InvalidClientCredentials = "Geçersiz client credentials.";
        public const string ServiceClientDeactivated = "Servis client deaktif edilmiş.";
        public const string InvalidScope = "İstenen scope'lara erişim izniniz yok.";
        public const string InsufficientScope = "Token gerekli scope'a sahip değil.";
        public const string TokenExpired = "Token süresi dolmuş.";
        public const string InvalidToken = "Geçersiz token.";
        public const string ServiceClientCreated = "Servis client başarıyla oluşturuldu.";
        public const string ServiceClientUpdated = "Servis client başarıyla güncellendi.";
        public const string ServiceClientNotFound = "Servis client bulunamadı.";
        public const string ServiceClientAlreadyExists = "Bu isimde bir servis client zaten mevcut.";
        public const string ClientSecretRegenerated = "Client secret başarıyla yenilendi.";
    }
}

