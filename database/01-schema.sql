-- =============================================================================
-- Student Grading System - database schema
-- Target: MySQL 8.0.16 or newer (CHECK constraints are enforced from 8.0.16)
--
-- Creates the StudentGradingSystem database and all of its tables.
-- WARNING: running this script again DROPS the existing tables and their data.
--
--   mysql -u root -p < database/01-schema.sql
--   mysql -u root -p < database/02-seed.sql
-- =============================================================================

CREATE DATABASE IF NOT EXISTS StudentGradingSystem
    CHARACTER SET utf8mb4
    COLLATE utf8mb4_unicode_ci;

USE StudentGradingSystem;

-- Drop in reverse dependency order so the script can be re-run safely.
DROP TABLE IF EXISTS Grades;
DROP TABLE IF EXISTS StudentMarks;
DROP TABLE IF EXISTS Activities;
DROP TABLE IF EXISTS CourseEnrollment;
DROP TABLE IF EXISTS Users;
DROP TABLE IF EXISTS Courses;
DROP TABLE IF EXISTS Students;

-- -----------------------------------------------------------------------------
-- Students
-- -----------------------------------------------------------------------------
CREATE TABLE Students (
    StudentID    INT          NOT NULL AUTO_INCREMENT,
    StudentName  VARCHAR(100) NOT NULL,
    CreatedAt    TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (StudentID)
) ENGINE = InnoDB;

-- -----------------------------------------------------------------------------
-- Users - login accounts. Passwords are stored as salted PBKDF2-SHA256 hashes
-- in the format: pbkdf2-sha256$<iterations>$<base64 salt>$<base64 hash>
-- -----------------------------------------------------------------------------
CREATE TABLE Users (
    UserID        INT          NOT NULL AUTO_INCREMENT,
    Username      VARCHAR(50)  NOT NULL,
    PasswordHash  VARCHAR(255) NOT NULL,
    Role          ENUM('Instructor', 'Student') NOT NULL,
    StudentID     INT          NULL,
    CreatedAt     TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (UserID),
    UNIQUE KEY UQ_Users_Username (Username),
    UNIQUE KEY UQ_Users_StudentID (StudentID),
    CONSTRAINT FK_Users_Students
        FOREIGN KEY (StudentID) REFERENCES Students (StudentID),
    -- Student accounts must point at a student record; instructor accounts must not.
    CONSTRAINT CK_Users_RoleStudentLink CHECK (
        (Role = 'Student' AND StudentID IS NOT NULL) OR
        (Role = 'Instructor' AND StudentID IS NULL)
    )
) ENGINE = InnoDB;

-- -----------------------------------------------------------------------------
-- Courses
-- -----------------------------------------------------------------------------
CREATE TABLE Courses (
    CourseID     INT          NOT NULL AUTO_INCREMENT,
    CourseCode   VARCHAR(20)  NOT NULL,
    CourseTitle  VARCHAR(100) NOT NULL,
    Semester     VARCHAR(20)  NOT NULL,
    Status       VARCHAR(20)  NOT NULL DEFAULT 'Active',
    PRIMARY KEY (CourseID),
    UNIQUE KEY UQ_Courses_CourseCode (CourseCode),
    CONSTRAINT CK_Courses_Status CHECK (Status IN ('Active', 'Inactive', 'Completed'))
) ENGINE = InnoDB;

-- -----------------------------------------------------------------------------
-- CourseEnrollment - which students take which courses
-- -----------------------------------------------------------------------------
CREATE TABLE CourseEnrollment (
    EnrollmentID INT       NOT NULL AUTO_INCREMENT,
    CourseID     INT       NOT NULL,
    StudentID    INT       NOT NULL,
    EnrolledAt   TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (EnrollmentID),
    UNIQUE KEY UQ_CourseEnrollment_Course_Student (CourseID, StudentID),
    KEY IX_CourseEnrollment_Student (StudentID),
    CONSTRAINT FK_CourseEnrollment_Courses
        FOREIGN KEY (CourseID) REFERENCES Courses (CourseID) ON DELETE CASCADE,
    CONSTRAINT FK_CourseEnrollment_Students
        FOREIGN KEY (StudentID) REFERENCES Students (StudentID) ON DELETE CASCADE
) ENGINE = InnoDB;

-- -----------------------------------------------------------------------------
-- Activities - graded assessments in a course (quiz, assignment, mid term, ...)
-- The weightages of all activities in a course add up to at most 100.
-- -----------------------------------------------------------------------------
CREATE TABLE Activities (
    ActivityID   INT          NOT NULL AUTO_INCREMENT,
    CourseID     INT          NOT NULL,
    ActivityType VARCHAR(50)  NOT NULL,
    TotalMarks   DECIMAL(6,2) NOT NULL,
    Weightage    DECIMAL(5,2) NOT NULL,
    CreatedAt    TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (ActivityID),
    UNIQUE KEY UQ_Activities_Course_Type (CourseID, ActivityType),
    CONSTRAINT FK_Activities_Courses
        FOREIGN KEY (CourseID) REFERENCES Courses (CourseID) ON DELETE CASCADE,
    CONSTRAINT CK_Activities_TotalMarks CHECK (TotalMarks > 0),
    CONSTRAINT CK_Activities_Weightage  CHECK (Weightage > 0 AND Weightage <= 100)
) ENGINE = InnoDB;

-- -----------------------------------------------------------------------------
-- StudentMarks - marks a student obtained in an activity
-- -----------------------------------------------------------------------------
CREATE TABLE StudentMarks (
    MarkID        INT          NOT NULL AUTO_INCREMENT,
    ActivityID    INT          NOT NULL,
    StudentID     INT          NOT NULL,
    ObtainedMarks DECIMAL(6,2) NOT NULL,
    LastUpdated   TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (MarkID),
    UNIQUE KEY UQ_StudentMarks_Activity_Student (ActivityID, StudentID),
    KEY IX_StudentMarks_Student (StudentID),
    CONSTRAINT FK_StudentMarks_Activities
        FOREIGN KEY (ActivityID) REFERENCES Activities (ActivityID) ON DELETE CASCADE,
    CONSTRAINT FK_StudentMarks_Students
        FOREIGN KEY (StudentID) REFERENCES Students (StudentID) ON DELETE CASCADE,
    CONSTRAINT CK_StudentMarks_ObtainedMarks CHECK (ObtainedMarks >= 0)
) ENGINE = InnoDB;

-- -----------------------------------------------------------------------------
-- Grades - calculated final grade per student per course
-- -----------------------------------------------------------------------------
CREATE TABLE Grades (
    GradeID      INT          NOT NULL AUTO_INCREMENT,
    CourseID     INT          NOT NULL,
    StudentID    INT          NOT NULL,
    Percentage   DECIMAL(5,2) NOT NULL,
    FinalGrade   VARCHAR(2)   NOT NULL,
    CalculatedAt TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (GradeID),
    UNIQUE KEY UQ_Grades_Course_Student (CourseID, StudentID),
    KEY IX_Grades_Student (StudentID),
    CONSTRAINT FK_Grades_Courses
        FOREIGN KEY (CourseID) REFERENCES Courses (CourseID) ON DELETE CASCADE,
    CONSTRAINT FK_Grades_Students
        FOREIGN KEY (StudentID) REFERENCES Students (StudentID) ON DELETE CASCADE,
    CONSTRAINT CK_Grades_Percentage CHECK (Percentage >= 0 AND Percentage <= 100)
) ENGINE = InnoDB;
