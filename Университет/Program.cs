using System;
using System.Collections.Generic;
enum StudentStatus
{
    Active,
    Expelled,
    Graduated
}
class Student
{
    private string number;
    public string Number
    {
        get
        {
            return number;
        }
        set
        {
            if(string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Номер студента не может быть пустым", nameof(Number));
            }
            else
            {
                number = value;
            }
        }
    }
    private string name;
    public string Name
    {
        get
        {
            return name;
        }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Имя студента не может быть пустым",nameof(Name));
            }
            else
            {
                name = value;
            }
        }
    }
    private string faculty;
    public string Faculty
    {
        get
        {
            return faculty;
        }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Факультет студента не может быть пустым", nameof(Faculty));
            }
            else
            {
                faculty = value;
            }
        }
    }
    public StudentStatus Status { get; set; }
    public Dictionary<string,int> Grades { get; private set; }
    public Student(string number,string name,string faculty)
    {
        Number = number;
        Name = name;
        Faculty = faculty;
        Status = StudentStatus.Active;
        Grades = new Dictionary<string, int>();
    }
    public void AddGrade(string courseTitle, int grade)
    {
        switch (Status)
        {
            case StudentStatus.Active:


                if (Grades.ContainsKey(courseTitle))
                {
                    throw new InvalidOperationException("Оценка по этому курсу уже есть");
                }
                else
                {
                    if (grade < 2 || grade > 5)
                    {
                        throw new ArgumentOutOfRangeException(nameof(grade), "Оценка от 2 до 5");
                    }
                    else
                    {
                        Grades[courseTitle] = grade;
                        break;
                    }
                }
            case StudentStatus.Expelled:
                throw new InvalidOperationException("Студент отчислен");
            case StudentStatus.Graduated:
                throw new InvalidOperationException("Студент выпустился");
        }
    }
    public double GetAverageGrade()
    {
        if (Grades.Count == 0)
        {
            throw new DivideByZeroException("Нет оценок для расчёта");
        }
        else
        {
            double total = 0;
            foreach (var pair in Grades)
            {
                total += pair.Value;
            }
            return total / Grades.Count;
        }
    }
    public void Expel()
    {
        switch (Status)
        {
            case StudentStatus.Active:
                Status = StudentStatus.Expelled;
                break;
            case StudentStatus.Expelled:
                throw new InvalidOperationException("Студент отчислен");
            case StudentStatus.Graduated:
                throw new InvalidOperationException("Студент выпустился");
        }
    }
    public void Graduate()
    {
        switch (Status)
        {
            case StudentStatus.Active:
                Status = StudentStatus.Graduated;
                break;
            case StudentStatus.Expelled:
                throw new InvalidOperationException("Студент отчислен");
            case StudentStatus.Graduated:
                throw new InvalidOperationException("Студент выпустился");
        }
    }
}

class Course
{
    private string title;
    public string Title
    {
        get
        {
            return title;
        }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Название курса не может быть пустым", nameof(Title));
            }
            else
            {
                title = value;
            }
        }
    }

    private string teacher;

    public string Teacher
    {
        get
        {
            return teacher;
        }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Невозможно создать курс без учителя",nameof(Teacher));
            }
            else
            {
                teacher = value;
            }
        }
    }

    private int credits;

    public int Credits
    {
        get
        {
            return credits;
        }
        set
        {
            if (value <= 0 )
            {
                throw new ArgumentOutOfRangeException(nameof(Credits), "Кредиты должны быть больше 0");
            }
            else
            {
                credits = value;
            }
        }
    }

    private int maxStudents;
    public int MaxStudents
    {
        get
        {
            return maxStudents;
        }
        set
        {
            if (value <= 0 || value >200)
            {
                throw new ArgumentOutOfRangeException(nameof(MaxStudents), "Количество студентов от 1 до 200");
            }
            else
            {
                maxStudents = value;
            }
        }
    }

    public HashSet<string> EnrolledStudents { get; private set; }

    public Course(string title,string teacher,int credits,int maxStudents)
    {
        Title = title;
        Teacher = teacher;
        Credits = credits;
        MaxStudents = maxStudents;
        EnrolledStudents = new HashSet<string>();
    }

    public void Enroll(string studentID)
    {
        if (EnrolledStudents.Count >= MaxStudents)
        {
            throw new InvalidOperationException("Нет мест на курсе");
        }
        else if (EnrolledStudents.Contains(studentID))
        {
            throw new InvalidOperationException("Студент уже записан");
        }
        else
        {
            EnrolledStudents.Add(studentID);
        }
    }
    public void Unenroll(string studentID)
    {
        if (!EnrolledStudents.Contains(studentID))
        {
            throw new InvalidOperationException("Студент не записан");
        }
        else
        {
            EnrolledStudents.Remove(studentID);
        }
    }

    public int FreeSeats()
    {
        return MaxStudents - EnrolledStudents.Count;
    }
}

