﻿using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using GoogleTestAdapter.Helpers;
using GoogleTestAdapter.Framework;
using GoogleTestAdapter.TestAdapter.Framework;
using GoogleTestAdapter.TestAdapter.Settings;

namespace GoogleTestAdapter.TestAdapter
{
    [DefaultExecutorUri(TestExecutor.ExecutorUriString)]
    [FileExtension(".exe")]
    public class TestDiscoverer : ITestDiscoverer
    {
        private TestEnvironment TestEnvironment { get; set; }
        private GoogleTestDiscoverer Discoverer { get; set; }

        public TestDiscoverer() : this(null)
        {
            ExecutionTracer.Trace("TestDiscoverer instantiated");
        }

        public TestDiscoverer(TestEnvironment testEnvironment)
        {
            TestEnvironment = testEnvironment;
            Discoverer = new GoogleTestDiscoverer(TestEnvironment);
        }


        public void DiscoverTests(IEnumerable<string> executables, IDiscoveryContext discoveryContext,
            IMessageLogger logger, ITestCaseDiscoverySink discoverySink)
        {
            ExecutionTracer.Trace("DiscoverTests called");
            ILogger loggerAdapter = new VsTestFrameworkLogger(logger);
            ExecutionTracer.Trace("Starting discovery");
            if (TestEnvironment == null || TestEnvironment.Options.GetType() == typeof(Options)) // check whether we have a mock
            {
                var settingsProvider = discoveryContext.RunSettings.GetSettings(GoogleTestConstants.SettingsName) as RunSettingsProvider;
                RunSettings ourRunSettings = settingsProvider != null ? settingsProvider.Settings : new RunSettings();

                TestEnvironment = new TestEnvironment(new Options(ourRunSettings, loggerAdapter), loggerAdapter);
                Discoverer = new GoogleTestDiscoverer(TestEnvironment);
            }
            ExecutionTracer.Trace("Initialized test environment");
            TestEnvironment.LogInfo("Trace file: " + ExecutionTracer.TraceFile);

            try
            {
                VsTestFrameworkReporter reporter = new VsTestFrameworkReporter(discoverySink);
                Discoverer.DiscoverTests(executables, reporter);
            }
            catch (Exception e)
            {
                TestEnvironment.LogError("Exception while discovering tests: " + e);
            }
            ExecutionTracer.Trace("Finished test discovery");
            ExecutionTracer.Flush();
        }

    }

}