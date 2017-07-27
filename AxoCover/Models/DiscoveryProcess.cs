﻿using AxoCover.Common.Events;
using AxoCover.Common.Extensions;
using AxoCover.Common.Models;
using AxoCover.Common.ProcessHost;
using AxoCover.Common.Runner;
using AxoCover.Common.Settings;
using AxoCover.Models.Extensions;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;

namespace AxoCover.Models
{
  public class DiscoveryProcess : TestProcess<ITestDiscoveryService>, ITestDiscoveryMonitor
  {
    public event EventHandler<EventArgs<string>> MessageReceived;

    private DiscoveryProcess(IHostProcessInfo hostProcess, string[] testPlatformAssemblies) :
      base(hostProcess.Embed(new ServiceProcessInfo(RunnerMode.Discovery, testPlatformAssemblies))) { }
    
    public static DiscoveryProcess Create(string[] testPlatformAssemblies, TestPlatform testPlatform = TestPlatform.x86)
    {
      var hostProcess = new PlatformProcessInfo(testPlatform);
      return new DiscoveryProcess(hostProcess, testPlatformAssemblies);
    }

    void ITestDiscoveryMonitor.RecordMessage(TestMessageLevel testMessageLevel, string message)
    {
      MessageReceived?.Invoke(this, new EventArgs<string>(message));
    }

    public TestCase[] DiscoverTests(TestDiscoveryTask[] testDiscoveryTasks, string runSettingsPath)
    {
      return TestService.DiscoverTests(testDiscoveryTasks, runSettingsPath);
    }
  }
}
