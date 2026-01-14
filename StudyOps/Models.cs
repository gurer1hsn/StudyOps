using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace StudyOps
{
    [DataContract]
    public class Question
    {
        [DataMember(Order = 1)] public string Id { get; set; }
        [DataMember(Order = 2)] public string Subject { get; set; }   // Konu
        [DataMember(Order = 3)] public string Text { get; set; }      // Soru metni

        [DataMember(Order = 4)] public string A { get; set; }
        [DataMember(Order = 5)] public string B { get; set; }
        [DataMember(Order = 6)] public string C { get; set; }
        [DataMember(Order = 7)] public string D { get; set; }

        [DataMember(Order = 8)] public string Correct { get; set; }    // "A/B/C/D"
        [DataMember(Order = 9)] public string Difficulty { get; set; } // "Kolay/Orta/Zor"
    }

    [DataContract]
    public class Exam
    {
        [DataMember(Order = 1)] public string Id { get; set; }
        [DataMember(Order = 2)] public string Title { get; set; }      // Deneme adı
        [DataMember(Order = 3)] public string Subject { get; set; }    // Konu (filtre)
        [DataMember(Order = 4)] public List<string> QuestionIds { get; set; } = new List<string>();
        [DataMember(Order = 5)] public DateTime CreatedAt { get; set; } // <-- DateTime olmalı
    }

    [DataContract]
    public class ExamResult
    {
        [DataMember(Order = 1)] public string Id { get; set; }
        [DataMember(Order = 2)] public string ExamId { get; set; }
        [DataMember(Order = 3)] public DateTime TakenAt { get; set; }

        [DataMember(Order = 4)] public int Total { get; set; }
        [DataMember(Order = 5)] public int Correct { get; set; } // <-- CorrectCount değil, Correct

        [DataMember(Order = 6)] public int Score { get; set; }   // 0-100
    }
}
