namespace AdoNetLib.Configurations
{
    public static class ConnectionString
    {
        //Для хранения строк подключения статический класс — один из удобных вариантов,
        //так как вам не требуется потом искать все места в коде, где вы могли использовать строку подключения, если вам потребуется поменять что-то.
        //Также удобным бывает хранение в XML-файлах конфигурации
        public static string MsSqlConnection => @"Server=.\SQLEXPRESS01;Database=testing;Trusted_Connection=True;TrustServerCertificate=True;";
    }
}
