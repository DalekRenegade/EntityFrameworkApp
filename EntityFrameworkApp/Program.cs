using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var db = args.Length == 1 ? null : new UniversityModelFirstDbContext();
            var flag = true;
            do
            {
                Console.Write("Options: (L)ist  (A)dd  (M)odify  (D)elete  (Q)uit? ");
                var input = Console.ReadLine();
                switch (input)
                {
                    case "L":
                    case "l":
                        var students = ListStudents(db);
                        foreach (var student in students)
                        {
                            Console.WriteLine(student);
                        }
                        break;
                    case "A":
                    case "a":
                        Console.Write("Enter first name: ");
                        var firstName = Console.ReadLine();
                        Console.Write("Enter last name: ");
                        var lastName = Console.ReadLine();
                        AddStudent(firstName, lastName, DateTime.Now, db);
                        break;
                    case "M":
                    case "m":
                        Console.Write("Enter existing first name: ");
                        var filterFirstName = Console.ReadLine();
                        Console.Write("Enter new first name: ");
                        var newFirstName = Console.ReadLine();
                        Console.Write("Enter new last name: ");
                        var newLastName = Console.ReadLine();
                        ModifyStudent(filterFirstName, newFirstName, newLastName, db);
                        break;
                    case "D":
                    case "d":
                        Console.Write("Enter existing first name: ");
                        var deleteFirstName = Console.ReadLine();
                        DeleteStudent(deleteFirstName, db);
                        break;
                    default:
                        flag = false;
                        break;
                }
            } while (flag);
        }

        private static IEnumerable<string> ListStudents(UniversityModelFirstDbContext customDbContext = null)
        {
            var students = new LinkedList<string>();
            UniversityModelFirstDbContext db = null;
            try
            {
                db = customDbContext ?? new UniversityModelFirstDbContext();
                var query = from b in db.Students orderby b.ID select b;
                foreach (var item in query)
                {
                    students.AddLast($"{item.ID} - {item.FirstName} {item.LastName}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (customDbContext == null)
                {
                    db?.Dispose();
                }
            }
            return students;
        }

        private static void AddStudent(string firstName, string lastName, DateTime enrollmentDate, UniversityModelFirstDbContext customDbContext =null)
        {
            UniversityModelFirstDbContext db = null;
            try
            {
                db = customDbContext ?? new UniversityModelFirstDbContext();
                var student = new Student
                {
                    LastName = lastName,
                    FirstName = firstName,
                    EnrollmentDate = enrollmentDate
                };
                db.Students.Add(student);
                db.SaveChanges();
                Console.WriteLine("Added...");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (customDbContext == null)
                {
                    db?.Dispose();
                }
            }
        }

        private static void ModifyStudent(string filterFirstName, string newFirstName = null, string newLastName = null, UniversityModelFirstDbContext customDbContext = null)
        {
            UniversityModelFirstDbContext db = null;
            try
            {
                db = customDbContext ?? new UniversityModelFirstDbContext();
                var student = (from d in db.Students where d.FirstName == filterFirstName select d).Single();
                student.LastName = string.IsNullOrEmpty(newLastName) ? student.LastName : newLastName;
                student.FirstName = string.IsNullOrEmpty(newFirstName) ? student.FirstName : newFirstName;
                db.SaveChanges();
                Console.WriteLine("Modified...");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (customDbContext == null)
                {
                    db?.Dispose();
                }
            }
        }

        private static void DeleteStudent(string filterFirstName, UniversityModelFirstDbContext customDbContext = null)
        {
            UniversityModelFirstDbContext db = null;
            try
            {
                db = customDbContext ?? new UniversityModelFirstDbContext();
                var student = (from d in db.Students where d.FirstName == filterFirstName select d).Single();
                db.Students.Remove(student);
                db.SaveChanges();
                Console.WriteLine("Deleted...");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (customDbContext == null)
                {
                    db?.Dispose();
                }
            }
        }

    }
}
