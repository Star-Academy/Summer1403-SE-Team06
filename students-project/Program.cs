using System.Text.Json;

namespace Mohaymen.StudentProject;
public class Program {
    private const int TopStudentsCount = 3;
    private const string StudentsFilePath = @"data\students.json";
    private const string ScoresFilePath = @"data\scores.json";
    
    public static void Main()
    {
        List<Student> students = new();
        List<StudentScore> studentScores = new();

        try
        {
            students = JsonSerializer.Deserialize<List<Student>>(File.ReadAllText(StudentsFilePath));
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception.Message);
            return;
        }
        
        try
        {
            studentScores = JsonSerializer.Deserialize<List<StudentScore>>(File.ReadAllText(ScoresFilePath));
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception.Message);
            return;
        }


        Dictionary<int, Student> studentsMap = new();
        students.ForEach(student => studentsMap.Add(student.StudentNumber, student));

        List<GpaInformation> topStudents = GetTopStudents(studentScores);

        foreach(GpaInformation topStudent in topStudents)
        {
            Student studentInformation = studentsMap[topStudent.StudentNumber];
            Console.WriteLine($"{studentInformation.FirstName} {studentInformation.LastName} : {topStudent.Gpa:F2}");
        }
    }

    private record GpaInformation(int StudentNumber, double Gpa);
    
    private static List<GpaInformation> GetTopStudents(List<StudentScore> studentScores)
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