using System;
using System.Threading.Tasks;
using Avalonia.Threading;
using OdyTools.Editors;

namespace OdyTools.Utils
{
    public sealed class AutosaveService : IDisposable
    {
        private readonly Editor _editor;
        private readonly DispatcherTimer _timer;
        private bool _isSaving;
        private DateTime _lastEditUtc;

        public DateTime? LastAutosaveUtc { get; private set; }

        public AutosaveService(Editor editor, int intervalMinutes)
        {
            _editor = editor ?? throw new ArgumentNullException(nameof(editor));
            _lastEditUtc = DateTime.UtcNow;
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMinutes(Math.Max(1, intervalMinutes))
            };
            _timer.Tick += OnTick;
        }

        public void Start() => _timer.Start();

        public void Stop() => _timer.Stop();

        public void NotifyEdited()
        {
            _lastEditUtc = DateTime.UtcNow;
        }

        public void ClearForCurrentFile()
        {
            var (filepath, _, _) = _editor.GetRecoveryInfo();
            if (string.IsNullOrWhiteSpace(filepath))
            {
                return;
            }
            AtomicFileWriter.DeleteAutosaveFor(filepath);
        }

        private async void OnTick(object sender, EventArgs e)
        {
            if (_isSaving || !_editor.IsDirty)
            {
                return;
            }

            if ((DateTime.UtcNow - _lastEditUtc).TotalSeconds < 10)
            {
                return;
            }

            var (filepath, _, _) = _editor.GetRecoveryInfo();
            if (string.IsNullOrWhiteSpace(filepath))
            {
                return;
            }

            Tuple<byte[], byte[]> built;
            try
            {
                built = _editor.Build();
            }
            catch
            {
                return;
            }

            if (built?.Item1 == null || built.Item1.Length == 0)
            {
                return;
            }

            string autosavePath = AtomicFileWriter.GetAutosavePathForFile(filepath);
            var options = new AtomicWriteOptions
            {
                CreateBackup = false,
                VerifyLength = true,
                RetryCount = 2,
                RetryDelayMs = 200,
                MaxBackups = 1
            };

            _isSaving = true;
            try
            {
                byte[] data = built.Item1;
                await Task.Run(() => AtomicFileWriter.WriteAtomic(autosavePath, data, options));
                LastAutosaveUtc = DateTime.UtcNow;
            }
            catch
            {
                // Intentionally ignored to avoid disrupting editor workflows.
            }
            finally
            {
                _isSaving = false;
            }
        }

        public void Dispose()
        {
            _timer.Tick -= OnTick;
            _timer.Stop();
        }
    }
}
