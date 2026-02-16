using System;
using System.Threading.Tasks;
using NetSparkleUpdater;
using NetSparkleUpdater.Enums;
using NetSparkleUpdater.Interfaces;
using NetSparkleUpdater.SignatureVerifiers;
using OdyTools.Data;

namespace OdyTools.Windows
{
    public class UpdateManager : IDisposable
    {
        private readonly GlobalSettings _settings;
        private SparkleUpdater _sparkle;
        private bool _initialized;

        public UpdateManager(bool silent = false)
        {
            _settings = new GlobalSettings();
            SilentCheck = silent;
        }

        public bool SilentCheck { get; set; } = true;

        public string StableAppcastUrl { get; set; } =
            "https://github.com/th3w1zard1/Andastra/releases/latest/download/appcast.xml";

        public string BetaAppcastUrl { get; set; } =
            "https://github.com/th3w1zard1/Andastra/releases/download/bleeding-edge/appcast-beta.xml";

        public string Ed25519PublicKey { get; set; } = "";

        public bool CheckOnStartup { get; set; } = true;

        public void Initialize()
        {
            if (_initialized)
            {
                return;
            }

            string appcastUrl = _settings.UseBetaChannel ? BetaAppcastUrl : StableAppcastUrl;
            ISignatureVerifier verifier = string.IsNullOrWhiteSpace(Ed25519PublicKey)
                ? (ISignatureVerifier)new Ed25519Checker(SecurityMode.Unsafe, "")
                : new Ed25519Checker(SecurityMode.Strict, Ed25519PublicKey);

            _sparkle = new SparkleUpdater(appcastUrl, verifier)
            {
                RelaunchAfterUpdate = true
            };
            _initialized = true;
        }

        public void Start()
        {
            Initialize();
            if (CheckOnStartup)
            {
                _sparkle.StartLoop(true);
            }
        }

        public async Task CheckForUpdatesAsync(bool silent = false)
        {
            SilentCheck = silent;
            Initialize();
            await Task.Run(() =>
            {
                if (SilentCheck)
                {
                    _sparkle.CheckForUpdatesQuietly();
                }
                else
                {
                    _sparkle.CheckForUpdatesAtUserRequest();
                }
            });
        }

        public void CheckForUpdates(bool silent = false)
        {
            Task.Run(async () => await CheckForUpdatesAsync(silent));
        }

        public void Stop()
        {
            // NetSparkleUpdater 3.x uses IDisposable; no separate Stop() method
            _sparkle?.Dispose();
            _sparkle = null;
            _initialized = false;
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
