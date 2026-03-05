using System;
using System.IO;
using System.Reflection;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Headless;
using BioWare.Common;
using OdyTools.Data;
using OdyTools.Editors.DLG;
using OdyTools.Editors;
using OdyTools.NET;
using OdyTools.Utils;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;

namespace OdyTools.Tests
{
    [NonParallelizable]
    public class SaveRecoverySettingsTests
    {
        private static MethodInfo GetRequiredNonPublicMethod(Type type, string methodName, BindingFlags flags)
        {
            MethodInfo method = type.GetMethod(methodName, flags);
            Assert.That(method, Is.Not.Null, $"Expected method '{methodName}' on {type.Name}");
            return method;
        }

        private static MethodInfo GetConfigureCrashRecoveryStartupMethod()
        {
            return GetRequiredNonPublicMethod(
                typeof(App),
                "ConfigureCrashRecoveryStartup",
                BindingFlags.NonPublic | BindingFlags.Static);
        }

        private sealed class TestEditor : Editor
        {
            public TestEditor()
                : base(null, "Test Editor", "", new ResourceType[0], new ResourceType[0], null)
            {
            }

            public bool ReadAutosaveEnabled() => IsAutosaveEnabled;
            public int ReadAutosaveIntervalMinutes() => AutosaveIntervalMinutes;
            public bool ReadBackupsEnabled() => CreateBackupsOnSave;
            public int ReadBackupCount() => BackupCount;

            public override Tuple<byte[], byte[]> Build()
            {
                return new Tuple<byte[], byte[]>(new byte[] { 0x01 }, new byte[] { 0x01 });
            }
        }

        [Test]
        public void GlobalSettings_AutosaveIsAlwaysOn_AndUsesFixedInterval()
        {
            var settings = GlobalSettings.Instance;

            int oldAutosaveInterval = settings.AutosaveIntervalMinutes;
            int oldMaxBackups = settings.MaxBackupCount;
            int oldCrashInterval = settings.CrashRecoveryIntervalSeconds;

            try
            {
                settings.AutosaveIntervalMinutes = 0;
                settings.MaxBackupCount = 0;
                settings.CrashRecoveryIntervalSeconds = 1;

                Assert.That(settings.AutosaveEnabled, Is.True);
                Assert.That(settings.AutosaveIntervalMinutes, Is.EqualTo(GlobalSettings.ManagedAutosaveIntervalMinutes));
                Assert.That(settings.MaxBackupCount, Is.EqualTo(1));
                Assert.That(settings.CrashRecoveryIntervalSeconds, Is.EqualTo(5));
            }
            finally
            {
                settings.AutosaveIntervalMinutes = oldAutosaveInterval;
                settings.MaxBackupCount = oldMaxBackups;
                settings.CrashRecoveryIntervalSeconds = oldCrashInterval;
            }
        }

        [Test]
        public void GlobalSettings_RemovesLegacyAutosaveKeys_OnCleanup()
        {
            var settings = GlobalSettings.Instance;

            settings.SetValue("AutosaveEnabled", false);
            settings.SetValue("AutosaveIntervalMinutes", 17);
            settings.SetValue("OdyToolDLG.autosave_enabled", false);
            settings.SetValue("OdyToolDLG.autosave_interval_minutes", 19);

            MethodInfo cleanupMethod = GetRequiredNonPublicMethod(
                typeof(GlobalSettings),
                "CleanupLegacyAutosaveSettings",
                BindingFlags.NonPublic | BindingFlags.Instance);
            cleanupMethod.Invoke(settings, null);

            Assert.That(settings.GetValue<object>("AutosaveEnabled", null), Is.Null);
            Assert.That(settings.GetValue<object>("AutosaveIntervalMinutes", null), Is.Null);
            Assert.That(settings.GetValue<object>("OdyToolDLG.autosave_enabled", null), Is.Null);
            Assert.That(settings.GetValue<object>("OdyToolDLG.autosave_interval_minutes", null), Is.Null);
            Assert.That(settings.AutosaveEnabled, Is.True);
            Assert.That(settings.AutosaveIntervalMinutes, Is.EqualTo(GlobalSettings.ManagedAutosaveIntervalMinutes));
        }

