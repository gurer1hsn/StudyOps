using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace StudyOps
{
    public static class Storage
    {
        private static readonly string DataDir =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data");

        private static readonly string QuestionsFile = Path.Combine(DataDir, "questions.json");
        private static readonly string ExamsFile = Path.Combine(DataDir, "exams.json");
        private static readonly string ResultsFile = Path.Combine(DataDir, "results.json");

        static Storage()
        {
            if (!Directory.Exists(DataDir))
                Directory.CreateDirectory(DataDir);
        }

        // -------------------- GENERIC JSON HELPERS --------------------
        private static List<T> ReadList<T>(string path)
        {
            try
            {
                if (!File.Exists(path)) return new List<T>();

                var json = File.ReadAllText(path, Encoding.UTF8);
                if (string.IsNullOrWhiteSpace(json)) return new List<T>();

                using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
                {
                    var ser = new DataContractJsonSerializer(typeof(List<T>));
                    return (List<T>)ser.ReadObject(ms);
                }
            }
            catch
            {
                return new List<T>();
            }
        }

        private static void WriteList<T>(string path, List<T> list)
        {
            if (list == null) list = new List<T>();

            using (var ms = new MemoryStream())
            {
                var ser = new DataContractJsonSerializer(typeof(List<T>));
                ser.WriteObject(ms, list);

                var json = Encoding.UTF8.GetString(ms.ToArray());
                File.WriteAllText(path, json, Encoding.UTF8);
            }
        }

        // -------------------- QUESTIONS --------------------
        public static List<Question> LoadQuestions()
            => ReadList<Question>(QuestionsFile);

        public static void SaveQuestions(List<Question> questions)
            => WriteList(QuestionsFile, questions);

        // -------------------- EXAMS --------------------
        public static List<Exam> LoadExams()
            => ReadList<Exam>(ExamsFile);

        public static void SaveExams(List<Exam> exams)
            => WriteList(ExamsFile, exams);

        // -------------------- RESULTS --------------------
        public static List<ExamResult> LoadResults()
            => ReadList<ExamResult>(ResultsFile);

        public static void SaveResults(List<ExamResult> results)
            => WriteList(ResultsFile, results);
    }
}