class University
{
    public string Name { get; set; }

    public Dictionary<string,Student> Students { get; set; }
    public Dictionary<string,Course> Courses { get; set; }

    public University(string name)
    {
        Name = name;
        Students = new Dictionary<string, Student>();
        Courses = new Dictionary<string, Course>();
    }

    public void AddStudent(Student student)
    {
        if (Students.ContainsKey(student.Number))
        {
            throw new InvalidOperationException("Номер студента уже используется");
        }
        else
        {
            Students.Add(student.Number, student);
        }
    }

    public void AddCourse(Course course)
    {
        if (Courses.ContainsKey(course.Title))
        {
            throw new InvalidOperationException("Название курса уже используется");
        }
        else
        {
            Courses.Add(course.Title, course);
        }
    }

    public Student FindStudent(string id)
    {
        if (Students.TryGetValue(id, out Student student))
        {
            return student;
        }
        else
        {
            return null;
        }
    }

    public Course FindCourse(string title)
    {
        if (Courses.TryGetValue(title,out Course course))
        {
            return course;
        }
        else
        {
            return null;
        }
    }

    public void EnrollStudent(string studentID,string courseTitle)
    {
        Student foundedstudent = FindStudent(studentID);
        if (foundedstudent == null)
        {
            throw new InvalidOperationException("Студент не найден");
        }
        Course foundedcourse = FindCourse(courseTitle);
        if (foundedcourse == null)
        {
            throw new InvalidOperationException("Курс не найден");
        }
        
        switch(foundedstudent.Status)
        {
            case StudentStatus.Active:
                foundedcourse.Enroll(foundedstudent.Number);
                break;
            default:
                throw new InvalidOperationException("Студент не активен");
        }
    }
    public void UnenrollStudent(string studentID, string courseTitle)
    {
        Student foundedstudent = FindStudent(studentID);
        if (foundedstudent == null)
        {
            throw new InvalidOperationException("Студент не найден");
        }
        Course foundedcourse = FindCourse(courseTitle);
        if (foundedcourse == null)
        {
            throw new InvalidOperationException("Курс не найден");
        }
        foundedcourse.Unenroll(foundedstudent.Number);
    }

    public void AssignGrade(string studentID,string courseTitle, int grade)
    {
        Student foundedstudent = FindStudent(studentID);
        if (foundedstudent == null)
        {
            throw new InvalidOperationException("Студент не найден");
        }
        Course foundedcourse = FindCourse(courseTitle);
        if (foundedcourse == null)
        {
            throw new InvalidOperationException("Курс не найден");
        }
        if (!foundedcourse.EnrolledStudents.Contains(studentID))
        {
            throw new InvalidOperationException("Студент не записан на курс");
        }
        else
        {
            foundedstudent.AddGrade(courseTitle, grade);
        }
    }

