using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;

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

        //Через присоединенную модель
        //SqlDataReader отвечает за построчное считывание результата, который пришел к нам из запроса,
        //получаем не данные сразу, а лишь способ их прочитать
        public SqlDataReader? SelectAllCommandReader(string table)
        {
            var command = new SqlCommand
            {
                CommandType = CommandType.Text,
                CommandText = "select * from " + table,
                Connection = connector.GetConnection(),
            };

            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                return reader;
            }

            return null;
        }

        //Метод удаляет строку по имени колонке и значению
        //Возвращает количество обработанных строк
        public int DeleteByColumn(string table, string column, string value)
        {
            var command = new SqlCommand
            {
                CommandType = CommandType.Text,
                CommandText = "delete from " + table + " where " + column + " = '" + value + "';",
                Connection = connector.GetConnection(),
            };

            return command.ExecuteNonQuery();
        }

        //Через хранимую процедуру и передачу ей параметра
        public int ExecProcedureAdding(string name, string login)
        {
            var command = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "AddingUserProc",
                Connection = connector.GetConnection(),
            };

            //указываем параметры для процедуры, для это используется объект SqlParameter
            //испольузется для передачи в процедуру или можем передавать в качестве параметров данные в любой запрос, то есть параметризировать его
            command.Parameters.Add(new SqlParameter("@Name", name));
            command.Parameters.Add(new SqlParameter("@Login", login));

            //на данном этапе возможно передать что угодно, в том числе и ошибочные данные с символами,
            //которые будут разделять строку, что вызовет в БД ошибки в данных

            //злоумышленник может передать команду, которая уничтожит нашу базу данных или таблицу

            //потому для работы с прямыми запросами SQL желательно использовать либо процедуры с параметрами, либо параметризованные запросы.
            //Объект SqlParameter экранирует данные автоматически.

            return command.ExecuteNonQuery();
        }

        public int UpdateByColumn(string table, string columntocheck, string valuecheck, string columntoupdate, string valueupdate)
        {
            var command = new SqlCommand
            {
                CommandType = CommandType.Text,
                CommandText = $"update {table} set {columntoupdate} = '{valueupdate}' where {columntocheck} = '{valuecheck}'",
                Connection = connector.GetConnection(),
            };

            return command.ExecuteNonQuery();
        }
    }
}
