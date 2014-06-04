namespace Module._2
{
    using System;
    using System.ComponentModel;
    using System.IO;

    /// <summary>
    /// Class Configuration. This class cannot be inherited.
    /// </summary>
    public sealed class Configuration
        : INotifyPropertyChanged
    {
        #region Private Member

        private readonly FileSystemWatcher watcher;

        private TimeSpan refreshTime;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Configuration"/> class.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        public Configuration(string filePath)
        {
            this.watcher = new FileSystemWatcher(filePath);
            this.watcher.Changed += this.OnConfigurationFileChanged;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="Configuration"/> class.
        /// </summary>
        ~Configuration()
        {
            this.watcher.Changed -= this.OnConfigurationFileChanged;
        }

        #endregion

        #region Public Events

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the refresh time.
        /// </summary>
        /// <value>The refresh time.</value>
        public TimeSpan RefreshTime
        {
            get
            {
                return this.refreshTime;
            }

            set
            {
                this.refreshTime = value;

                if (null != this.PropertyChanged)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("RefreshTime"));
                }
            }
        }

        #endregion

        #region Private Functions

        private void OnConfigurationFileChanged(object sender, FileSystemEventArgs e)
        {
            var configuValue = File.ReadAllText(e.FullPath);
            var parts = configuValue.Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 2)
            {
                this.RefreshTime = TimeSpan.FromSeconds(double.Parse(parts[1]));
            }
        }

        #endregion
    }
}
