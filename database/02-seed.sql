-- =============================================================================
-- Student Grading System - demo data
-- Run after 01-schema.sql. Every name below is fictional.
--
-- Demo accounts (local development only - change or remove them before any real use):
--   instructor                     / Instructor@123
--   jsmith, ejohnson, mbrown, ...  / Student@123
-- =============================================================================

USE StudentGradingSystem;

-- -----------------------------------------------------------------------------
-- Courses
-- -----------------------------------------------------------------------------
INSERT INTO Courses (CourseID, CourseCode, CourseTitle, Semester, Status) VALUES
    (1,  'CS101', 'Programming Fundamentals',    'Fall 2025',   'Active'),
    (2,  'CS102', 'Object Oriented Programming', 'Spring 2025', 'Active'),
    (3,  'CS201', 'Data Structures',             'Fall 2025',   'Active'),
    (4,  'CS202', 'Database Systems',            'Spring 2025', 'Active'),
    (5,  'CS301', 'Software Construction',       'Fall 2025',   'Active'),
    (6,  'CS302', 'Design and Analysis of Algorithms', 'Spring 2025', 'Active'),
    (7,  'CS401', 'Operating Systems',           'Fall 2025',   'Active'),
    (8,  'CS402', 'Computer Networks',           'Spring 2025', 'Active'),
    (9,  'CS501', 'Artificial Intelligence',     'Fall 2025',   'Active'),
    (10, 'CS502', 'Machine Learning',            'Spring 2025', 'Active');

-- -----------------------------------------------------------------------------
-- Students
-- -----------------------------------------------------------------------------
INSERT INTO Students (StudentID, StudentName) VALUES
    (1,  'John Smith'),
    (2,  'Emily Johnson'),
    (3,  'Michael Brown'),
    (4,  'Sarah Davis'),
    (5,  'David Wilson'),
    (6,  'Jennifer Lee'),
    (7,  'Robert Taylor'),
    (8,  'Lisa Anderson'),
    (9,  'Thomas Martinez'),
    (10, 'Jessica Garcia');

-- Every demo student takes every course.
INSERT INTO CourseEnrollment (CourseID, StudentID)
SELECT c.CourseID, s.StudentID
FROM Courses c
CROSS JOIN Students s;

-- -----------------------------------------------------------------------------
-- Login accounts (PBKDF2-SHA256, 210,000 iterations, unique salt per user)
-- -----------------------------------------------------------------------------
INSERT INTO Users (Username, PasswordHash, Role, StudentID) VALUES
    ('instructor', 'pbkdf2-sha256$210000$EeI7nW0N7uYBytlo1LVimw==$BjW7VDOFVA2Pi/dC46ZPs+gk8dlR2dCE9yjJhw0dXMA=', 'Instructor', NULL),
    ('jsmith',     'pbkdf2-sha256$210000$wtzAD7yhlBzSGABwLMH3Lw==$7ZbPJEJ+tscL01i7BEgPhY7dGk+LPDs+gOk54U4MUag=', 'Student', 1),
    ('ejohnson',   'pbkdf2-sha256$210000$En+1wrL5FVMAvOlELCmGFQ==$9foGHPboGyuLSFTBEGN/UH6gVLxUUVdxgGpX45sL72U=', 'Student', 2),
    ('mbrown',     'pbkdf2-sha256$210000$HqC8q3jRxiRvIs7qDVTCug==$+kJ60JM3sX5m7MYqgpEIodto/JYQVZJsPQHLXkmGh2I=', 'Student', 3),
    ('sdavis',     'pbkdf2-sha256$210000$YUli6O4oGrfhAkMUn9ysmQ==$7uoT6GJNqLxTIUC8bs06KLCqa9GeQ7hvJjdqA2BHCXk=', 'Student', 4),
    ('dwilson',    'pbkdf2-sha256$210000$Y3n/AxqFnHyWLFm2MZRKBg==$XA/Qhiz+TyK/9D7kgo40oKJLos8sywsoeqIIl2+qBTA=', 'Student', 5),
    ('jlee',       'pbkdf2-sha256$210000$zMaOa+rDX4Ac9k6NQFn0pg==$5xa7Je2KBsPKuItspvQ2xAQpRcV0JuP/SmQNTQOLzEc=', 'Student', 6),
    ('rtaylor',    'pbkdf2-sha256$210000$G8c5OdjmRxx24vvHBjJCRA==$G33gklj0LjMURKWBeKjGIS71s5Kv5kr/rD8rzuQyWY8=', 'Student', 7),
    ('landerson',  'pbkdf2-sha256$210000$x3IcRH8R+Jw+YT4rab8oDQ==$qpu/hIEHK3mSJp0GzLItD+gJVAlIgm5/ODFRxDWGe84=', 'Student', 8),
    ('tmartinez',  'pbkdf2-sha256$210000$Lqa66QNImg/LCm82MLzfng==$/Y3oi1rciAJ5PNtVcAUcxbJ05wEMzEHCws2BTu7+/Xc=', 'Student', 9),
    ('jgarcia',    'pbkdf2-sha256$210000$bx4LsVNbHYqcw+xCgTSLMg==$mYfAop6HqjXUrt6yTTZbXl3Iv6Ftgx5+c6VCUJulCA0=', 'Student', 10);

