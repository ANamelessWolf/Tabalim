using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tabalim.Updater.Core.Controller;

namespace Tabalim.Updater.Sync
{
    class Program
    {
        /// <summary>
        /// Broom Hatter Version
        /// </summary>
        static String Version
        {
            get { return Assembly.GetAssembly(typeof(Program)).GetName().Version.ToString(3); }
        }
        /// <summary>
        /// Gets the help menu.
        /// </summary>
        /// <value>
        /// The help menu.
        /// </value>
        static string HelpMenu
        {
            get
            {
                return
                    "Tabalim Updater sync {0}\n" +
                    "-A\t\t\tCreate assembly patch\n" +
                    "-F\t\t\tCreate full update package\n" +
                    "-V\t\t\tChecks the server and installed version\n" +
                    "-H,\t\t\tPrints Help menu";
            }
        }
        /// <summary>
        /// Gets the available command options
        /// </summary>
        static string[] CommandOptions => new string[] { "-A", "-F", "-V", "-H" };
        /// <summary>
        /// Mains the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        static void Main(string[] args)
        {
            List<String> input = args.Select(x => x.ToUpper()).Distinct().ToList();
            List<TaskCommand> tasks = new List<TaskCommand>();
            try
            {
                //Se llena la lista de trabajo para Ryoga
                for (int i = 0; i < input.Count; i++)
                    if (CommandOptions.Contains(input[i]))
                        tasks.Add(new TaskCommand()
                        {
                            CommandString = input[i],
                            CommandParameters = args.Length > 0 ? SubArray<string>(args, 1) : null
                        });
                //Se resulven las tareas solicitadas
                foreach (TaskCommand task in tasks)
                    SolveTask(task);
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
        }
        /// <summary>
        /// Resuelve la tarea solicitada
        /// </summary>
        /// <param name="task">La tarea a resolver</param>
        private static void SolveTask(TaskCommand task)
        {
            try
            {
                switch (task.CommandString)
                {
                    case "-A":
                        //To Do update assembly
                        RunSyncCommand(task.CommandParameters, CreateAssembly);
                        break;
                    case "-F":
                        //To Do update full
                        RunSyncCommand(task.CommandParameters, CreateFullPackage);
                        break;
                    case "-V":
                        //To Do print app and server versión
                        break;
                    case "-H":
                        Console.WriteLine(String.Format(HelpMenu, Version));
                        break;
                    default:
                        throw new Exception("Commando no encontrado");
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
        /// <summary>
        /// Creates the full package.
        /// </summary>
        /// <param name="obj">The object.</param>
        private static void CreateFullPackage(string[] obj)
        {
            CheckerController ch = new CheckerController();
            var files = ch.GetFiles();
            PackageMaker pMaker = new PackageMaker(files, true);
            pMaker.MakePackage();
        }
        /// <summary>
        /// Creates the assembly.
        /// </summary>
        /// <param name="arg">The argument.</param>
        private static void CreateAssembly(string[] arg)
        {
            CheckerController ch = new CheckerController();
            var files = ch.GetFiles().Where(x=> x.IsAssembly);
            PackageMaker pMaker = new PackageMaker(files, true);
            pMaker.MakePackage();
            var json = JsonConvert.SerializeObject(pMaker.Info);
        }



        /// <summary>
        /// Runs the synchronize command.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="action">The action.</param>
        /// <exception cref="System.Exception">
        /// Commando en formato incorrecto, revise la ayuda. -H
        /// </exception>
        private static void RunSyncCommand(string[] parameters, Action<string[]> action)
        {
            try
            {
                action(parameters);
                Console.ReadLine();
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
        /// <summary>
        /// Gets the command information from an array
        /// </summary>
        /// <typeparam name="T">The array data type</typeparam>
        /// <param name="data">The array data.</param>
        /// <param name="index">The start index.</param>
        /// <param name="length">The new array length.</param>
        /// <returns></returns>
        private static T[] SubArray<T>(T[] data, int index)
        {
            T[] result = new T[data.Length - index];
            Array.Copy(data, index, result, 0, data.Length - index);
            return result;
        }
    }
}