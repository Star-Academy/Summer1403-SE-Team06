using System.Text.Json;

public class Program {
    private const int TopStudentsCount = 3;
    private const string StudentsFilePath = @"data\students.json";
    private const string ScoresFilePath = @"data\scores.json";
    
    public static void Main()
    {
        var students = JsonSerializer.Deserialize<List<Student>>(File.ReadAllText(StudentsFilePath));
        var studentScores = JsonSerializer.Deserialize<List<StudentScore>>(File.ReadAllText(ScoresFilePath));

        var studentsMap = new Dictionary<int, Student>();
        students.ForEach(student => studentsMap.Add(student.StudentNumber, student));

        var topStudents = GetTopStudents(studentScores);
        
        for (var i = 0; i < topStudents.Count; i++)
        {
            var currentStudent = topStudents[i];
            Student studentInfo = studentsMap[currentStudent.StudentNumber];
            Console.WriteLine($"{i + 1}.{studentInfo.FirstName} {studentInfo.LastName} : {currentStudent.Gpa:F2}");
        }
    }

    private static List<GpaInfo> GetTopStudents(List<StudentScore> studentScores)
    {
        var topStudents = studentScores.GroupBy(studentScore => studentScore.StudentNumber)
            .Select(group => new GpaInfo(
                group.Key,
                group.Average(studentScore => studentScore.Score)
            )).OrderByDescending(group => group.Gpa).
            Take(TopStudentsCount).
            ToList();
        return topStudents;
    }

    private record GpaInfo(int StudentNumber, double Gpa);
}

public class Student {
    public int StudentNumber {get; set;}
    public string FirstName {get; set;}
    public string LastName {get; set;}
}

public class StudentScore {
    public int StudentNumber {get; set;}
    public string Lesson {get; set;}
    public double Score {get; set;}
}