using Newtonsoft.Json;
using System.Net;
using System.Reflection;

namespace HelloWorld
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CheckForUpdates();
        }
        public class UpdateInfo
        {
            public string version { get; set; }
            public string url { get; set; }
            public string changelog { get; set; }
        }

        private void CheckForUpdates()
        {
            string updateUrl = "https://raw.githubusercontent.com/menady0/HelloWorldApp/main/update-info.json";

            try
            {
                using (WebClient client = new WebClient())
                {
                    string json = client.DownloadString(updateUrl);
                    UpdateInfo update = JsonConvert.DeserializeObject<UpdateInfo>(json);

                    Version currentVersion = Assembly.GetExecutingAssembly().GetName().Version;
                    Version latestVersion = new Version(update.version);

                    if (latestVersion > currentVersion)
                    {
                        DialogResult result = MessageBox.Show($"Update available!\n\n{update.changelog}\n\nDownload now?", "Update", MessageBoxButtons.YesNo);
                        if (result == DialogResult.Yes)
                        {
                            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                            {
                                FileName = update.url,
                                UseShellExecute = true
                            });
                            Application.Exit();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Update check failed: " + ex.Message);
            }
        }
    }
}
