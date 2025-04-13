﻿using AdoNetLib;
using Microsoft.Data.SqlClient;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdoNetConsole
{
    static class Program
    {
        static void Main(string[] args)
        {
            var connector = new MainConnector();

            var result = connector.ConnectAsync();

            var data = new DataTable();

            if (result.Result)
            {
                Console.WriteLine("Подключено успешно!");

                var db = new DbExecutor(connector);

                var tablename = "NetworkUser";
                Console.WriteLine("Получаем данные таблицы " + tablename);

                #region отсоединенная модель
                //data = db.SelectAll(tablename);

                //Console.WriteLine("Отключаем БД!");
                //connector.DisconnectAsync();

                ////неважно где запрашивать (до отключения от БД или после), так как все записалось уже в DataTable
                //Console.WriteLine("Количество строк в " + tablename + ": " + data.Rows.Count);

                ////При работе с DataSet DataTable
                ////Все данные при этом будут храниться в оперативной памяти вашего ПК, потому, если речь о работе с миллионами строк,
                ////то это может здорово влиять на производительность. 

                ////Если мы изменим БД, а потом заново подключимся, то переменная DataTable все равно будет как и была (так как мы ее не перезаписывали)

                ////Данные можно вывести как данные из обычного массива
                ////ИЛИ
                ////Используя сущность item (это пространство всех «ячеек» таблицы)

                ////Выведем на экран данные по наименованиям колонок
                //foreach (DataColumn column in data.Columns)
                //{
                //    Console.Write($"{column.ColumnName}\t");
                //}

                //Console.WriteLine();

                ////Обратимся к коллекции строк и выведем все данные в этой строке
                //foreach (DataRow row in data.Rows)
                //{
                //    var cells = row.ItemArray;
                //    foreach (var cell in cells)
                //    {
                //        Console.Write($"{cell}\t");
                //    }
                //    Console.WriteLine();

                //    //К элементам строки также можно обращаться по имени столбца, то есть использовать его как индекс
                //    //Console.Write($"{row[data.Columns[0].ColumnName]}\t");
                //}
                #endregion

                #region присоединенная модель
                var reader = db.SelectAllCommandReader(tablename);

                //при работе с SqlDataReader мы будем обращаться к данным по имени столбца
                
                //сформируем себе список столбцов и запишем его в массив
                var columnList = new List<string>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    var name = reader.GetName(i);
                    columnList.Add(name);
                }

                //по именам мы будем использовать reader, чтобы отображать значения каждого столбца в строке.
                //отображаем имена столбцов
                for (int i = 0; i < columnList.Count; i++)
                {
                    Console.Write($"{columnList[i]}\t");
                }
                Console.WriteLine();

                //приступим к чтению данных
                //метод reader.Read() возвращает значение true, пока reader не достиг своего конца
                while (reader.Read())
                {
                    for (int i = 0; i < columnList.Count; i++)
                    {
                        var value = reader[columnList[i]];
                        Console.Write($"{value}\t");
                    }

                    Console.WriteLine();
                }

                //если в момент запуска программы изменить БД, то так как reader запущен изменения не увидим
                //Несмотря на то, что reader читает ваш запрос построчно, запрос уже выполнен.
                //В ходе работы читается результат запроса, а не выполняется сам запрос.
                
                //Нужно делать механизм, который обновляет данные после изменения,
                //либо обновляет их с какой-то периодичностью, то есть перевыполняет ваш запрос.
                #endregion
            }
            else
            {
                Console.WriteLine("Ошибка подключения!");
            }

            Console.ReadKey();

        }
    }
}
