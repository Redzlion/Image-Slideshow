using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using Image = System.Drawing.Image;

namespace Image_Slideshow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string Name { get; set; }
        public static List<Images> images = new List<Images>();
        public Storyboard inAnimation;
        public Storyboard outAnimation;

        public MainWindow()
        {
            InitializeComponent();
            var plugins = Directory.GetFiles(@Directory.GetCurrentDirectory()).Where(x => x.EndsWith(".dll"));

            foreach (var plugin in plugins)
            {
                Assembly DLL = Assembly.LoadFrom(plugin);
                foreach (Type type in DLL.GetExportedTypes())
                {
                    if (typeof(ISlideshowEffect).IsAssignableFrom(type))
                    {
                        if (type.IsInterface) continue;
                        ISlideshowEffect effect = Activator.CreateInstance(type) as ISlideshowEffect;
                        if (effect == null) continue;
                        else
                        {
                            cbEffects.Items.Add(effect);
                            slideshowMenu.Items.Add(effect);
                        }
                    }
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var item in DriveInfo.GetDrives())
            {
                var tvi = new TreeViewItem { Header = item, Tag = item, FontWeight = FontWeights.Normal };
                tvi.Items.Add(node);
                tvi.Expanded += new RoutedEventHandler(explorer_Expanded);
                partitionsTV.Items.Add(tvi);
            }
        }

        public static bool DirectoryHasPermission(string DirectoryPath, FileSystemRights AccessRight)
        {
            if (string.IsNullOrEmpty(DirectoryPath)) return false;

            try
            {
                AuthorizationRuleCollection rules = Directory.GetAccessControl(DirectoryPath).GetAccessRules(true, true, typeof(System.Security.Principal.SecurityIdentifier));
                WindowsIdentity identity = WindowsIdentity.GetCurrent();

                foreach (FileSystemAccessRule rule in rules)
                {
                    if (identity.Groups.Contains(rule.IdentityReference))
                    {
                        if ((AccessRight & rule.FileSystemRights) == AccessRight)
                        {
                            if (rule.AccessControlType == AccessControlType.Allow)
                                return true;
                        }
                    }
                }
            }
            catch { }
            return false;
        }

        private void PartitionsTV_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                images.Clear();
                imageListView.ItemsSource = null;
                imageListView.Items.Clear();

                if (DirectoryHasPermission(((TreeViewItem)partitionsTV.SelectedItem).Tag.ToString(), FileSystemRights.Read))
                {
                    var files = Directory.GetFiles(((TreeViewItem)partitionsTV.SelectedItem).Tag.ToString(), "*.*", SearchOption.TopDirectoryOnly).Where(x => x.EndsWith(".png") || x.EndsWith(".jpg") || x.EndsWith(".gif"));
                    if (files.Count() > 0)
                    {
                        foreach (var file in files)
                        {
                            images.Add(new Images
                            {
                                name = new FileInfo(file).Name,
                                image = new FileInfo(file).FullName
                            });
                        }
                        if (images.Count > 0)
                        {
                            imageListView.ItemsSource = images;
                        }
                    }
                }
            }
            catch (Exception ex) { }
        }

        public object node = null;

        private void explorer_Expanded(object sender, RoutedEventArgs e)
        {
            var tvi = (TreeViewItem)sender;
            if (tvi.Items.Count > 0 && tvi.Items[0] == node)
            {
                tvi.Items.Clear();
                try
                {
                    foreach (string s in Directory.GetDirectories(tvi.Tag.ToString()))
                    {
                        var subItem = new TreeViewItem
                        {
                            Header = s.Substring(s.LastIndexOf("\\") + 1),
                            Tag = s,
                            FontWeight = FontWeights.Normal,
                        };
                        subItem.Items.Add(node);
                        subItem.Expanded += new RoutedEventHandler(explorer_Expanded);
                        tvi.Items.Add(subItem);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void imageListView_Selected(object sender, RoutedEventArgs e)
        {
        }

        private void imageListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (imageListView.SelectedItems.Count > 0)
            {
                fileDetails.Children.Clear();
                fileDetails.Children.Add(new TextBlock { Margin = new Thickness(5), TextAlignment = TextAlignment.Left, Text = $"File name: {images[imageListView.SelectedIndex].name}" });

                fileDetails.Children.Add(new TextBlock { Margin = new Thickness(5), TextAlignment = TextAlignment.Left, Text = $"Width:\t{Image.FromFile(images[imageListView.SelectedIndex].image).Width.ToString()}" });
                fileDetails.Children.Add(new TextBlock { Margin = new Thickness(5), TextAlignment = TextAlignment.Left, Text = $"Height:\t{Image.FromFile(images[imageListView.SelectedIndex].image).Height.ToString()}" });
                fileDetails.Children.Add(new TextBlock { Margin = new Thickness(5), TextAlignment = TextAlignment.Left, Text = $"Size:\t{(new FileInfo(images[imageListView.SelectedIndex].image).Length / 1024).ToString()} KB" });
            }
            else
            {
                fileDetails.Children.Clear();
                fileDetails.Children.Add(new TextBlock { Margin = new Thickness(5), TextAlignment = TextAlignment.Center, Text = "No File Selected" });
            }
        }

        private void selectFolder_Click(object sender, RoutedEventArgs e)
        {
            var folderDialog = new System.Windows.Forms.FolderBrowserDialog();
            if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    images.Clear();
                    imageListView.ItemsSource = null;
                    imageListView.Items.Clear();

                    if (DirectoryHasPermission(folderDialog.SelectedPath, FileSystemRights.Read))
                    {
                        var files = Directory.GetFiles(folderDialog.SelectedPath, "*.*", SearchOption.TopDirectoryOnly).Where(x => x.EndsWith(".png") || x.EndsWith(".jpg") || x.EndsWith(".gif"));
                        if (files.Count() > 0)
                        {
                            foreach (var file in files)
                            {
                                images.Add(new Images
                                {
                                    name = new FileInfo(file).Name,
                                    image = new FileInfo(file).FullName
                                });
                                //                       }
                            }
                            if (images.Count > 0)
                            {
                                imageListView.ItemsSource = images;
                            }
                        }
                    }
                }
                catch (Exception ex) { }
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("A Simple Image Slideshow app");
        }

        private void btnStartSlideshow_Click(object sender, RoutedEventArgs e)
        {
            if (imageListView.Items.Count > 1)
            {
                new Slideshow(cbEffects.SelectedItem) { Owner = this }.ShowDialog();
            }
        }

        private void slideshowMenu_Click(object sender, RoutedEventArgs e)
        {
            if (imageListView.Items.Count > 1)
            {
                var window = new Slideshow(((MenuItem)e.OriginalSource).CommandParameter) { Owner = this };
                window.ShowDialog();
            }
        }

        private void exit_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}