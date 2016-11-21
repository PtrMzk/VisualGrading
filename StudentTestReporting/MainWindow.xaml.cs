﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using StudentTestReporting.Students;
using StudentTestReporting.Tests;
using StudentTestReporting.Grades;

namespace StudentTestReporting
{
    public partial class MainWindow : Window
    {

        string defaultNameEntry = string.Empty;
        string defaultNicknameEntry = string.Empty;


        string defaultTestEntry = string.Empty;


        Student student = new Student();
        Test test = new Test();
        Grade grades2 = new Grade();

        public MainWindow()
        {
            new GradeManager();
            new StudentManager();
            new TestManager();
            InitializeComponent();
        }

   

    }
}