        [Test]
        public async Task Editor_DefaultSaveBehavior_UsesAlwaysOnAutosave_AndGlobalBackupSettings()
        {
            var settings = GlobalSettings.Instance;

            bool oldAutosaveEnabled = settings.AutosaveEnabled;
            int oldAutosaveInterval = settings.AutosaveIntervalMinutes;
            bool oldBackupsEnabled = settings.BackupsEnabled;
            int oldMaxBackups = settings.MaxBackupCount;

            try
            {
                settings.AutosaveEnabled = false;
                settings.AutosaveIntervalMinutes = 7;
                settings.BackupsEnabled = false;
                settings.MaxBackupCount = 9;

                using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
                {
                    await session.Dispatch(() =>
                    {
                        var editor = new TestEditor();
                        Assert.That(editor.ReadAutosaveEnabled(), Is.True);
                        Assert.That(editor.ReadAutosaveIntervalMinutes(), Is.EqualTo(GlobalSettings.ManagedAutosaveIntervalMinutes));
                        Assert.That(editor.ReadBackupsEnabled(), Is.False);
                        Assert.That(editor.ReadBackupCount(), Is.EqualTo(9));
                    }, CancellationToken.None);
                }
            }
            finally
            {
                settings.AutosaveEnabled = oldAutosaveEnabled;
                settings.AutosaveIntervalMinutes = oldAutosaveInterval;
                settings.BackupsEnabled = oldBackupsEnabled;
                settings.MaxBackupCount = oldMaxBackups;
            }
        }

        [Test]
        public void CrashRecoveryService_IntervalResolution_UsesGlobalSettingWithMinimum()
        {
            var settings = GlobalSettings.Instance;
            int oldCrashInterval = settings.CrashRecoveryIntervalSeconds;

            MethodInfo intervalMethod = typeof(EditorCrashRecoveryService).GetMethod(
                "GetBackupIntervalSeconds",
                BindingFlags.NonPublic | BindingFlags.Static);

            Assert.That(intervalMethod, Is.Not.Null);

            try
            {
                settings.CrashRecoveryIntervalSeconds = 2;
                object clampedResult = intervalMethod.Invoke(null, null);
                Assert.That(clampedResult, Is.EqualTo(5));

                settings.CrashRecoveryIntervalSeconds = 45;
                object configuredResult = intervalMethod.Invoke(null, null);
                Assert.That(configuredResult, Is.EqualTo(45));
            }
            finally
            {
                settings.CrashRecoveryIntervalSeconds = oldCrashInterval;
            }
        }

        [Test]
        public void DlgSettings_AutosaveIsAlwaysOn_AndUsesFixedInterval()
        {
            var settings = GlobalSettings.Instance;
            var dlgSettings = new DLGSettings();

            bool oldAutosaveEnabled = dlgSettings.AutosaveEnabled(settings.AutosaveEnabled);
            int oldAutosaveInterval = dlgSettings.AutosaveIntervalMinutes(settings.AutosaveIntervalMinutes);
            bool oldBackupsEnabled = dlgSettings.BackupsEnabled(settings.BackupsEnabled);
            int oldMaxBackups = dlgSettings.MaxBackupCount(settings.MaxBackupCount);

            try
            {
                dlgSettings.SetAutosaveEnabled(false);
                dlgSettings.SetAutosaveIntervalMinutes(11);
                dlgSettings.SetBackupsEnabled(false);
                dlgSettings.SetMaxBackupCount(8);

                Assert.That(dlgSettings.AutosaveEnabled(true), Is.True);
                Assert.That(dlgSettings.AutosaveEnabled(false), Is.True);
                Assert.That(dlgSettings.AutosaveIntervalMinutes(3), Is.EqualTo(GlobalSettings.ManagedAutosaveIntervalMinutes));
                Assert.That(dlgSettings.AutosaveIntervalMinutes(13), Is.EqualTo(GlobalSettings.ManagedAutosaveIntervalMinutes));
                Assert.That(dlgSettings.BackupsEnabled(true), Is.False);
                Assert.That(dlgSettings.MaxBackupCount(5), Is.EqualTo(8));
            }
            finally
            {
                dlgSettings.SetAutosaveEnabled(oldAutosaveEnabled);
                dlgSettings.SetAutosaveIntervalMinutes(oldAutosaveInterval);
                dlgSettings.SetBackupsEnabled(oldBackupsEnabled);
                dlgSettings.SetMaxBackupCount(oldMaxBackups);
            }
        }

        [Test]
        public void DlgSettings_BackupCount_ClampsToMinimum()
        {
            var settings = GlobalSettings.Instance;
            var dlgSettings = new DLGSettings();

            int oldMaxBackups = dlgSettings.MaxBackupCount(settings.MaxBackupCount);

            try
            {
                dlgSettings.SetMaxBackupCount(0);

                Assert.That(dlgSettings.MaxBackupCount(99), Is.EqualTo(1));
            }
            finally
            {
                dlgSettings.SetMaxBackupCount(oldMaxBackups);
            }
        }

