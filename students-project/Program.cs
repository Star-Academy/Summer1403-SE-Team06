using System.Text.Json;

namespace Mohaymen.StudentProject;
public class Program {
    private const int TopStudentsCount = 3;
    private const string StudentsFilePath = @"data\students.json";
    private const string ScoresFilePath = @"data\scores.json";
    
    public static void Main()
    {
        var students = new List<Student>();
        var studentScores = new List<StudentScore>();

        try
        {
            students = JsonSerializer.Deserialize<List<Student>>(File.ReadAllText(StudentsFilePath));
            studentScores = JsonSerializer.Deserialize<List<StudentScore>>(File.ReadAllText(ScoresFilePath));
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception.Message);
            return;
        }
        
        Dictionary<int, Student> studentsMap = new();
        students.ForEach(student => studentsMap.Add(student.StudentNumber, student));

        List<GpaInformation> topStudentsGpa = GetTopStudentsGpa(studentScores);

        foreach(var topStudentGpa in topStudentsGpa)
        {
            var student = studentsMap[topStudentGpa.StudentNumber];
            Console.WriteLine($"{student.FirstName} {student.LastName} : {topStudentGpa.Gpa:F2}");
        }
    }

    private record GpaInformation(int StudentNumber, double Gpa);
    
    private static List<GpaInformation> GetTopStudentsGpa(List<StudentScore> studentScores)
    {
        var topStudents = studentScores.GroupBy(studentScore => studentScore.StudentNumber)
            .Select(group => new GpaInformation(
                group.Key,
                group.Average(studentScore => studentScore.Score)
            )).OrderByDescending(group => group.Gpa).
            Take(TopStudentsCount).
            ToList();
        return topStudents;
    }
}