    public void ExpelStudent(string studentID)
    {
        Student foundedstudent = FindStudent(studentID);
        if (foundedstudent == null)
        {
            throw new InvalidOperationException("Студент не найден");
        }
        foundedstudent.Expel();

        foreach (var pair in Courses)
        {
            if (pair.Value.EnrolledStudents.Contains(studentID))
            {
                pair.Value.Unenroll(studentID);
            }
        }
    }
}

class Program
{
    static void Main()
    {
        University university = new University("МГУ");

        while (true)
        {
            try
            {
                Console.WriteLine("Меню:\n1. Добавить студента\n2. Добавить курс\n3. Записать на курс\n4. Отписать от курса\n5. Поставить оценку\n6. Отчислить студента\n7. Отчёты\n0. Выход");
                int choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        Console.WriteLine("Введите уникальный номер студента");
                        string number = Console.ReadLine();
                        Console.WriteLine("Введите имя студента");
                        string name = Console.ReadLine();
                        Console.WriteLine("Введите факультет студента");
                        string faculty = Console.ReadLine();
                        university.AddStudent(new Student(number, name, faculty));
                        Console.WriteLine($"Студент {name} с номером {number} успешно добавлен!");
                        break;
                    case 2:
                        Console.WriteLine("Введите название курса");
                        string title = Console.ReadLine();
                        Console.WriteLine("Введите имя учителя");
                        string teacher = Console.ReadLine();
                        Console.WriteLine("Введите кредиты");
                        int credits = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Введите максимальное число студентов");
                        int maxStudents = Convert.ToInt32(Console.ReadLine());
                        university.AddCourse(new Course(title, teacher, credits, maxStudents));
                        Console.WriteLine($"Курс {title} успешно добавлен!");
                        break;
                    case 3:
                        Console.WriteLine("Введите уникальный номер студента");
                        string enrollstudentID = Console.ReadLine();
                        Console.WriteLine("Введите название курса");
                        string enrollcourseTitle = Console.ReadLine();
                        university.EnrollStudent(enrollstudentID, enrollcourseTitle);
                        Console.WriteLine($"Студент {enrollstudentID} записан на курс {enrollcourseTitle} успешно!");
                        break;
                    case 4:
                        Console.WriteLine("Введите уникальный номер студента");
                        string unerollstudentID = Console.ReadLine();
                        Console.WriteLine("Введите название курса");
                        string unerollcourseTitle = Console.ReadLine();
                        university.UnenrollStudent(unerollstudentID, unerollcourseTitle);
                        Console.WriteLine($"Студент {unerollstudentID} отписан от курса {unerollcourseTitle} успешно!");
                        break;
                    case 5:
                        Console.WriteLine("Введите уникальный номер студента");
                        string gradestudentID = Console.ReadLine();
                        Console.WriteLine("Введите название курса");
                        string gradecourseTitle = Console.ReadLine();
                        Console.WriteLine("Введите оценку: 2-5");
                        int grade = Convert.ToInt32(Console.ReadLine());
                        university.AssignGrade(gradestudentID, gradecourseTitle, grade);
                        Console.WriteLine($"Оценка {grade} успешно добавлена студенту {gradestudentID}!");
                        break;
                    case 6:
                        Console.WriteLine("Введите уникальный номер студента");
                        string expellStudentID = Console.ReadLine();
                        university.ExpelStudent(expellStudentID);
                        Console.WriteLine($"Студент {expellStudentID} отчислен успешно!");
                        break;
                    case 7:
                        int action =1000;
                        while (action != 0)
                        {
                            try
                            {
                                Console.WriteLine("Меню отчётов:" +
                                    "\n1. Все студенты" +
                                    "\n2. Все курсы" +
                                    "\n3. Уникальные факультеты" +
                                    "\n4. Топ-3 студента по среднему баллу" +
                                    "\n5. Стипендиаты" +
                                    "\n6. Средний балл по факультету" +
                                    "\n7. Курсы без студентов" +
                                    "\n8. Студенты без оценок" +
                                    "\n9. Распределение оценок" +
                                    "\n10. Отличники" +
                                    "\n0. Выход" +
                                    "");
                                action = Convert.ToInt32(Console.ReadLine());

                                switch (action)
                                {
                                    case 1:
                                        foreach (var pair in university.Students)
                                        {
                                            Console.WriteLine("Номер: " + pair.Key);
                                            Console.WriteLine("Имя: " + pair.Value.Name);
                                            Console.WriteLine("Факультет: " + pair.Value.Faculty);
                                            Console.WriteLine("Статус: " + pair.Value.Status);
                                            double average = pair.Value.GetAverageGrade();
                                            if (average > 0)
                                            {
                                                Console.WriteLine("Средний бал: " + average);
                                            }    
                                            else
                                            {
                                                Console.WriteLine("Нет оценок");
                                            }
                                            Console.WriteLine("-----");
                                        }
                                        break;
                                    case 2:
                                        foreach(var pair in university.Courses)
                                        {
                                            Console.WriteLine("Название: " + pair.Key);
                                            Console.WriteLine("Учитель: " + pair.Value.Teacher);
                                            Console.WriteLine("Кредиты: " + pair.Value.Credits);
                                            Console.WriteLine("Количество пустых мест: " + pair.Value.FreeSeats() + " Общее количество мест: " + pair.Value.MaxStudents);
                                            Console.WriteLine("Записанные студенты:");
                                            foreach (var student in pair.Value.EnrolledStudents)
                                            {
                                                Console.WriteLine(student);
                                            }
                                            Console.WriteLine("-----");
                                        }
                                        break;
                                    case 3:
                                        HashSet<string> UniqueFaculty = new HashSet<string>();
                                        foreach (var pair in university.Students)
                                        {
                                            UniqueFaculty.Add(pair.Value.Faculty);
                                        }
                                        foreach (var uniquefaculty in UniqueFaculty)
                                        {
                                            Console.WriteLine("Название: " + uniquefaculty);
                                        }
                                        break;
                                    case 4:
                                        Dictionary<string, double> Top3Students = new Dictionary<string, double>();
                                        foreach (var pair in university.Students)
                                        {
                                            if (pair.Value.Grades.Count > 0)
                                            {
                                                if (Top3Students.TryGetValue(pair.Key, out double value))
                                                {
                                                    Top3Students[pair.Key] += pair.Value.GetAverageGrade();
                                                }
                                                else
                                                {
                                                    Top3Students[pair.Key] = pair.Value.GetAverageGrade();
                                                }
                                            }
                                        }
                                        for (int i = 0; i<3;i++)
                                        {
                                            string winner = "";
                                            double winneraverage = 0;
                                            if (Top3Students.Count == 0)
                                            {
                                                break;
                                            }
                                            foreach (var pair in Top3Students)
                                            {
                                                if (winneraverage<pair.Value)
                                                {
                                                    winner = pair.Key;
                                                    winneraverage = pair.Value;
                                                }
                                            }
                                            Console.WriteLine("Номер: " + winner + " Средний бал: "+ winneraverage);
                                            Top3Students.Remove(winner);
                                            winner = "";
                                            winneraverage = 0;
                                        }
                                        break;
                                    case 5:
                                        foreach(var pair in university.Students)
                                        {
                                            if (pair.Value.Grades.Count > 0)
                                            {
                                                switch (pair.Value.GetAverageGrade())
                                                {
                                                    case double average when average >= 4.5:
                                                        Console.WriteLine("Имя: " + pair.Value.Name);
                                                        Console.WriteLine("Повышенная стипендия");
                                                        Console.WriteLine("-----");
                                                        break;
                                                    case double average when average >= 4:
                                                        Console.WriteLine("Имя: " + pair.Value.Name);
                                                        Console.WriteLine("Стандартная стипендия");
                                                        Console.WriteLine("-----");
                                                        break;
                                                    case double average when average < 4:
                                                        break;
                                                    default:
                                                        throw new InvalidOperationException("Мимо кассы");
                                                }
                                            }
                                        }
                                        break;
                                    case 6:
                                        Dictionary<string, double> AverageFaculty = new Dictionary<string, double>();
                                        Dictionary<string, int> StudentsCount = new Dictionary<string, int>();
                                        foreach (var pair in university.Students)
                                        {
                                            if (pair.Value.Grades.Count >0)
                                            {
                                                if(AverageFaculty.TryGetValue(pair.Value.Faculty,out double value))
                                                {
                                                    AverageFaculty[pair.Value.Faculty] += pair.Value.GetAverageGrade();
                                                    StudentsCount[pair.Value.Faculty]++;
                                                }
                                                else
                                                {
                                                    AverageFaculty[pair.Value.Faculty] = pair.Value.GetAverageGrade();
                                                    StudentsCount[pair.Value.Faculty] = 1;
                                                }
                                            }
                                        }
                                        foreach (var pair in AverageFaculty)
                                        {
                                            Console.WriteLine("Факультет: " + pair.Key + " Средний бал: " + AverageFaculty[pair.Key] / StudentsCount[pair.Key]);
                                        }
                                        break;
                                    case 7:
                                        foreach (var pair in university.Courses)
                                        {
                                            if (pair.Value.EnrolledStudents.Count ==0)
                                            {
                                                Console.WriteLine(pair.Key);
                                            }
                                        }
                                        break;
                                    case 8:
                                        foreach (var pair in university.Students)
                                        {
                                            if (pair.Value.Grades.Count == 0)
                                            {
                                                Console.WriteLine(pair.Key);
                                            }
                                        }
                                        break;
                                    case 9:
                                        Dictionary<int, int> Grades = new Dictionary<int, int>();
                                        foreach (var pair in university.Students)
                                        {
                                            foreach(var p in pair.Value.Grades)
                                            {
                                                if (Grades.TryGetValue(p.Value,out int value))
                                                {
                                                    Grades[p.Value]++;
                                                }
                                                else
                                                {
                                                    Grades[p.Value] = 1;
                                                }
                                            }
                                        }
                                        foreach (var pair in Grades)
                                        {
                                            Console.WriteLine("Оценка: " + pair.Key + " Количество: " + pair.Value);
                                        }
                                        break;
                                    case 10:
                                        foreach(var pair in university.Students)
                                        {
                                            bool allFive = true;
                                            if (pair.Value.Grades.Count == 0)
                                            {
                                                allFive = false;
                                            }
                                            else
                                            {
                                                foreach(var p in pair.Value.Grades)
                                                {
                                                    if (p.Value != 5)
                                                    {
                                                        allFive = false;
                                                    }
                                                }
                                                if (allFive)
                                                {
                                                    Console.WriteLine(pair.Value.Name);
                                                }
                                            }
                                        }
                                        break;
                                    case 0:
                                        break;
                                    default:
                                        Console.WriteLine("Неправильный ввод меню");
                                        break;
                                }
                            }
                            catch (Exception)
                            {
                                Console.WriteLine("Что то пошло не так");
                            }
                            finally
                            {
                                Console.WriteLine($"========={university.Name}=========");
                            }
                        }
                        break;
                    case 0:
                        return;
                    default:
                        Console.WriteLine("Неправильный ввод меню");
                        break;
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Ошибка: введено не число");
            }
            catch (OverflowException)
            {
                Console.WriteLine("Ошибка: число слишком большое");
            }
            catch (DivideByZeroException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception)
            {
                Console.WriteLine("Ошибка: что то пошло не так");
            }
            finally
            {
                Console.WriteLine($"========={university.Name}=========");
            }
        }
    }
}