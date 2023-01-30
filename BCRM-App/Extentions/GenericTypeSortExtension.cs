using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Linq.Expressions;
using System.Threading;
using BCRM_App.Models.General;

namespace BCRM_App.Extentions
{
    public static partial class GenericTypeSortExtension
    {
        /// <summary>
        /// this extension method will sorting data with generic data type and it can sorting by multiple* sort 
        /// </summary>
        /// <typeparam name="T">type of data collection</typeparam>
        /// <param name="datas">data collection</param>
        /// <param name="sorts">sort data</param>
        /// <returns></returns>
        public static IQueryable<T> OrderByMultipleIQueryable<T>(this IQueryable<T> datas, List<SortModel> sorts, StringComparer comparer = default, CultureInfo cultureInfo = default)
        {
            if (cultureInfo != null) Thread.CurrentThread.CurrentCulture = cultureInfo;
            if (comparer == null) comparer = StringComparer.Ordinal;

            if (sorts == null || sorts.Count == 0) return datas;
            if (datas == null || datas.Count() == 0) return datas;

            if (sorts.Count() <= 0) return datas;
            var sortFirstSeq = sorts[0];
            if (string.IsNullOrEmpty(sortFirstSeq.Key)) return datas.AsQueryable();

            // Custom lambda expression for dynamic data source
            var nextParams = Expression.Parameter(typeof(T), sortFirstSeq.Key);
            var nextSortExpresstion = Expression.Lambda<Func<T, string>>(Expression.Convert(Expression.Property(nextParams, sortFirstSeq.Key), typeof(object)), nextParams);

            if (sortFirstSeq.Value == 2)
            {
                datas = RecursiveSort<T>(datas.AsQueryable<T>().OrderByDescending(nextSortExpresstion), sorts, comparer);
            }
            else
            {
                datas = RecursiveSort<T>(datas.AsQueryable<T>().OrderBy(nextSortExpresstion), sorts, comparer);
            }

            return datas;
        }

        /// <summary>
        /// this extension method will sorting data with generic data type and it can sorting by single* sort 
        /// </summary>
        /// <typeparam name="T">type of data collection</typeparam>
        /// <param name="datas">data collection</param>
        /// <param name="sorts">sort data</param>
        /// <returns></returns>
        public static IQueryable<T> OrderBySingleIQueryable<T>(this IQueryable<T> datas, SortModel sort, StringComparer comparer = default, CultureInfo cultureInfo = default)
        {
            if (cultureInfo != null) Thread.CurrentThread.CurrentCulture = cultureInfo;
            if (comparer == null) comparer = StringComparer.Ordinal;

            var dataOrdered = datas;
            if (sort == null) return datas;

            var Params = Expression.Parameter(typeof(T), sort.Key);
            var SortExpresstion = Expression.Lambda<Func<T, object>>(Expression.Convert(Expression.Property(Params, sort.Key), typeof(object)), Params);

            switch (sort.Value)
            {
                case 2:
                    dataOrdered = dataOrdered.AsQueryable<T>().OrderByDescending(SortExpresstion as Expression<Func<T, string>>, comparer);
                    break;
                default:
                    dataOrdered = dataOrdered.AsQueryable<T>().OrderBy(SortExpresstion as Expression<Func<T, string>>, comparer);
                    break;
            }

            return dataOrdered;
        }

        /// <summary>
        /// this extension method will sorting data with generic data type and it can sorting by multiple* sort 
        /// </summary>
        /// <typeparam name="T">type of data collection</typeparam>
        /// <param name="datas">data collection</param>
        /// <param name="sorts">sort data</param>
        /// <returns></returns>
        public static IQueryable<T> OrderByMultipleIEnumerable<T>(this IEnumerable<T> datas, List<SortModel> sorts)
        {
            if (sorts == null || sorts.Count == 0) return datas.AsQueryable();
            if (datas == null || datas.Count() == 0) return datas.AsQueryable();

            if (sorts.Count() <= 0) return datas.AsQueryable();
            var sortFirstSeq = sorts[0];
            if (string.IsNullOrEmpty(sortFirstSeq.Key)) return datas.AsQueryable();

            // Custom lambda expression for dynamic data source
            var nextParams = Expression.Parameter(typeof(T), sortFirstSeq.Key);
            var nextSortExpresstion = Expression.Lambda<Func<T, object>>(Expression.Convert(Expression.Property(nextParams, sortFirstSeq.Key), typeof(object)), nextParams);

            if (sortFirstSeq.Value == 2)
            {
                datas = RecursiveSort<T>(datas.AsQueryable<T>().OrderByDescending(nextSortExpresstion), sorts);
            }
            else
            {
                datas = RecursiveSort<T>(datas.AsQueryable<T>().OrderBy(nextSortExpresstion), sorts);
            }

            return datas.AsQueryable();
        }