        [Test]
        public void DlgSettings_WhenAutosaveSettingsMissingOrInvalid_StillUsesAlwaysOnDefaults()
        {
            var settings = GlobalSettings.Instance;
            var dlgSettings = new DLGSettings();

            object oldAutosaveRaw = settings.GetValue("OdyToolDLG.autosave_enabled", (object)null);
            object oldIntervalRaw = settings.GetValue("OdyToolDLG.autosave_interval_minutes", (object)null);

            try
            {
                settings.SetValue<object>("OdyToolDLG.autosave_enabled", null);
                settings.SetValue<object>("OdyToolDLG.autosave_interval_minutes", "not_an_int");

                Assert.That(dlgSettings.AutosaveEnabled(defaultValue: true), Is.True);
                Assert.That(dlgSettings.AutosaveEnabled(defaultValue: false), Is.True);
                Assert.That(dlgSettings.AutosaveIntervalMinutes(defaultValue: 13), Is.EqualTo(GlobalSettings.ManagedAutosaveIntervalMinutes));
            }
            finally
            {
                settings.SetValue<object>("OdyToolDLG.autosave_enabled", oldAutosaveRaw);
                settings.SetValue<object>("OdyToolDLG.autosave_interval_minutes", oldIntervalRaw);
            }
        }

        [Test]
        public void Autosave_WorkingCopyPath_IsManagedAndNeverEqualsSourceFilepath()
        {
            string sourceFilePath = Path.Combine(Path.GetTempPath(), "andastra_source_test", "example.utc");
            string autosavePath = AtomicFileWriter.GetAutosavePathForFile(sourceFilePath);

            Assert.That(autosavePath, Is.Not.EqualTo(sourceFilePath));
            Assert.That(Path.GetExtension(autosavePath), Is.EqualTo(".autosave"));
            Assert.That(autosavePath, Does.Contain(Path.Combine("OdyToolsV3", "Autosave")));
            Assert.That(Path.GetDirectoryName(autosavePath), Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public void App_CrashRecoveryStartup_Disabled_DoesNothing()
        {
            MethodInfo configureMethod = GetConfigureCrashRecoveryStartupMethod();

            bool started = false;
            bool restored = false;
            bool cleanExit = false;
            int scheduledCount = 0;
            int shutdownHandlerRegistrations = 0;

            object[] args =
            {
                false,
                (Action)(() => started = true),
                (Func<Task<bool>>)(() => Task.FromResult(true)),
                (Action)(() => restored = true),
                (Action)(() => cleanExit = true),
                (Action<Func<Task>>)(_ => scheduledCount++),
                (Action<EventHandler<ShutdownRequestedEventArgs>>)(_ => shutdownHandlerRegistrations++)
            };

            configureMethod.Invoke(null, args);

            Assert.That(started, Is.False);
            Assert.That(restored, Is.False);
            Assert.That(cleanExit, Is.False);
            Assert.That(scheduledCount, Is.EqualTo(0));
            Assert.That(shutdownHandlerRegistrations, Is.EqualTo(0));
        }

        [Test]
        public void App_CrashRecoveryStartup_Enabled_SchedulesRecoveryAndRegistersCleanExit()
        {
            MethodInfo configureMethod = GetConfigureCrashRecoveryStartupMethod();

            bool started = false;
            bool restored = false;
            bool cleanExit = false;
            EventHandler<ShutdownRequestedEventArgs> capturedShutdownHandler = null;

            object[] args =
            {
                true,
                (Action)(() => started = true),
                (Func<Task<bool>>)(() => Task.FromResult(true)),
                (Action)(() => restored = true),
                (Action)(() => cleanExit = true),
                (Action<Func<Task>>)(work => work().GetAwaiter().GetResult()),
                (Action<EventHandler<ShutdownRequestedEventArgs>>)(handler => capturedShutdownHandler = handler)
            };

            configureMethod.Invoke(null, args);

            Assert.That(started, Is.True);
            Assert.That(restored, Is.True);
            Assert.That(capturedShutdownHandler, Is.Not.Null);

            capturedShutdownHandler(null, null);
            Assert.That(cleanExit, Is.True);
        }

        [Test]
        public void App_CrashRecoveryStartup_Enabled_NoRestoreWhenRecoveryDeclined()
        {
            MethodInfo configureMethod = GetConfigureCrashRecoveryStartupMethod();

            bool restored = false;

            object[] args =
            {
                true,
                (Action)(() => { }),
                (Func<Task<bool>>)(() => Task.FromResult(false)),
                (Action)(() => restored = true),
                (Action)(() => { }),
                (Action<Func<Task>>)(work => work().GetAwaiter().GetResult()),
                (Action<EventHandler<ShutdownRequestedEventArgs>>)(_ => { })
            };

            configureMethod.Invoke(null, args);

            Assert.That(restored, Is.False);
        }
    }
}
