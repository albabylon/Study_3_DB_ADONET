using Microsoft.Data.SqlClient;
using System.Data;

namespace AdoNetLib
{
    public class DbExecutor
    {
        private MainConnector connector;
        
        public DbExecutor(MainConnector connector)
        {
            this.connector = connector;
        }

        public DataTable SelectAll(string table)
        {
            var selectcommandtext = "select * from " + table;

            var adapter = new SqlDataAdapter(selectcommandtext, connector.GetConnection());

            //Представляет хранилище или кэш данных в памяти, извлеченных из источника данных
            //Объект DataSet содержит таблицы, которые представлены типом DataTable.
            //Таблица DataTable, в свою очередь, состоит из столбцов и строк.
            //Каждый столбец представляет объект DataColumn, а строка — объект DataRow.
            var ds = new DataSet();

            //Метод Fill() неявно открывает объект подключения,
            //который хранится в свойстве Connection объекта DataAdapter, если это подключение не открыто
            //Если метод Fill() открывает подключение, то после получения данных метод Fill закрывает это подключение.
            adapter.Fill(ds);

            //На данный момент у нас не может быть много результатов, поэтому мы будем обращаться к первой из этих таблиц
            return ds.Tables[0];
        }
    }
}
