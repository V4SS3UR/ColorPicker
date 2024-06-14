using ColorPicker.Core;
using ColorPicker.MVVM.Model;
using ColorPicker.MVVM.View;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Media;

namespace ColorPicker.MVVM.ViewModel
{
    internal class ProjectNavigation_ViewModel : ObservableObject
    {
        public static ProjectNavigation_ViewModel Instance { get; private set; }

        public RelayCommand AddProjectCommand { get; set; }

        public ObservableCollection<Project> ProjectCollection { get; private set; }

        private Project _selectedProject; public Project SelectedProject
        {
            get { return _selectedProject; }
            set
            {
                _selectedProject = value;

                if (value != null)
                {
                    NavigateToProject(value);
                }
                else
                {
                    CurrentView = null;
                }

                OnPropertyChanged();
            }
        }

        private object _currentView; public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }

        public ProjectNavigation_ViewModel()
        {
            Instance = this;

            this.ProjectCollection = new ObservableCollection<Project>();

            ProjectSave[] projectSaves = GetJson();

            if (!projectSaves.Any())
            {
                this.AddProjectCommandFunction();
            }

            foreach (var projectSave in projectSaves)
            {
                Project project = new Project()
                {
                    Name = projectSave.Name,
                    ColorPinned = new System.Collections.ObjectModel.ObservableCollection<ColorObject>(projectSave.ColorPinned.Select(o => new ColorObject(o.Name) { Color = (Color)ColorConverter.ConvertFromString(o.HexValue) })),
                    ColorHistory = new System.Collections.ObjectModel.ObservableCollection<ColorObject>(projectSave.HistoryPinned.Select(o => new ColorObject(o.Name) { Color = (Color)ColorConverter.ConvertFromString(o.HexValue) })),
                };

                this.AddProjectCommandFunction(project);
            }

            this.AddProjectCommand = new RelayCommand(o => this.AddProjectCommandFunction());
        }

        public void AddProjectCommandFunction(Project project = null)
        {
            if (project == null)
            {
                project = new Project() { Name = "Nouveau" };
            }
            this.ProjectCollection.Add(project);
            SelectedProject = project;
        }

        public void DeleteProject(Project project)
        {
            this.ProjectCollection.Remove(project);
            this.SelectedProject = ProjectCollection.FirstOrDefault();
        }

        public void NavigateToProject(Project project)
        {
            Project_View view = new Project_View();
            Project_ViewModel viewModel = (Project_ViewModel)view.DataContext;
            viewModel.Project = project;
            this.CurrentView = view;
        }

        

        public static void SaveJson(Project[] projects)
        {
            ProjectSave[] projectSaves = projects.Select(o => new ProjectSave(o)).ToArray();
            string json = JsonConvert.SerializeObject(projectSaves, Newtonsoft.Json.Formatting.Indented);
            System.IO.File.WriteAllText(Path.Combine(AppContext.BaseDirectory, "projects.json"), json);
        }

        public static ProjectSave[] GetJson()
        {
            ProjectSave[] output = new ProjectSave[] { };
            try
            {
                output = JsonConvert.DeserializeObject<ProjectSave[]>(File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "projects.json")));
            }
            catch (Exception)
            {
            }

            return output;
        }

        public class ProjectSave
        {
            public string Name { get; set; }

            public ColorSave[] ColorPinned { get; set; }
            public ColorSave[] HistoryPinned { get; set; }

            public ProjectSave()
            {
            }

            public ProjectSave(Project project)
            {
                this.Name = project.Name;
                this.ColorPinned = project.ColorPinned.Select(o => new ColorSave(o)).ToArray();
                this.HistoryPinned = project.ColorHistory.Select(o => new ColorSave(o)).ToArray();
            }

            public class ColorSave
            {
                public string Name { get; set; }
                public string HexValue { get; set; }

                public ColorSave()
                {
                }

                public ColorSave(ColorObject colorObject)
                {
                    this.Name = colorObject.Name;
                    this.HexValue = colorObject.Color.ToString();
                }
            }
        }
    }
}