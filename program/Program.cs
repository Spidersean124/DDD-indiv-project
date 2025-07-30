using DDDProject.Services;
using System;
using System.Runtime.CompilerServices;
using DDDProject.DataSaving;
using DDDProject.Reports_Meetings;
using DDDProject.Stakeholders;

namespace DDDProject.Main
{
    internal class Program
    {
        static void Main(string[] args)
        {
            InitialLoginPage loginPageHandler = new InitialLoginPage();
            loginPageHandler.LoginPage();
        }
    }
}
