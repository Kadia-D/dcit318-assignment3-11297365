using System;
using System.Collections.Generic;
using System.IO;

namespace GradingSystem
{
    // --- Student Class ---
    public class Student
    {
        public int Id { get; }
        public string FullName { get; }
        public int Score { get; }

        public Student(int id, string fullName, int score)
        {
            Id = id;
            FullName = fullName;
            Score = score;
        }

        public string GetGrade()
        {
            return Score switch
            {
                >= 80 and <= 100 => "A",
                >= 70 and <= 79 => "B",
                >= 60 and <= 69 => "C",
                >= 50 and <= 59 => "D",
                _ => "F"
            };
        }
    }

    // --- Custom Exceptions ---
    public class InvalidScoreFormatException : Exception
    {
        public InvalidScoreFormatException(string message) : base(message) { }
    }

    public class MissingFieldException : Exception
    {
        public MissingFieldException(string message) : base(message) { }
    }

    // --- StudentResultProcessor Class ---
    public class StudentResultProcessor
    {
        public List<Student> ReadStudentsFromFile(string inputFilePath)
        {
            var students = new List<Student>();

            using (StreamReader reader = new StreamReader(inputFilePath))
            {
                string line;
                int lineNumber = 1;

                while ((line = reader.ReadLine()) != null)
                {
                    var parts = line.Split(',');

                    if (parts.Length != 3)
                        throw new MissingFieldException($"Line {lineNumber}: Expected 3 fields but got {parts.Length}");

                    if (!int.TryParse(parts[0].Trim(), out int id))
                        throw new FormatException($"Line {lineNumber}: Invalid ID format");

                    string name = parts[1].Trim();

                    if (!int.TryParse(parts[2].Trim(), out int score))
                        throw new InvalidScoreFormatException($"Line {lineNumber}: Score '{parts[2]}' is not a valid number");

                    students.Add(new Student(id, name, score));
                    lineNumber++;
                }
            }

            return students;
        }

        public void WriteReportToFile(List<Student> students, string outputFilePath)
        {
            using (StreamWriter writer = new StreamWriter(outputFilePath))
            {
                foreach (var student in students)
                {
                    string summary = $"{student.FullName} (ID: {student.Id}): Score = {student.Score}, Grade = {student.GetGrade()}";
                    writer.WriteLine(summary);
                }
            }
        }
    }

    // --- Main Program ---
    class Program
    {
        static void Main()
        {
            string inputPath = "students.txt";  // Ensure this file exists in the executable directory
            string outputPath = "report.txt";

            try
            {
                var processor = new StudentResultProcessor();
                List<Student> students = processor.ReadStudentsFromFile(inputPath);
                processor.WriteReportToFile(students, outputPath);
                Console.WriteLine(" Report successfully generated in 'report.txt'");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine(" Error: Input file not found.");
            }
            catch (InvalidScoreFormatException ex)
            {
                Console.WriteLine(" Error: " + ex.Message);
            }
            catch (MissingFieldException ex)
            {
                Console.WriteLine(" Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(" Unexpected error: " + ex.Message);
            }
        }
    }
}
