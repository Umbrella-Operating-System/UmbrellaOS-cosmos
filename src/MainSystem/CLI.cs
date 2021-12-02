using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Cosmos.System;
using Umbrella.MainSystem.Console;

namespace Umbrella.MainSystem
{
    internal class CLI
    {
        public static void Commands()
        {
            input:
            // Before input like PS1 in BASH
            Console.WriteMethods.Write($"\n{Kernel.CurrentDirectory}", ConsoleColor.White);
            Console.WriteMethods.Write("@", ConsoleColor.Gray);
            Console.WriteMethods.Write($"{Kernel.CurrentUser}", ConsoleColor.Yellow);
            Console.WriteMethods.Write("> ", ConsoleColor.White);
            // Get input
            var input = System.Console.ReadLine();
            var arguments = Methods.ParseCommandLine(input); // Parse arguments
            var commandName = arguments[0]; // Command name
            if (arguments.Count > 0) arguments.RemoveAt(0); // Leave only the arguments

            if (commandName.Trim(' ').ToLower() is "mkdir")
            {
                #region Вывод помощи
                if (arguments.Count > 0 && arguments[0].ToLower() is "-h" or "--help")
                {
                    Methods.SnowHelp("mkdir [ARG IF NEED '-h' ] [DIR]", "This command create empty folder.", "-h, --help)");
                    goto input;
                }
                #endregion Вывод помощи

                #region Без опций
                if (arguments.Count > 0 && arguments[0] is not "-h" or "--help" && !arguments[0].Contains('-'))
                {
                    if (!arguments[0].Contains('.') && !arguments[0].Contains(@"\") && !arguments[0].Contains("/") &&
                        !arguments[0].Contains(':') && !arguments[0].Contains('*') && !arguments[0].Contains('?') &&
                        !arguments[0].Contains('<') && !arguments[0].Contains('>') && !arguments[0].Contains('|') &&
                        !arguments[0].Contains('+'))
                    {
                        #region Основной код

                        if (Directory.Exists(arguments[0])) // Если путь уже существует
                        {
                            System.Console.WriteLine("That path exists already.");
                            goto input;
                        }

                        if (!Directory.Exists(arguments[0])) // Если путь не существует - создаём
                        {
                            Directory.CreateDirectory(@$"{Kernel.CurrentDirectory}{arguments[0]}");
                            System.Console.WriteLine("Directory created!");
                            goto input;
                        }

                        #endregion Основной код
                    }
                    #region Если запрещённые символы есть
                    else
                    {
                        System.Console.WriteLine(
                            "The following characters are prohibited in the names of folders and files:\n" +
                            @"'-' '.' '\' '/' ':' '*' '?' '<' '>' '|' '+'");
                        goto input;
                    }
                    #endregion Если запрещённые символы есть
                }

                #endregion Если аргументы есть, но они не являются '-h' или '--help'

                #region Если добавлены лишние аргументы к опции

                if (arguments.Count > 1 &&
                    arguments[0].ToLower() is "-h"
                        or "--help") // Если аргументов больше одного, и один из них это существующая опция (человек пытает написать лишний аргумент к опции, типа $ ls -al CurrentDir"
                {
                    System.Console.WriteLine(
                        "The option is used without additional arguments. \nmkdir [ARG '-h' or '--help' if need] [DIRNAME]");
                    goto input;
                }

                #endregion Если добавлены лишние аргументы

                #region Если опции не существует

                if (arguments.Count > 0 && arguments[0].ToLower().Contains('-') &&
                    arguments[0].ToLower() is not "-h" or "--help")
                {
                    System.Console.WriteLine("Invalid option");
                    goto input;
                }

                #endregion Если опции не существует
            }

            #region Вывод файлов и каталогов ('-r' '-al')
            if (commandName.Trim(' ').ToLower() is "ls")
            {
                var dirs = Directory.GetDirectories(Kernel.CurrentDirectory); // Получение всех каталогов в текущей директории

                #region Вывод помощи
                #region Если в аргументе указана опция '-h' или '--help' 
                if (arguments.Count > 0 && (arguments[0].ToLower() is "-h" or "--help")) //  Если в аргументе указана опция '-h' или '--help' 
                {
                    Methods.SnowHelp("ls [ARG IF NEED]", "This command outputs all files in the current directory and can output additional information in subdirectories.", "-h, --help, -R, -al");

                    goto input;
                }
                #endregion Если в аргументе указана опция '-h' или '--help' 
                #endregion Вывод помощи

                #region Опции
                #region Опция '-r'
                if (arguments.Count is 1 && arguments[0].ToLower() is "-r") // Если аргумент один и в нём указана опция '-r'
                {
                    #region Верхний уровень
                    foreach (var dirsInCurrentDir in dirs) // Перечисление каталогов в текущей директории
                    {
                        var folderName = new DirectoryInfo(Kernel.CurrentDirectory + dirsInCurrentDir).Name; // Получение имени каталога

                        Console.WriteMethods.WriteLine($"Folder: {folderName}");

                        #region Нижний уровень
                        var subDirectories = Directory.GetDirectories(Kernel.CurrentDirectory + dirsInCurrentDir); // Получение подкаталогов в перечисленных выше каталогов текущей директории 
                        foreach (var subFolder in subDirectories) // Перечисление подкаталогов в перечисленных выше каталогов текущей директории 
                        {
                            var subFolderName = new DirectoryInfo(subFolder).Name; // Получение имени подкаталога
                            Console.WriteMethods.WriteLine($"---> Subfolder: {subFolderName}");
                        }
                        var subFiles = Directory.GetFiles(Kernel.CurrentDirectory + dirsInCurrentDir); // Получение файлов в перечисленных выше каталогов текущей директории 
                        foreach (var subFile in subFiles) // Перечисление файлов в перечисленных выше каталогов текущей директории 
                        {
                            var fileName = Path.GetFileName(subFile); // Получение имени файла
                            Console.WriteMethods.WriteLine("---> Subfile: " + fileName);
                        }
                        #endregion Нижний уровень

                    }

                    goto input;
                    #endregion Верхний уровень
                }
                #endregion Опция '-r'

                #region Опция '-al'
                if (arguments.Count is 1 && arguments[0].ToLower() is "-al") // Если аргумент лишь один и в нём указана опция '-al'
                {
                    var allFiles = Directory.GetFiles(Kernel.CurrentDirectory); // Получение файлов в текущей директории
                    foreach (var oneFile in allFiles) // Перечисление по одному файлу из всех выше полученных
                    {
                        var fileName = Path.GetFileName(Kernel.CurrentDirectory + oneFile); // Получение имени файла
                        var fileSize = new FileInfo(Kernel.CurrentDirectory + oneFile).Length; // Получение длинны файла (размера)
                        var fileContent = File.ReadAllText(Kernel.CurrentDirectory + oneFile); // Получение всего текста в файле

                        Console.WriteMethods.WriteLine("\nFile name: " + fileName);
                        Console.WriteMethods.WriteLine("File size: " + fileSize + "byte");
                        Console.WriteMethods.WriteLine("Content: " + fileContent);
                    }

                    goto input;
                }
                #endregion Опция '-al'

                #region Если добавлены лишние аргументы к опции
                if (arguments.Count > 1 &&
                    arguments[0].ToLower() is "-al" or "-h" or "--help" or "-r") // Если аргументов больше одного, и один из них это существующая опция (человек пытает написать лишний аргумент к опции, типа $ ls -al CurrentDir"
                {
                    Console.WriteMethods.WriteLine($"The option is used without additional arguments. \nls [ARG IF NEED]");
                    goto input;
                }
                #endregion Если добавлены лишние аргументы

                #region Если опции не существует
                if (arguments.Count > 0 && arguments[0].ToLower().Contains('-') &&
                    arguments[0].ToLower() is not "-h" or "--help" or "-r" or "-al") // Если аргументво больше 0, но в аргументе присутсвует символ '-', при этом не вызывая ни одну из существующих опций
                {
                    Console.WriteMethods.WriteLine("Invalid option");
                    goto input;
                }
                #endregion Если опции не существует
                #endregion Опции

                #region Если введена команда без аргументов
                if (arguments.Count is 0) // Если количество спарсенных аргументво равняется нулю
                {
                    foreach (var dirsInCurrentDir in dirs) // Перечисление каталогов
                    {
                        var folderName = new DirectoryInfo(Kernel.CurrentDirectory + dirsInCurrentDir).Name; // Получение имени каталога
                        Console.WriteMethods.WriteLine(folderName); // Вывод имени каталога
                    }

                    var allFiles = Directory.GetFiles(Kernel.CurrentDirectory); // Перечисление файлов
                    foreach (var oneFile in allFiles)
                    {
                        var fileName = Path.GetFileName(Kernel.CurrentDirectory + oneFile); // Получение имени файла
                        Console.WriteMethods.WriteLine(fileName); // Вывод имени файла
                    }
                    goto input; // Возвращение к вводу
                }
                #endregion Если введена команда без аргументов
            }
            #endregion Вывод файлов и каталогов

            
        }


        /// <summary>
        /// This function parses all arguments through the specified signs into an array
        /// </summary>
        /// <param name="cmdLine"></param>
        /// <returns>Array with arguments</returns>

    }
}

