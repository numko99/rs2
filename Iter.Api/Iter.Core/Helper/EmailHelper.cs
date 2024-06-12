namespace Iter.Core.Helper
{
    public static class EmailHelper
    {
        public const string EMAIL_VERIFICATION = "EMAIL VERIFICATION";
        public const string FORGOT_PASSWORD = "FORGOT PASSWORD";

        public static string UserCreatedTitle = "Korisnički račun platforme ITer";
        public static string UserCreatedContent = "<p>Obavještavamo Vas da je korisnički račun za pristup ITer platformi kreiran. Kako bismo vam omogućili korištenje naše platforme, u nastavku su navedeni vaši pristupni podaci:</p>" +
                   "<p style='margin-bottom:5px'>Email: <b>admir.numanovic@hotmail.com</b></p>" +
                   "<p style='margin-top:0px'>Lozinka: <b>test1234</b></p>" +
                   "<p>Iz sigurnosnih razloga, molimo Vas da prilikom prve prijave promijenite ovu privremenu lozinku. To možete učiniti prateći upute u sekciji \"Postavke računa\" nakon što se prijavite.</p>";
    }
}