-- -----------------------------------------------------------------------------
-- CS301 Software Construction - fully assessed (weightages total 100%)
-- -----------------------------------------------------------------------------
INSERT INTO Activities (ActivityID, CourseID, ActivityType, TotalMarks, Weightage) VALUES
    (1, 5, 'Quiz 1',       10,  10),
    (2, 5, 'Assignment 1', 20,  15),
    (3, 5, 'Mid Term',     50,  25),
    (4, 5, 'Final Exam',   100, 50);

INSERT INTO StudentMarks (ActivityID, StudentID, ObtainedMarks) VALUES
    -- Quiz 1 (out of 10)
    (1, 1, 9.5), (1, 2, 9), (1, 3, 8), (1, 4, 8.5), (1, 5, 7),
    (1, 6, 7.5), (1, 7, 6), (1, 8, 6.5), (1, 9, 5), (1, 10, 4),
    -- Assignment 1 (out of 20)
    (2, 1, 19), (2, 2, 18), (2, 3, 17), (2, 4, 16), (2, 5, 15),
    (2, 6, 14), (2, 7, 13), (2, 8, 12), (2, 9, 11), (2, 10, 8),
    -- Mid Term (out of 50)
    (3, 1, 46), (3, 2, 44), (3, 3, 40), (3, 4, 38), (3, 5, 36),
    (3, 6, 33), (3, 7, 31), (3, 8, 28), (3, 9, 26), (3, 10, 20),
    -- Final Exam (out of 100)
    (4, 1, 92), (4, 2, 85), (4, 3, 80), (4, 4, 76), (4, 5, 72),
    (4, 6, 66), (4, 7, 62), (4, 8, 56), (4, 9, 51), (4, 10, 40);

-- -----------------------------------------------------------------------------
-- CS201 Data Structures - in progress (60% assessed, no final exam yet)
-- -----------------------------------------------------------------------------
INSERT INTO Activities (ActivityID, CourseID, ActivityType, TotalMarks, Weightage) VALUES
    (5, 3, 'Quiz 1',       10, 10),
    (6, 3, 'Assignment 1', 25, 20),
    (7, 3, 'Mid Term',     50, 30);

INSERT INTO StudentMarks (ActivityID, StudentID, ObtainedMarks) VALUES
    (5, 1, 8),  (5, 2, 10), (5, 3, 7),  (5, 4, 9),  (5, 5, 6),
    (5, 6, 8),  (5, 7, 5),  (5, 8, 9),  (5, 9, 7),  (5, 10, 6),
    (6, 1, 22), (6, 2, 24), (6, 3, 18), (6, 4, 20), (6, 5, 15),
    (6, 6, 21), (6, 7, 14), (6, 8, 23), (6, 9, 17), (6, 10, 12),
    (7, 1, 41), (7, 2, 47), (7, 3, 35), (7, 4, 39), (7, 5, 30),
    (7, 6, 43), (7, 7, 27), (7, 8, 45), (7, 9, 33), (7, 10, 25);

-- -----------------------------------------------------------------------------
-- Released grades for CS301, computed with the same rules the app uses:
--   percentage = SUM(obtained / total * weightage), rounded to 2 decimals
--   A >= 90, A- >= 85, B+ >= 80, B >= 75, B- >= 70, C+ >= 65, C >= 60, C- >= 55, D >= 50, else F
-- -----------------------------------------------------------------------------
INSERT INTO Grades (CourseID, StudentID, Percentage, FinalGrade)
SELECT r.CourseID,
       r.StudentID,
       r.Percentage,
       CASE
           WHEN r.Percentage >= 90 THEN 'A'
           WHEN r.Percentage >= 85 THEN 'A-'
           WHEN r.Percentage >= 80 THEN 'B+'
           WHEN r.Percentage >= 75 THEN 'B'
           WHEN r.Percentage >= 70 THEN 'B-'
           WHEN r.Percentage >= 65 THEN 'C+'
           WHEN r.Percentage >= 60 THEN 'C'
           WHEN r.Percentage >= 55 THEN 'C-'
           WHEN r.Percentage >= 50 THEN 'D'
           ELSE 'F'
       END
FROM (
    SELECT ce.CourseID,
           ce.StudentID,
           ROUND(COALESCE(SUM(sm.ObtainedMarks / a.TotalMarks * a.Weightage), 0), 2) AS Percentage
    FROM CourseEnrollment ce
    JOIN Activities a          ON a.CourseID = ce.CourseID
    LEFT JOIN StudentMarks sm  ON sm.ActivityID = a.ActivityID AND sm.StudentID = ce.StudentID
    WHERE ce.CourseID = 5
    GROUP BY ce.CourseID, ce.StudentID
) AS r;