        public static IEnumerable<T> StringOrderByMultipleIEnumerable<T>(this IEnumerable<T> datas, List<SortModel> sorts, StringComparer comparer = default, CultureInfo cultureInfo = default)
        {
            if (cultureInfo != null) Thread.CurrentThread.CurrentCulture = cultureInfo;
            if (comparer == null) comparer = StringComparer.Ordinal;

            if (sorts == null || sorts.Count == 0) return datas.AsQueryable();
            if (datas == null || datas.Count() == 0) return datas.AsQueryable();

            if (sorts.Count() <= 0) return datas.AsQueryable();
            var sortFirstSeq = sorts[0];
            if (string.IsNullOrEmpty(sortFirstSeq.Key)) return datas.AsQueryable();

            // Custom lambda expression for dynamic data source
            var nextParams = Expression.Parameter(typeof(T), sortFirstSeq.Key);
            var nextSortExpresstion = Expression.Lambda<Func<T, string>>(Expression.Convert(Expression.Property(nextParams, sortFirstSeq.Key), typeof(string)), nextParams);

            if (sortFirstSeq.Value == 2)
            {
                datas = RecursiveSort<T>(datas.AsQueryable<T>().OrderByDescending(nextSortExpresstion), sorts, comparer);
            }
            else
            {
                datas = RecursiveSort<T>(datas.AsQueryable<T>().OrderBy(nextSortExpresstion), sorts, comparer);
            }

            return datas.AsQueryable();
        }


        /// <summary>
        /// this extension method will sorting data with generic data type and it can sorting by single* sort 
        /// </summary>
        /// <typeparam name="T">type of data collection</typeparam>
        /// <param name="datas">data collection</param>
        /// <param name="sorts">sort data</param>
        /// <returns></returns>
        public static IEnumerable<T> OrderBySingleEnumerable<T>(this IEnumerable<T> datas, SortModel sort, StringComparer comparer = default, CultureInfo cultureInfo = default)
        {
            if (cultureInfo != null) Thread.CurrentThread.CurrentCulture = cultureInfo;
            if (comparer == null) comparer = StringComparer.Ordinal;

            var dataOrdered = datas;
            if (sort == null) return datas;
            if (string.IsNullOrEmpty(sort.Key)) return datas;

            var Params = Expression.Parameter(typeof(T), sort.Key);
            var SortExpresstion = Expression.Lambda<Func<T, object>>(Expression.Convert(Expression.Property(Params, sort.Key), typeof(object)), Params);

            switch (sort.Value)
            {
                case 1:
                    dataOrdered = dataOrdered.AsQueryable<T>().OrderBy(SortExpresstion as Expression<Func<T, string>>, comparer);
                    break;
                default:
                    dataOrdered = dataOrdered.AsQueryable<T>().OrderByDescending(SortExpresstion as Expression<Func<T, string>>, comparer);
                    break;
            }

            return dataOrdered;
        }

        // Recursive function for multiple sorting
        private static IOrderedQueryable<T> RecursiveSort<T>(IOrderedQueryable<T> datas, List<SortModel> sorts, StringComparer comparer = default)
        {
            var nextSort = sorts[0];
            sorts.RemoveAt(0);
            Type t = typeof(T).GetType().GetMember(nextSort.Key).GetType();

            var nextParams = Expression.Parameter(typeof(T), nextSort.Key);
            var nextSortExpresstion = Expression.Lambda<Func<T, object>>(Expression.Convert(Expression.Property(nextParams, nextSort.Key), typeof(object)), nextParams);

            if (nextSort.Value == 2)
            {
                if (sorts.Count > 0) return RecursiveSort<T>(datas.ThenByDescending(nextSortExpresstion), sorts, comparer);
                return datas.ThenByDescending(nextSortExpresstion);
            }
            else
            {
                if (sorts.Count > 0) return RecursiveSort<T>(datas.ThenBy(nextSortExpresstion), sorts, comparer);
                return datas.ThenBy(nextSortExpresstion);
            }
        }

        public static IEnumerable<Student> ExampleMultiSort()
        {
            List<Student> students = new List<Student>();
            List<int> age = new List<int>() { 3, 5, 7 };
            List<string> rooms = new List<string>() { "A", "B", "C" };
            Random random = new Random();

            for (int i = 0; i < 20; i++)
            {
                var _index = random.Next(0, age.Count);

                Student student = new Student();
                student.Id = i;
                student.Name = $"Name {random.Next(0, 30)}";
                student.Age = age[_index];
                student.Room = rooms[random.Next(0, 2)];

                students.Add(student);

                //age.RemoveAt(_index);
            }

            return students.OrderByMultipleIEnumerable(new List<SortModel>() {
                new SortModel ()
                {
                    Seq = 0,
                    Key = "Age",
                    Value = 1
                },
                new SortModel ()
                {
                    Seq = 0,
                    Key = "Room",
                    Value = 1
                },
                new SortModel ()
                {
                    Seq = 0,
                    Key = "Name",
                    Value = 1
                },
                new SortModel ()
                {
                    Seq = 0,
                    Key = "Id",
                    Value = 1
                }
            });
        }

        public static IEnumerable<T> CustomCompare<T>(this IEnumerable<T> datas, SortModel sort, StringComparer comparer = default)
        {
            var dataOrdered = datas;

            var Params = Expression.Parameter(typeof(T), sort.Key);
            var SortExpresstion = Expression.Lambda<Func<T, string>>(Expression.Convert(Expression.Property(Params, sort.Key), typeof(object)), Params);

            switch (sort.Value)
            {
                case 1:
                    dataOrdered = dataOrdered.AsQueryable<T>().OrderBy(SortExpresstion, comparer);
                    break;
                default:
                    dataOrdered = dataOrdered.AsQueryable<T>().OrderByDescending(SortExpresstion, comparer);
                    break;
            }

            return dataOrdered;
        }

   

        public class Student
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Age { get; set; }
            public string Room { get; set; }
        }
    
    }
}
