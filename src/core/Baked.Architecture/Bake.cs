﻿using Baked.Architecture;
using Baked.Branding;
using System.Globalization;

namespace Baked;

public class Bake(IBanner _banner, Func<Application> _newApplication,
    RunFlags _runFlags = RunFlags.Start
)
{
    public static Bake New
    {
        get
        {
            var runFlags = RunFlags.Start;
            if (Environment.GetCommandLineArgs().Contains("--bake"))
            {
                runFlags |= RunFlags.Bake;
            }

            if (Environment.GetCommandLineArgs().Contains("--no-start"))
            {
                runFlags ^= RunFlags.Start;
            }

            return new(new BakedBanner(), () => new(new()),
                _runFlags: runFlags
            );
        }
    }

    public Application Application(Action<ApplicationDescriptor> describe)
    {
        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        _banner.Print();
        var descriptor = new ApplicationDescriptor();

        describe(descriptor);

        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Nfr")
        {
            return _newApplication().With(descriptor, RunFlags.Start | RunFlags.Bake);
        }

        return _newApplication().With(descriptor, _runFlags);
    }
}