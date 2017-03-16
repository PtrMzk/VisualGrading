delete from grade;
go;

delete from student;
GO;

delete from test;
GO;

insert into student(firstname, lastname, nickname, emailaddress, parentemailaddress)
    Values
      ('Pearl' ,'Matthews', null, 'pmatthews@xyz.com', 'thematthews@lorem.com'),
      ('Lydia', 'Proctor', null, 'lydia@abc.com', 'aprocter@ipsum.net')
      ,('Kathryn', 'Ortiz-Capulet', null, 'tcapulet@capulet.com', 'capulets@capulet.com')
      ,('Trent', 'Smith', null, 'mercutio@mercutio.net', null)
      ,('Thomas', 'Tillson', null, 'mercutio@mercutio.net', null)
      ,('Joseph', 'Ceja', null, 'mercutio@mercutio.net', null)
      ,('Matt', 'McFerren', null, 'mercutio@mercutio.net', null)
      ,('George', 'Stonge', null, 'mercutio@mercutio.net', null)
      ,('Tami', 'Rule', null, 'mercutio@mercutio.net', null)
      ,('Beverly', 'Chilsom', null, 'mercutio@mercutio.net', null)
      ,('Aida', 'Jacobs', null, 'mercutio@mercutio.net', null)
      ,('Joyce', 'Alexander', null, 'mercutio@mercutio.net', null)
      ,('Michael', 'Parker', null, 'mercutio@mercutio.net', null)
      ,('Joy', 'Jones', null, 'mercutio@mercutio.net', null)
      ,('Mary', 'Herring', null, 'mercutio@mercutio.net', null)
      ,('Jessie', 'Bates', null, 'mercutio@mercutio.net', null)
      ,('Shelly', 'Hopkins', null, 'mercutio@mercutio.net', null)
      ,('Steven', 'Ingram', null, 'mercutio@mercutio.net', null)
      ,('Luis', 'Huff', null, 'mercutio@mercutio.net', null)
      ,('Susan', 'Ellison', null, 'mercutio@mercutio.net', null)
      ,('Harvey', 'Smith', null, 'mercutio@mercutio.net', null);

go;


update student
set EmailAddress = lower((substr(firstname, 0 , 1) || lastname || '@lorem.com'))
  ,ParentEmailAddress = lower(('a' || lastname || '@ipsum.net'));
go


insert into test (Name, Subject, SubCategory, Date, SeriesNumber, MaximumPoints)
  values

    ('Spelling Quiz - Unit 1', 'English', 'Spelling', '2016-10-17', 0, 100),
    ('Spelling Quiz - Unit 2', 'English', 'Spelling', '2016-10-22', 0, 100),
    ('Spelling Quiz - Unit 3', 'English', 'Spelling', '2016-11-03', 0, 100),
    ('Spelling Quiz - Unit 4', 'English', 'Spelling', '2016-11-14', 0, 100),
    ('Spelling Quiz - Unit 5', 'English', 'Spelling', '2016-11-20', 0, 100),
    ('Spelling Quiz - Unit 6', 'English', 'Spelling', '2016-11-30', 0, 100),
    ('Algebra Quiz - Equations', 'Math', 'Algebra', '2016-09-13', 0, 100),
    ('Algebra Quiz - Functions', 'Math', 'Algebra', '2016-09-27', 0, 100),
    ('Algebra Quiz - Exponents', 'Math', 'Algebra', '2016-09-27', 0, 100),
    ('Algebra Final', 'Math', 'Algebra', '2016-11-01', 0, 100),
    ('English Quiz - Shakespeare', 'English', 'Literature', '2016-08-13', 0, 100),
    ('English Quiz - American Literature', 'English', 'Literature', '2016-10-13', 0, 100),
    ('English Final', 'English', 'Literature', '2016-12-01', 0, 100)
    ,('Chemistry Final', 'Science', 'Chemistry', '2016-12-12', 0, 100)
    ,('Chemistry Pop Quiz', 'Science', 'Chemistry', '2016-11-12', 0, 100);

    go;

insert into Grade (StudentID, TestID)
    select s.id, t.id from student s cross join test t;
go;

update grade
set points =
cast(abs(random()) / 9223372036854775807.0 * 40 as int) + 60;
go;

select * From Grade;
GO;

select * FROM Student;
go;